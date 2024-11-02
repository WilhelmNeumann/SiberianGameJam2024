using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Dialogs
{
    public class DialogOptionsManager: Singleton<DialogOptionsManager>
    {
        [SerializeField] private DialogOptionButton _serializedPrefab;

        public void SetDialogOptions(DialogLine dialogLine)
        {
            // foreach (var dialogLine in dialogLine.options)
            // {
            // var button = Instantiate(_serializedPrefab, transform, false);
            // button.SetText(dialogLines);
            //     
            // }
            
        }
    }
}