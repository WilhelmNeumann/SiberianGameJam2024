using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dialogs;
using Npc;
using Unity.VisualScripting;
using UnityEngine;

namespace Utils
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] public int Gold = 1000;

        public IEnumerator Start()
        {
            var npcData = NpcFactory.GenerateNpc();
            yield return TavernNpc.Instance.WalkIn();

            var scenario = GetDialogScenario(npcData);

            foreach (var dialogLine in scenario)
            {
                if (dialogLine.Type == DialogType.Npc)
                {
                    DialogWindow.Instance.NpcTalk(dialogLine.Text, npcData.NpcName);
                    yield return new WaitUntil(() => DialogWindow.Instance.CanContinue);
                }
            }
            
            yield return TavernNpc.Instance.WalkOut();
        }


        private static List<DialogLine> GetDialogScenario(NpcData npcData)
        {
            var scenario = npcData.GreetingsText.Select(ToNpcTalkDialogLine).ToList();

            if (npcData.NpcType == NpcType.Villager)
            {
            }

            var goodBye = npcData.ByeText.Select(ToNpcTalkDialogLine).ToList();

            scenario.AddRange(goodBye);
            return scenario;
        }

        private static DialogLine ToNpcTalkDialogLine(string text) => new()
        {
            Type = DialogType.Npc,
            Text = text,
        };

        private struct DialogLine
        {
            public DialogType Type;
            public string Text;
            public Action Action;
        }

        public enum DialogType
        {
            Npc,
            Player
        }
    }
}