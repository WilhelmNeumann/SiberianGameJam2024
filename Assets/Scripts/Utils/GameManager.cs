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
            var npc = NpcManager.Instance.CreateNpc();
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
        }


        // Получить сценарий для диалога, со всеми репликами
        private static List<DialogLine> GetDialogScenario(NpcData npcData)
        {
            var scenario = new List<DialogLine>();
            var greetings = npcData.GreetingsText.Select(ToNpcTalkDialogLine).ToList();
            greetings.Last().ResponseOptions = new List<DialogOption>()
            {
                new() { Text = "Добро пожаловать в таверну \"Бухлишко и две шишки\", чем могу помочь?" }
            };

            scenario.AddRange(greetings);

            var npcDialog = npcData.NpcType switch
            {
                NpcType.Hero => GetDialogWithHero(npcData),
                NpcType.TaxCollector => GetDialogWithTaxCollector(npcData),
                NpcType.Villager => GetDialogWithVillager(npcData),
                NpcType.Cultist => throw new ArgumentOutOfRangeException(),
                _ => throw new ArgumentOutOfRangeException()
            };

            scenario.Add(npcDialog);

            var goodBye = npcData.ByeText.Select(ToNpcTalkDialogLine).ToList();
            scenario.AddRange(goodBye);
            return scenario;
        }

        private static DialogLine GetDialogWithHero(NpcData npcData)
        {
            var quest = ToNpcTalkDialogLine("Есть для меня работа?");
            var quests = QuestJournal.Instance.SideQuests;

            var responseOptions = new List<DialogOption>()
            {
                new()
                {
                    Text = "У меня нет для тебя работы на сегодня",
                    Action = () =>
                        DialogWindow.Instance.NpcTalk("Пойду искать приключения самостоятельно!", npcData.NpcName)
                }
            };

            var options = quests.Select(q => new DialogOption
            {
                Text = q.Objective,
                Action = () => { DialogWindow.Instance.NpcTalk("Охуенчик!", npcData.NpcName); }
            }).ToList();

            responseOptions.AddRange(options);
            quest.ResponseOptions = options;
            return quest;
        }

        private static DialogLine GetDialogWithTaxCollector(NpcData npcData)
        {
            var line = ToNpcTalkDialogLine($"Я пришел собрать нологи! Плоти {Instance.TaxToPay} золота");
            line.ResponseOptions = new List<DialogOption>()
            {
                new()
                {
                    Text = $"Хорошо, вот твои деньги. [Заплатить {Instance.TaxToPay} золотых]",
                    Action = () =>
                    {
                        Instance.Gold -= Instance.TaxToPay;
                        DOVirtual.Int(Instance.Gold, Instance.Gold - Instance.TaxToPay, 1f,
                                value => Instance.GoldTextMesh.text = value.ToString())
                            .SetEase(Ease.InOutSine)
                            .SetAutoKill(true);
                        DialogWindow.Instance.NpcTalk("Охуенчик!", npcData.NpcName);
                    }
                },
                new()
                {
                    Text = "У меня нет таких денег",
                    Action = () => { GameOver(); }
                }
            };

            return line;
        }

        private static DialogLine GetDialogWithVillager(NpcData npcData)
        {
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
                        DialogWindow.Instance.NpcTalk("Охуенчик!", npcData.NpcName);
                        Debug.Log(QuestJournal.Instance.SideQuests);
                    }
                },
                new()
                {
                    Text = "Увы дружище, ничем не могу помочь",
                    Action = () => { DialogWindow.Instance.NpcTalk("Бля пиздец хуево :(", npcData.NpcName); }
                }
            };

            return quest;
        }

        private static void GameOver()
        {
        }

        private static DialogLine ToNpcTalkDialogLine(string text) => new() { Text = text };
    }
}