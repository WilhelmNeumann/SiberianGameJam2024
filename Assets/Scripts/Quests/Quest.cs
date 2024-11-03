namespace Quests
{
    public class Quest
    {
        public string ApplicationText; // У меня проблема, разбойники украли мое говно и ушли в адскую пещеру
        public string Objective; // Принести говно из адской пещеры
        public string CompletionText; // Я принес говно из адской пещеры, как и договаривались
        public int Xp;
        public int Gold;
        public int Difficulty;
        public string QuestGiverName; // Продавец говна
        public Location Location;
        public QuestType QuestType;
        public QuestState QuestState;
    }

    public enum QuestState
    {
        InProgress,
        Success,
        Failed
    }
    
    public enum QuestType
    {
        MainQuest,
        SideQuest,
    }
}