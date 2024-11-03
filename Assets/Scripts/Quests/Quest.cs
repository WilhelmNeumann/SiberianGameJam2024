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
        public int LocationId;
        public QuestType QuestType;
    }

    public enum QuestType
    {
        MainQuest,
        SideQuest,
    }
}