namespace Quests
{
    public class QuestDescription
    {
        public QuestDescription(string details, MainSkill getMainSkill)
        {
            Description = details;
            MainSkill = getMainSkill;
        }

        public string Description {get; set;}
        public MainSkill MainSkill {get; private set;}
    }
}