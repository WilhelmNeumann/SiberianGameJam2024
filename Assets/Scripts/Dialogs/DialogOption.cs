using System;
using Quests;

namespace Dialogs
{
    public class DialogOption
    {
        public string Text;
        public Action Action;
        public Func<QuestDescription> GetDetailsText;
    }
}