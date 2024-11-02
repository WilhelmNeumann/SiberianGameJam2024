using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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
        [SerializeField] private DialogOptionButton _serializedPrefab;
        [SerializeField] private Transform _dialogOptionsLayoutGroup;
        [SerializeField] public float TextAppearDuration = 1f;
        public bool CanContinue { get; private set; } = false;

        private Tweener _dialogTweener;

        public void NpcTalk(string text, string npcName)
        {
            CanContinue = false;
            NpcNameText.text = npcName;
            NpcTextArea.gameObject.SetActive(true);
            TweenText(text);
        }

        private void TweenText(string text)
        {
            _dialogTweener = DOVirtual.Int(0, text.Length, TextAppearDuration, i => NpcText.text = text[..i])
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
            PlayerTextArea.gameObject.SetActive(false);
            NpcTextArea.gameObject.SetActive(true);
            if (_dialogTweener != null)
            {
                _dialogTweener?.Complete();
            }
            else
            {
                CanContinue = true;
            }
        }

        public void Hide()
        {
            NpcTextArea.gameObject.SetActive(false);
            PlayerTextArea.gameObject.SetActive(false);
        }

        public void ShowPlayerDialogOptions(List<DialogOption> options)
        {
            NpcTextArea.gameObject.SetActive(false);
            PlayerTextArea.gameObject.SetActive(true);

            foreach (Transform button in _dialogOptionsLayoutGroup)
            {
                Destroy(button.gameObject);
            }

            foreach (var option in options)
            {
                var button = Instantiate(_serializedPrefab, _dialogOptionsLayoutGroup, true);
                button.SetText(option.Text);
            }
        }
    }
}