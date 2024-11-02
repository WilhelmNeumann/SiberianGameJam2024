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
                if (dialogLine.Type == DialogType.Npc)
                {
                    DialogWindow.Instance.NpcTalk(dialogLine.Text, npcData.NpcName);
                    yield return new WaitUntil(() => DialogWindow.Instance.CanContinue);
                }

                if (dialogLine.Type == DialogType.Player)
                {
                    
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
            }
            

            var goodBye = npcData.ByeText.Select(ToNpcTalkDialogLine).ToList();

            scenario.AddRange(goodBye);
            return scenario;
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