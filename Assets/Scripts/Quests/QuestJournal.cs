using System.Collections.Generic;
using Utils;

namespace Quests
{
    public class QuestJournal: Singleton<QuestJournal>
    {
        public List<Quest> QuestPool = new List<Quest>();

        public void AddQuest(Quest quest)
        {
            QuestPool.Add(quest);
        }
    }
}