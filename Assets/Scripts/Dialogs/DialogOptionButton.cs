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
            Action.Invoke();
        }

        public void SetText(string optionText)
        {
            Text = optionText;
            TextMeshProUGUI.text = optionText;
        }
    }
}