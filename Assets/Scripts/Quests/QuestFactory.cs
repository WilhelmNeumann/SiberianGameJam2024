namespace Quests
{
    public class QuestFactory
    {
        /// Генерация рандомных квестов
        public static Quest GenerateQuest()
        {
            return new Quest
            {
                Description = "Принеси 50 камней из адской пещеры",
                Xp = 15,
                Gold = 10,
                Difficulty = 10
            };
        }
    }
}