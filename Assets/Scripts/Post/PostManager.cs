using System.Collections;
using System.Collections.Generic;
using Npc;
using Quests;
using TMPro;
using UnityEngine;
using Utils;

namespace System {
    public class PostManager : Singleton<PostManager> {
        
        [SerializeField]
        private TMP_Text lettersCountText;

        [SerializeField]
        private Vector2 position;
        
        private Queue<String> _quests = new Queue<String>();
        
        private void Start() {
            UpdateCounter();
        }
        
        public void AddQuest(Quest quest, NpcData npc) {
            _quests.Enqueue(GetLetterText(quest, npc));
            UpdateCounter();
        }
        
        public String GetQuest() {
            var nextText =  _quests.Dequeue();
            UpdateCounter();
            return nextText;
        }
        
        private void UpdateCounter() {
            var questCount = _quests.Count;
            if (questCount > 0) {
                transform.position = position;
            }
            else {
                transform.position = new Vector2(-1000, -1000);
            }
            lettersCountText.text = questCount.ToString();
        }

        private string GetLetterText(Quest quest, NpcData npc) {
            //NPCName умер выполняя квест quest.objective
            return $"{npc.NpcName} умер выполняя квест {quest.Objective}";
        }

    }
}