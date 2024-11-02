using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Dialogs
{
    public class DialogWindow : Singleton<DialogWindow>
    {
        [SerializeField] public TextMeshProUGUI PlayerText;
        [SerializeField] public TextMeshProUGUI NpcText;
        [SerializeField] public TextMeshProUGUI NpcNameText;

        [SerializeField] public GameObject PlayerTextArea;
        [SerializeField] public GameObject NpcTextArea;

        [SerializeField] public float Duration = 1f;
        public bool CanContinue { get; private set; } = false;

        private Tweener _dialogTweener;
        
        public void NpcTalk(string text, string npcName)
        {
            CanContinue = false;
            NpcNameText.text = npcName;
            NpcTextArea.gameObject.SetActive(true);
            TweenText(text);
        }

        public void PlayerQuestAcceptanceDialogOptions()
        {
        }

        private void TweenText(string text)
        {
            _dialogTweener = DOVirtual.Int(0, text.Length, Duration, i => NpcText.text = text[..i])
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    NpcText.text = text;
                    _dialogTweener = null;
                })
                .SetAutoKill(true);
        }

        public void ContinueDialog()
        {
            if (_dialogTweener != null)
            {
                _dialogTweener?.Kill();
            }
            else
            {
                CanContinue = true;
            }
        }
    }
}