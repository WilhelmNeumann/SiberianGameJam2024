using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Quests {
    public class QuestView : MonoBehaviour {
        
        [SerializeField]
        private Text textDescription;
        
        private Quest _quest;
        
        public void SetQuest(Quest quest) {
            _quest = quest;
            textDescription.text = GenerateQuestDescription(quest);
        }
        
        private static string GenerateQuestDescription(Quest quest)
        {
            return $"Objective: {quest.Objective}\n" +
                   $"Experience: +{quest.Xp} Xp\n" +
                   $"Reward: {quest.Gold} gold\n" +
                   $"Difficulty: {quest.Difficulty}/10\n";
        }
    }
}