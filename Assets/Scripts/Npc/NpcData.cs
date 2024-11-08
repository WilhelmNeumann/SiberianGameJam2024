using System;
using System.Collections.Generic;
using Quests;
using Utils;

namespace Npc
{
    public class NpcData
    {
        public string NpcName;
        public NpcType NpcType;
        public int Level;
        public List<string> GreetingsText;
        public List<string> ByeText;
        public Quest Quest;
        public bool IsIntro;
        public int PrefabIndex = - 1;
        public NpcViewData NpcViewData;
        public int Strength = 0;
        public int Intelligence = 0;
        public int Charisma = 0;
        public PotionType ActivePotion = PotionType.None;
        public Action PreVisitAction;
        public Action PostVisitAction;
    }
}