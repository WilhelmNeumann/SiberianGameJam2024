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
                GreetingsText = "Приветствую трактирщик! Какой хороший день",
                ByeText = "Ну да ладно, пойду а то дел по горло."
            };
        }
    }
}