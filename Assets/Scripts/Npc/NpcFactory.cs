using Quests;

namespace Npc
{
    public abstract class NpcFactory
    {
        public static NpcData GenerateNpc()
        {
            var npcName = "Ульф железнозадый";

            return new NpcData
            {
                NpcType = NpcType.Villager,
                NpcName = npcName,
                Quest = QuestFactory.GenerateQuest(npcName),
                GreetingsText = new[] { "Приветствую трактирщик! Что наливаешь? У меня тут проблема с крысами в подвале" },
                ByeText = new[] { "Ну да ладно, пойду а то дел по горло." }
            };
        }
    }
}