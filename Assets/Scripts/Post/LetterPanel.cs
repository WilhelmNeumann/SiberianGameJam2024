using DefaultNamespace;
using TMPro;
using UnityEngine;

namespace System {
    public class LetterPanel : CloseablePanel {
        [SerializeField]
        private TMP_Text letterText;

        private void Awake() {
            var quest = PostManager.Instance.GetQuest();
            SetLetter(quest);
        }

        public void SetLetter(string letter) {
            letterText.text = letter;
        }
        
    }
}