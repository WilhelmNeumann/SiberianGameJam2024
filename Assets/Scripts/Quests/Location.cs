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

        public static Location GetById(int id)
        {
            var location = Locations.FirstOrDefault(x => x.ID == id);
            if (location == null)
            {
                throw new System.ArgumentException($"Location with ID {id} not found");
            }
            return location;
        }

        public static int GetFailedCount() => Locations.Where(x => x.State == LocationState.Bad).ToList().Count;
        public static int GetNeutralCount() => Locations.Where(x => x.State == LocationState.Neutral).ToList().Count;
        public static int GetGoodCount() => Locations.Where(x => x.State == LocationState.Good).ToList().Count;
        public static int GetStoryCompletePercent() => (Locations.Count - GetNeutralCount()) * 10;
        
        public static readonly List<Location> Locations = new()
        {
            new Location
            {
                ID = 1,
                Name = "The Swamp",
                Difficulty = 1,
                QuestObjective = "Clear the swamp of cultists",
                GoodCompletionText = "I cleared the swamp of cultists and collected a mountain of loot.",
                BadCompletionText = "Your hero is dead, and the swamp is now under our control.",
                RewardToGive = 200,
                RewardToReceive = 150,
            },
            new Location
            {
                ID = 2,
                Name = "The Mill",
                Difficulty = 1,
                QuestObjective = "Free the mill from cultists",
                GoodCompletionText = "I freed the mill from cultists, though now it's a mess",
                BadCompletionText = "The mill became your hero's grave. Now we're in charge there.",
                RewardToGive = 200,
                RewardToReceive = 200
            },
            new Location
            {
                ID = 3,
                Name = "Dwarven Mines",
                Difficulty = 2,
                QuestObjective = "Defend the dwarven mountain from cultist attack",
                GoodCompletionText = "The dwarves can sleep peacefully, the region is liberated",
                BadCompletionText = "The dwarven mountain is now ours thanks to you. Your hero didn't last 5 minutes.",
                RewardToGive = 300,
                RewardToReceive = 250
            },
            new Location
            {
                ID = 4,
                Name = "The Forest",
                Difficulty = 3,
                QuestObjective = "Clear the forest of cultists",
                GoodCompletionText = "The forest is now peaceful, not a living soul in sight.",
                BadCompletionText = "We threw a bloody feast with your hero as the main course. And the forest is now ours.",
                RewardToGive = 100,
                RewardToReceive = 300
            },
            new Location
            {
                ID = 5,
                Name = "Mage Towers",
                Difficulty = 3,
                QuestObjective = "Defend the mage's tower from cultist attack",
                GoodCompletionText = "The wizard is safe. Will he help us in the final battle?",
                BadCompletionText = "We threw your hero off the tower. Too bad you didn't see it.",
                RewardToGive = 400,
                RewardToReceive = 350,
            },
            new Location
            {
                ID = 6,
                Name = "The Town",
                Difficulty = 4,
                QuestObjective = "Help the jarl in the battle for the town",
                GoodCompletionText = "We repelled the attack on the town, where's my reward?",
                BadCompletionText = "The town is captured. The hero is dead! The Demon King will soon rise!",
                RewardToGive = 150,
                RewardToReceive = 400
            },
            new Location
            {
                ID = 7,
                Name = "Fishing Village",
                Difficulty = 5,
                QuestObjective = "Find out what happened to the fishermen",
                GoodCompletionText = "The fishermen were taken into slavery, but I freed them",
                BadCompletionText = "We took your hero into slavery. Now he serves us.",
                RewardToGive = 250,
                RewardToReceive = 450
            },
            new Location
            {
                ID = 8,
                Name = "The Farmland",
                Difficulty = 6,
                QuestObjective = "Protect the grain delivery to town",
                GoodCompletionText = "It was hot, but I helped fend off the caravan from cultists",
                BadCompletionText = "Your hero was killed and resurrected as undead.",
                RewardToGive = 100,
                RewardToReceive = 500
            },
            new Location
            {
                ID = 9,
                Name = "The Lake",
                Difficulty = 7,
                QuestObjective = "Find the ancient demon-slaying sword",
                GoodCompletionText = "I found the ancient blade! With it I shall slay the Demon King!",
                BadCompletionText = "Your hero found the ancient blade, and was beheaded",
                RewardToGive = 200,
                RewardToReceive = 550
            },
            new Location
            {
                ID = 10,
                Name = "Devil's Mountain",
                Difficulty = 8,
                QuestObjective = "Defeat the Demon King",
                GoodCompletionText = "The Demon King is destroyed! The world is saved! Time to feast!",
                BadCompletionText = "Your hero served as food for the Demon King! He has risen and is coming here!",
                RewardToGive = 500,
                RewardToReceive = 800
            }
        };
    }

    public enum LocationState
    {
        InProgress,
        Neutral,
        Good,
        Bad
    }
}