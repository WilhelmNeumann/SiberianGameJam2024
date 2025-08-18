namespace Quests
{
    public class Quest
    {
        public string ApplicationText; // I have a problem, bandits stole my stuff and went to the hellish cave
        public string Objective; // Bring stuff from the hellish cave
        public string CompletionText; // I brought stuff from the hellish cave, as agreed
        public int Xp;
        public int Gold;
        public int Difficulty;
        public string QuestGiverName; // Stuff seller
        public Location Location;
        public QuestType QuestType;
        public QuestState QuestState = QuestState.None;
        public int RequiredStrength;
        public int RequiredIntelligence;
        public int RequiredCharisma;
        public MainSkill MainSkill {get; set; }
    }

    public enum QuestState
    {
        None,
        Success,
        Failed
    }
    
    public enum QuestType
    {
        MainQuest,
        SideQuest,
    }
    
    public enum MainSkill
    {
        Strength,
        Intelligence,
        Charisma
    }
}