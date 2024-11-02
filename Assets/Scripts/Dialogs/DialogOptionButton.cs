using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogs
{
    public class DialogOptionButton : MonoBehaviour
    {
        [SerializeField] public Text UiText;
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
            UiText.text = optionText;
        }
    }
}