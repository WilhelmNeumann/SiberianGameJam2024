using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Dialogs;
using Npc;
using Quests;
using TMPro;
using Unity.VisualScripting;
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
            yield return TavernNpc.Instance.WalkIn();

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

            yield return TavernNpc.Instance.WalkOut();
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
                NpcType.Hero => throw new ArgumentOutOfRangeException(),
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

        private static DialogLine GetDialogWithTaxCollector(NpcData npcData)
        {
            var line = ToNpcTalkDialogLine($"Я пришел собрать нологи! Плоти {Instance.TaxToPay} золота");
            line.ResponseOptions = new List<DialogOption>()
            {
                new()
                {
                    Text = $"Хорошо, вот твои деньги. [Заплатить {Instance.TaxToPay} золотых",
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