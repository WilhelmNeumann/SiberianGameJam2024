using TMPro;
using UnityEngine;

namespace Quests {
    public class QuestView : MonoBehaviour {
        
        [SerializeField]
        private TextMeshProUGUI textDescription;
        
        private Quest _quest;
        
        public void SetQuest(Quest quest) {
            _quest = quest;
            textDescription.text = _quest.Objective;
        }
        

    }
}