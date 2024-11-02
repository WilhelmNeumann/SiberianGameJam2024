using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dialogs
{
    public class DialogOptionButton: MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI TextMeshProUGUI;
        public string Text;
        public Action Action;

        public void OnClick()
        {
            Action.Invoke();
        
        }
    }
}