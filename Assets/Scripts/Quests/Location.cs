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
                BadCompletionText =
                    "Волки сожрали твоего героя, он мертв и его душа принадлежит нам. Вот твоя награда.",
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
                BadCompletionText =
                    "Спасибо что послал к нам героя, он мертв и его душа принадлежит нам. Вот твоя награда.",
                RewardToGive = 200,
                RewardToReceive = 500
            },
            new Location
            {
                ID = 3,
                Name = "Копи гномов",
                Difficulty = 4,
                QuestObjective = "Найти потерянное сокровище гномов",
                GoodCompletionText = "Я нашел сокровище гномов, где моя награда?",
                BadCompletionText = "Гномы поймали твоего героя и забрали его душу. Вот твоя награда.",
                RewardToGive = 300,
                RewardToReceive = 600
            },
            new Location
            {
                ID = 4,
                Name = "Лес",
                Difficulty = 1,
                QuestObjective = "Собрать лечебные травы",
                GoodCompletionText = "Я собрал лечебные травы, где моя награда?",
                BadCompletionText = "Твой герой заблудился в лесу и погиб. Вот твоя награда.",
                RewardToGive = 100,
                RewardToReceive = 300
            },
            new Location
            {
                ID = 5,
                Name = "Башни Магов",
                Difficulty = 5,
                QuestObjective = "Победить злого мага",
                GoodCompletionText = "Я победил злого мага, где моя награда?",
                BadCompletionText = "Маг уничтожил твоего героя. Вот твоя награда.",
                RewardToGive = 400,
                RewardToReceive = 700
            },
            new Location
            {
                ID = 6,
                Name = "Городок",
                Difficulty = 2,
                QuestObjective = "Помочь жителям с ремонтом",
                GoodCompletionText = "Я помог жителям, где моя награда?",
                BadCompletionText = "Твой герой погиб, помогая жителям. Вот твоя награда.",
                RewardToGive = 150,
                RewardToReceive = 350
            },
            new Location
            {
                ID = 7,
                Name = "Рыбацкая деревня",
                Difficulty = 3,
                QuestObjective = "Избавиться от морского чудовища",
                GoodCompletionText = "Я избавился от морского чудовища, где моя награда?",
                BadCompletionText = "Морское чудовище убило твоего героя. Вот твоя награда.",
                RewardToGive = 250,
                RewardToReceive = 550
            },
            new Location
            {
                ID = 8,
                Name = "Пашня",
                Difficulty = 1,
                QuestObjective = "Помочь с уборкой урожая",
                GoodCompletionText = "Я помог с уборкой урожая, где моя награда?",
                BadCompletionText = "Твой герой погиб, помогая с уборкой урожая. Вот твоя награда.",
                RewardToGive = 100,
                RewardToReceive = 300
            },
            new Location
            {
                ID = 9,
                Name = "Озеро",
                Difficulty = 2,
                QuestObjective = "Поймать редкую рыбу",
                GoodCompletionText = "Я поймал редкую рыбу, где моя награда?",
                BadCompletionText = "Твой герой утонул в озере. Вот твоя награда.",
                RewardToGive = 200,
                RewardToReceive = 400
            },
            new Location
            {
                ID = 10,
                Name = "Чёртова Гора",
                Difficulty = 5,
                QuestObjective = "Победить демона",
                GoodCompletionText = "Я победил демона, где моя награда?",
                BadCompletionText = "Демон уничтожил твоего героя. Вот твоя награда.",
                RewardToGive = 500,
                RewardToReceive = 800
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