using System.Collections.Generic;
using Utils;

namespace Quests
{
    public class QuestJournal: Singleton<QuestJournal>
    {
        public List<Quest> SideQuests = new();
        

        private void Start()
        {
            
        }
        
        public void AddSideQuest(Quest quest)
        {
            SideQuests.Add(quest);
        }
    }
}