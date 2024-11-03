using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Dialogs;
using Npc;
using Quests;
using TMPro;
using Ui;
using UnityEngine;
using Random = System.Random;

namespace Utils
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] public TextMeshProUGUI GoldTextMesh;
        [SerializeField] public int Gold = 1000;
        [SerializeField] public int TaxToPay = 300;

        public IEnumerator Start()
        {
            while (true)
            {
                yield return GameplayLoop();
            }
        }

        private static IEnumerator GameplayLoop()
        {
            var npcData = NpcFactory.GetNextVisitor();
            var npc = NpcManager.Instance.CreateNpc(npcData);
            var tavernNpc = npc.GetComponent<TavernNpc>();
            yield return tavernNpc.WalkIn();

            var scenario = GetDialogScenario(npcData);

            foreach (var dialogLine in scenario)
            {
                DialogWindow.Instance.NpcTalk(dialogLine.Text, npcData.NpcName);
                yield return new WaitUntil(() => DialogWindow.Instance.CanContinue);

                if (dialogLine.ResponseOptions != null)
                {
                    DialogWindow.Instance.ShowPlayerDialogOptions(dialogLine.ResponseOptions);
                    yield return new WaitUntil(() => DialogWindow.Instance.CanContinue);
                }
            }

            DialogWindow.Instance.Hide();
            yield return tavernNpc.WalkOut();
            // Если захотим добавтить паузу между посетителями
            // yield return new WaitForSeconds(Random.Range(0.8f, 3.2f));
        }


        // Получить сценарий для диалога, со всеми репликами
        private static List<DialogLine> GetDialogScenario(NpcData npcData)
        {
            var scenario = new List<DialogLine>();
            var greetings = npcData.GreetingsText.Select(ToNpcTalkDialogLine).ToList();

            if (!npcData.IsIntro)
            {
                greetings.Last().ResponseOptions = new List<DialogOption>()
                {
                    new() { Text = "Добро пожаловать в \"Треснувшую Бочку\", чем могу помочь?" }
                };
            }
            else
            {
                greetings.Last().ResponseOptions = new List<DialogOption>()
                {
                    new() { Text = "Хорошо, я посмотрю что можно сделать" }
                };
            }

            scenario.AddRange(greetings);

            var npcDialog = npcData.NpcType switch
            {
                NpcType.Hero => GetDialogWithHero(npcData),
                NpcType.TaxCollector => GetDialogWithTaxCollector(npcData),
                NpcType.Villager => GetDialogWithVillager(npcData),
                NpcType.Cultist => GetDialogWithVillager(npcData),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (npcDialog != null)
                scenario.Add(npcDialog);

            var goodBye = npcData.ByeText.Select(ToNpcTalkDialogLine).ToList();
            scenario.AddRange(goodBye);
            return scenario;
        }

        private static DialogLine GetDialogWithHero(NpcData npcData)
        {
            var quest = ToNpcTalkDialogLine("Есть для меня работа?");
            var quests = QuestJournal.Instance.SideQuests;

            var options = quests.Select(q => new DialogOption
            {
                Text = q.Objective,
                Action = () =>
                {
                    DialogWindow.Instance.NpcTalk("Я вернусь за наградой позже!", npcData.NpcName);
                    npcData.Quest = q;
                    var chance = CalculateSuccessChance(npcData, q);
                    var roll = new Random().Next(0, 100);
                    if (roll > chance)
                    {
                        npcData.Quest.QuestState = QuestState.Success;
                        NpcFactory.AddNpcToQueue(npcData);
                    }
                    else
                    {
                        npcData.Quest.QuestState = QuestState.Failed;
                    }
                },
                DetailsText = GenerateQuestDescriptionWithSuccessRate(npcData, q)
            }).ToList();

            var mainQuest = new List<DialogOption>()
            {
                new()
                {
                    Text = "У меня нет для тебя работы на сегодня",
                    Action = () =>
                        DialogWindow.Instance.NpcTalk("Пойду искать приключения самостоятельно!", npcData.NpcName)
                }
            };

            var emptyOption = new List<DialogOption>()
            {
                new()
                {
                    Text = "У меня нет для тебя работы на сегодня",
                    Action = () =>
                        DialogWindow.Instance.NpcTalk("Пойду искать приключения самостоятельно!", npcData.NpcName)
                }
            };
            
            
            options.AddRange(emptyOption);
            quest.ResponseOptions = options;
            return quest;
        }

        private static DialogLine GetDialogWithTaxCollector(NpcData npcData)
        {
            if (npcData.IsIntro)
            {
                return null;
            }
            
            var line = ToNpcTalkDialogLine($"Я пришел собрать нологи! Плоти {Instance.TaxToPay} золота");
            line.ResponseOptions = new List<DialogOption>()
            {
                new()
                {
                    Text = $"Хорошо, вот твои деньги. [Заплатить {Instance.TaxToPay} золотых]",
                    Action = () =>
                    {
                        DOVirtual.Int(Instance.Gold, Instance.Gold - Instance.TaxToPay, 1f,
                                value =>
                                {
                                    Instance.GoldTextMesh.text = value.ToString();
                                    Instance.Gold = value;
                                })
                            .SetEase(Ease.InOutSine)
                            .SetAutoKill(true);
                        
                        DialogWindow.Instance.NpcTalk("Ярл благодарит тебя!", npcData.NpcName);
                    }
                },
                new()
                {
                    Text = "У меня нет таких денег",
                    Action = () =>
                    {
                        DialogWindow.Instance.NpcTalk("Тогда твоя таверна закрыта", npcData.NpcName);
                        GameOver();
                    }
                }
            };

            return line;
        }

        private static DialogLine GetDialogWithVillager(NpcData npcData)
        {
            if (npcData.Quest == null)
            {
                return null;
            }

            var quest = ToNpcTalkDialogLine(npcData.Quest.ApplicationText);
            quest.ResponseOptions = new List<DialogOption>()
            {
                new()
                {
                    Text = "Думаю, я знаю того кто сможет тебе помочь за небольшую плату",
                    Action = () =>
                    {
                        QuestJournal.Instance.AddSideQuest(npcData.Quest);
                        NewQuestPopup.Instance.Init(npcData.Quest.Objective);
                        NewQuestPopup.Instance.gameObject.SetActive(true);
                        DialogWindow.Instance.NpcTalk("Отлично!", npcData.NpcName);
                    },
                    DetailsText = GenerateQuestDescription(npcData.Quest)
                },
                new()
                {
                    Text = "Увы дружище, ничем не могу помочь",
                    Action = () => { DialogWindow.Instance.NpcTalk("Эхх, а я так надеялся...", npcData.NpcName); }
                }
            };

            return quest;
        }

        private static void GameOver()
        {
        }

        private static DialogLine ToNpcTalkDialogLine(string text) => new() { Text = text };

        private static string GenerateQuestDescription(Quest quest)
        {
            var details = "Задание\n\n";

            if (quest.QuestType == QuestType.MainQuest)
            {
                details += "Тип: Сюжетный квест\n";
            }
            else
            {
                details += "Тип: Побочный квест\n";
            }

            return details +
                   $"Цель: {quest.Objective}\n" +
                   $"Опыт: +{quest.Xp} Xp\n" +
                   $"Награда: {quest.Gold} золота\n" +
                   $"Сложность: {quest.Difficulty}/10\n";
        }


        public static string GenerateQuestDescriptionWithSuccessRate(NpcData npcData, Quest quest) =>
            "Задание\n" +
            $"Цель: {quest.Objective}\n" +
            $"Награда: {quest.Gold}\n" +
            $"Сложность: {quest.Difficulty}/10\n" +
            $"Шанс успеха: {CalculateSuccessChance(npcData, quest)}%";

        public static double CalculateSuccessChance(NpcData npcData, Quest quest)
        {
            var heroLevel = npcData.Level;
            var questDifficulty = quest.Difficulty;
            // Рассчитываем вероятность успеха
            var successProbability = 0.5 + 0.1 * (heroLevel - questDifficulty);

            // Ограничиваем вероятность успеха в пределах от 0 до 1
            return Math.Clamp(successProbability, 0.0, 1.0) * 100;
        }
    }
}