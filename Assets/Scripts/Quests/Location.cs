using System.Collections.Generic;
using System.Linq;

namespace Quests
{
    public class Location
    {
        public int ID;
        public string Name;
        public LocationState State = LocationState.Neutral;
        public string QuestObjective;
        public string GoodCompletionText;
        public string BadCompletionText;
        public int RewardToGive = 200;
        public int RewardToReceive = 500;
        public int Difficulty;
        
        public void SetState(LocationState state) => State = state;

        public static Location GetById(int id) => Locations.First(x => x.ID == id);

        public static readonly List<Location> Locations = new()
        {
            new Location
            {
                ID = 1,
                Name = "Болото", 
                Difficulty = 3,
                QuestObjective = "Ребенок потерялся в лесу с волками, надо его спасти",
                GoodCompletionText = "Я спас ребенка от волков, где моя награда?",
                BadCompletionText = "Волки сожрали твоего героя, он мертв и его душа принадлежит нам. Вот твоя награда.",
                RewardToGive = 200,
                RewardToReceive = 500
            },
            new Location
            {
                ID = 2, 
                Name = "Мельница",
                Difficulty = 2,
                QuestObjective = "Освободить регион от культистов",
                GoodCompletionText = "Я освободил регион от культистов, где моя награда?",
                BadCompletionText = "Спасибо что послал к нам героя, он мертв и его душа принадлежит нам. Вот твоя награда.",
                RewardToGive = 200,
                RewardToReceive = 500
            }
        };
    }

    public enum LocationState
    {
        Neutral,
        Good,
        Bad
    }
}