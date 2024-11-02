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
        }


        private static List<DialogLine> GetDialogScenario(NpcData npcData)
        {
            var x = new List<DialogLine>();
            npcData.GreetingsText.ToList().ForEach(t =>
            {
                var line = new DialogLine
                {
                    Type = DialogType.Npc,
                    Text = t,
                };
                
                x.Add(line);
            });
            
            return x;
        }

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
