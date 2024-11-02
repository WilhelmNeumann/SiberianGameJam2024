using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dialogs;
using Npc;
using Quests;
using Unity.VisualScripting;
using UnityEngine;

namespace Utils
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] public int Gold = 1000;

        public IEnumerator Start()
        {
            while (true)
            {
                yield return GameplayLoop();
            }
        }

        private IEnumerator GameplayLoop()
        {
            var npcData = NpcFactory.GenerateNpc();
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
            scenario.AddRange(greetings);

            if (npcData.NpcType == NpcType.Villager)
            {
                scenario.Add(GetDialogWithVillager(npcData));
            }

            var goodBye = npcData.ByeText.Select(ToNpcTalkDialogLine).ToList();

            scenario.AddRange(goodBye);
            return scenario;
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
                        DialogWindow.Instance.ContinueDialog();
                    }
                },
                new()
                {
                    Text = "Увы дружище, ничем не могу помочь",
                    Action = () => DialogWindow.Instance.ContinueDialog()
                }
            };

            return quest;
        }

        private List<DialogLine> GetVillagerInteraction()
        {
            return null;
        }

        private static DialogLine ToNpcTalkDialogLine(string text) => new()
        {
            Type = DialogType.Npc,
            Text = text,
        };

        public enum DialogType
        {
            Npc,
            Player
        }
    }
}