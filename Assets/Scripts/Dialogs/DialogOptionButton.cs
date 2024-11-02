using System;
using TMPro;
using UnityEngine;

namespace Dialogs
{
    public class DialogOptionButton : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI TextMeshProUGUI;
        public string Text;
        public Action Action;

        public void OnClick()
        {
            Action?.Invoke();
            DialogWindow.Instance.ContinueDialog();
        }

        public void Init(string optionText, Action action)
        {
            Action = action;
            Text = optionText;
            TextMeshProUGUI.text = optionText;
        }
    }
}