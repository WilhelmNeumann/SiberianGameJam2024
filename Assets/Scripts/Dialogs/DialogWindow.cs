using DG.Tweening;
using TMPro;
using UnityEngine;
using Utils;

namespace Dialogs
{
    public class DialogWindow: Singleton<DialogWindow>
    {
        [SerializeField] public TextMeshProUGUI PlayerText;
        [SerializeField] public TextMeshProUGUI NpcText;
        [SerializeField] public TextMeshProUGUI NpcNameText;
        
        [SerializeField] public GameObject PlayerTextArea;
        [SerializeField] public GameObject NpcTextArea;

        [SerializeField] public float Duration = 1f;

        public void NpcTalk(string text, string npcName)
        {
            NpcNameText.text = npcName;
            NpcTextArea.gameObject.SetActive(true);
            TweenText(NpcText, text);
        }

        public void PlayerTalk(string text) => TweenText(PlayerText, text);

        private void TweenText(TextMeshProUGUI textField, string text)
        {
            DOVirtual.Int(0, text.Length, Duration, i => NpcText.text = text[..i]).SetEase(Ease.Linear);
        }
    }
}