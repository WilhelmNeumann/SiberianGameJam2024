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
            return $"Цель: {quest.Objective}\n" +
                   $"Опыт: +{quest.Xp} Xp\n" +
                   $"Награда: {quest.Gold} золота\n" +
                   $"Сложность: {quest.Difficulty}/10\n";
        }
    }
}