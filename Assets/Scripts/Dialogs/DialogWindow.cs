using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Dialogs
{
    public class DialogWindow : Singleton<DialogWindow>
    {
        [SerializeField] public Text NpcText;
        [SerializeField] public Text NpcNameText;

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
            PlayerTextArea.gameObject.SetActive(false);
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
            CanContinue = false;
            NpcTextArea.gameObject.SetActive(false);
            PlayerTextArea.gameObject.SetActive(true);

            foreach (Transform button in _dialogOptionsLayoutGroup)
            {
                Destroy(button.gameObject);
            }

            foreach (var option in options)
            {
                var button = Instantiate(_serializedPrefab, _dialogOptionsLayoutGroup, true);
                button.transform.localScale = Vector3.one;
                button.Init(option.Text, option.Action, option.GetDetailsText);
            }
        }
    }
}