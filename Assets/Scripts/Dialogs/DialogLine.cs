using System;
using System.Collections.Generic;
using Utils;

namespace Dialogs
{
    public class DialogLine
    {
        public GameManager.DialogType Type;
        public string Text;
        public List<DialogOption> ResponseOptions;
    }
}