using System.Collections.Generic;
using Npc;
using Utils;

namespace Quests
{
    public class QuestJournal: Singleton<QuestJournal>
    {
        public List<Quest> SideQuests = new();
        
        public void AddSideQuest(Quest quest) => SideQuests.Add(quest);
    }
}