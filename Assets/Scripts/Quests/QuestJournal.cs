using System.Collections.Generic;
using Npc;
using Utils;

namespace Quests
{
    public class QuestJournal: Singleton<QuestJournal>
    {
        public List<Quest> SideQuests = new();
        
        // Герои, которые ушли на задание
        public List<NpcData> HerosOnDuty = new();

        private void Start()
        {
            
        }
        
        public void AddSideQuest(Quest quest)
        {
            SideQuests.Add(quest);
        }

        public void GetNextMainStoryQuest()
        {
            
        }
    }
}