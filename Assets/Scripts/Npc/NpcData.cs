using System.Collections.Generic;
using Quests;

namespace Npc
{
    public class NpcData
    {
        public string NpcName;
        public NpcType NpcType;
        public List<string> GreetingsText;
        public List<string> ByeText;
        public Quest Quest;
    }
}