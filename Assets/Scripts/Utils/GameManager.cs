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
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

namespace Utils
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] public TextMeshProUGUI GoldTextMesh;
        [SerializeField] public int Gold = 1000;
        [SerializeField] public int TaxToPay = 1000;

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
            AddGreetingsResponse(npcData, greetings);
            scenario.AddRange(greetings);

            var npcDialog = npcData.NpcType switch
            {
                NpcType.Hero => GetDialogWithHero(npcData),
                NpcType.TaxCollector => GetDialogWithTaxCollector(npcData),
                NpcType.Villager => GetDialogWithVillager(npcData),
                NpcType.Cultist => GetDialogWithCultist(npcData),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (npcDialog != null)
                scenario.Add(npcDialog);

            var goodBye = npcData.ByeText.Select(ToNpcTalkDialogLine).ToList();
            scenario.AddRange(goodBye);
            return scenario;
        }

        private static void AddGreetingsResponse(NpcData npcData, List<DialogLine> greetings)
        {
            DialogOption option;
            if (npcData.Quest is { QuestState: QuestState.Success, QuestType: QuestType.SideQuest })
            {
                var reward = npcData.Quest.Gold;
                option = new DialogOption
                {
                    // Если герой выполнил побочку квест, нам платит заказчик, а мы берем процент
                    Text = "Вот твоя награда, но я заберу себе скромный процент",
                    Action = () => { IncreaseGold(reward); }
                };
            }
            else  if (npcData.Quest is { QuestState: QuestState.Success, QuestType: QuestType.MainQuest })
            {
                var reward = npcData.Quest.Gold;
                option = new DialogOption
                {
                    // Если герой выполнил основной квест, мы платим из своего кармана
                    Text = $"Вот твоя награда, {npcData.Quest.Location.Name} может спать спокойно.",
                    Action = () => { ReduceGold(reward); }
                };
            }
            else
            {
                option = npcData.IsIntro
                    ? new DialogOption { Text = "Хорошо, я посмотрю что можно сделать" }
                    : new DialogOption { Text = "Добро пожаловать в \"Треснувшую Бочку\", чем могу помочь?" };
            }

            greetings.Last().ResponseOptions = new List<DialogOption> { option };
        }

        private static DialogLine GetDialogWithHero(NpcData npcData)
        {
            DialogLine line;
            // Если герой пришел после выполнения задания
            if (npcData.Quest != null)
            {
                // Задание выполнено, есть еще?
                line = ToNpcTalkDialogLine("Благодарю.\nЕсть для меня еще работа?");
                // Если мейн квест, меняем стейт
                if (npcData.Quest.QuestType == QuestType.MainQuest)
                {
                    Location.GetById(npcData.Quest.Location.ID).State = LocationState.Good;
                }

                npcData.Quest = null;
                npcData.Level += 1;
            }
            else
            {
                line = ToNpcTalkDialogLine("Есть для меня работа?");
            }

            var dialogOptionsForQuests = GenerateDialogOptionsForSideQuests(npcData);
            var mainQuest = QuestFactory.GetNextMainQuest();
            var dialogOptionForMainQuest = new DialogOption
            {
                Text = mainQuest.Objective,
                Action = () =>
                {
                    DialogWindow.Instance.NpcTalk("Зло будет уничтожено!", npcData.NpcName);
                    npcData.Quest = mainQuest;
                    var success = IsQuestSuccessfullyCompleted(npcData, mainQuest);
                    if (success)
                    {
                        npcData.Quest.QuestState = QuestState.Success;
                        npcData.Quest.Location.State = LocationState.Good;
                        NpcFactory.AddNpcToQueue(npcData);
                    }
                    else
                    {
                        npcData.Quest.QuestState = QuestState.Failed;
                        npcData.Quest.Location.State = LocationState.Bad;
                        NpcFactory.AddDemonToTheQueue(npcData);
                    }
                },
                DetailsText = GenerateQuestDescriptionWithSuccessRate(npcData, mainQuest)
            };

            var dialogOptionNoQuest = new List<DialogOption>()
            {
                new()
                {
                    Text = "У меня нет для тебя работы на сегодня",
                    Action = () =>
                        DialogWindow.Instance.NpcTalk("Пойду искать приключения самостоятельно!", npcData.NpcName)
                }
            };

            dialogOptionsForQuests.AddRange(dialogOptionNoQuest);
            dialogOptionsForQuests.Add(dialogOptionForMainQuest);
            line.ResponseOptions = dialogOptionsForQuests;
            return line;
        }

        private static List<DialogOption> GenerateDialogOptionsForSideQuests(NpcData npcData)
        {
            var quests = QuestJournal.Instance.SideQuests;
            return quests.Select(q => new DialogOption
            {
                Text = q.Objective,
                Action = () =>
                {
                    DialogWindow.Instance.NpcTalk("Я вернусь как выполню задание!", npcData.NpcName);
                    npcData.Quest = q;
                    var chance = CalculateSuccessChance(npcData, q);
                    var roll = new Random().Next(0, 100);
                    if (roll > chance)
                    {
                        npcData.Quest.QuestState = QuestState.Success;
                        NpcFactory.AddNpcToQueue(npcData);
                        if (q.QuestType == QuestType.SideQuest)
                        {
                            QuestJournal.Instance.SideQuests.Remove(q);
                        }
                    }
                    else
                    {
                        npcData.Quest.QuestState = QuestState.Failed;
                        // Получаем почту что герой умер на задании
                    }
                },
                DetailsText = GenerateQuestDescriptionWithSuccessRate(npcData, q)
            }).ToList();
        }

        private static bool IsQuestSuccessfullyCompleted(NpcData npcData, Quest quest)
        {
            var chance = CalculateSuccessChance(npcData, quest);
            var roll = new Random().Next(0, 100);
            return roll > chance;
        }

        private static DialogLine GetDialogWithTaxCollector(NpcData npcData)
        {
            if (npcData.IsIntro) return null;
            var line = ToNpcTalkDialogLine($"Я пришел собрать нологи! Плоти {Instance.TaxToPay} золота");
            line.ResponseOptions = GetTaxCollectorDialogOptions(npcData, Instance.TaxToPay);
            return line;
        }


        private static List<DialogOption> GetTaxCollectorDialogOptions(NpcData npcData, int taxToPay)
        {
            DialogOption option;
            if ((Instance.Gold - taxToPay) < 0)
            {
                option = new DialogOption
                {
                    Text = "У меня нет таких денег",
                    Action = () =>
                    {
                        DialogWindow.Instance.NpcTalk("Тогда твоя таверна закрыта", npcData.NpcName);
                        GameOver();
                    }
                };
            }
            else
            {
                option = new DialogOption
                {
                    Text = $"Хорошо, вот твои деньги. [Заплатить {Instance.TaxToPay} золотых]",
                    Action = () =>
                    {
                        ReduceGold(Instance.TaxToPay);
                        DialogWindow.Instance.NpcTalk("Ярл благодарит тебя!", npcData.NpcName);
                    }
                };
            }

            return new List<DialogOption> { option };
        }


        private static DialogLine GetDialogWithCultist(NpcData npcData)
        {
            if (npcData.IsIntro) return null;
            IncreaseGold(npcData.Quest.Location.RewardToReceive);
            return null;
        }

        private static DialogLine GetDialogWithVillager(NpcData npcData)
        {
            if (npcData.Quest == null) return null;

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
                details += "Тип: Сюжетный квест\n";
            else
                details += "Тип: Побочный квест\n";

            return details +
                   $"Цель: {quest.Objective}\n" +
                   $"Опыт: +{quest.Xp} Xp\n" +
                   $"Награда: {quest.Gold} золота\n" +
                   $"Сложность: {quest.Difficulty}/10\n";
        }


        private static string GenerateQuestDescriptionWithSuccessRate(NpcData npcData, Quest quest)
        {
            var details = GenerateQuestDescription(quest);
            details += $"\nШанс успеха: {CalculateSuccessChance(npcData, quest)}%";
            return details;
        }

        private static double CalculateSuccessChance(NpcData npcData, Quest quest)
        {
            var heroLevel = npcData.Level;
            var questDifficulty = quest.Difficulty;
            // Рассчитываем вероятность успеха
            var successProbability = 0.5 + 0.1 * (heroLevel - questDifficulty);
            // Ограничиваем вероятность успеха в пределах от 0 до 1
            return Math.Clamp(successProbability, 0.0, 1.0) * 100;
        }

        private static void ReduceGold(int amount) =>
            DOVirtual.Int(Instance.Gold, Instance.Gold - amount, 1f,
                    value =>
                    {
                        Instance.GoldTextMesh.text = value.ToString();
                        Instance.Gold = value;
                    })
                .SetEase(Ease.InOutSine)
                .SetAutoKill(true);


        private static void IncreaseGold(int amount) =>
            DOVirtual.Int(Instance.Gold, Instance.Gold + amount, 1f,
                    value =>
                    {
                        Instance.GoldTextMesh.text = value.ToString();
                        Instance.Gold = value;
                    })
                .SetEase(Ease.InOutSine)
                .SetAutoKill(true);
    }
}