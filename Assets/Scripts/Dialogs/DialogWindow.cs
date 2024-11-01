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
        
        [SerializeField] public GameObject PlayerTextArea;
        [SerializeField] public GameObject NpcTextArea;

        [SerializeField] public float Duration = 1f;
        
        public void NpcTalk(string text, string personName) => TweenText(NpcText, text);

        public void PlayerTalk(string text) => TweenText(PlayerText, text);

        private void Start()
        {
            NpcTalk(
                "Lorem ipsum dolor sit amet consectetur adipisicing elit. Maxime mollitia, quas vel sint commodi repudiandae consequuntur voluptatum",
                "Npc1");
        }

        private void TweenText(TextMeshProUGUI textField, string text)
        {
            DOVirtual.Int(0, text.Length, Duration, i => NpcText.text = text[..i]).SetEase(Ease.Linear);
        }
    }
}