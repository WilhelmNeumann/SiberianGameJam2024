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

        public static int GetFailedCount() => Locations.Where(x => x.State == LocationState.Bad).ToList().Count;
        public static int GetNeutralCount() => Locations.Where(x => x.State == LocationState.Neutral).ToList().Count;
        public static int GetGoodCount() => Locations.Where(x => x.State == LocationState.Good).ToList().Count;
        public static int GetStoryCompletePercent() => (Locations.Count - GetNeutralCount()) * 10;
        
        public static readonly List<Location> Locations = new()
        {
            new Location
            {
                ID = 1,
                Name = "Болото",
                Difficulty = 1,
                QuestObjective = "Зачистить болото от культистов",
                GoodCompletionText = "Я зачистил болото от культистов и собрал гору лута.",
                BadCompletionText = "Твой герой мертв, и болото теперь под нашим контролем.",
                RewardToGive = 200,
                RewardToReceive = 500,
            },
            new Location
            {
                ID = 2,
                Name = "Мельница",
                Difficulty = 1,
                QuestObjective = "Освободить мельницу от культистов",
                GoodCompletionText = "Я освободил мельницу от культистов, правда теперь там бардак",
                BadCompletionText = "Мельница стала могилой твоего героя. Теперь мы там заправляем.",
                RewardToGive = 200,
                RewardToReceive = 500
            },
            new Location
            {
                ID = 3,
                Name = "Копи гномов",
                Difficulty = 2,
                QuestObjective = "Защитить гномью гору от атаки культистов",
                GoodCompletionText = "Гномы могут спать спокойно, регион освобожден",
                BadCompletionText = "Гномья гора теперь наша благодаря тебе. Твой герой не продержался и 5 минут.",
                RewardToGive = 300,
                RewardToReceive = 600
            },
            new Location
            {
                ID = 4,
                Name = "Лес",
                Difficulty = 3,
                QuestObjective = "Зачистить лес от культистов",
                GoodCompletionText = "В лесу теперь спокойно, ни одной живой души.",
                BadCompletionText = "Мы устроили кровавый пир с твоим героем в главной роли. И лес теперь наш.",
                RewardToGive = 100,
                RewardToReceive = 300
            },
            new Location
            {
                ID = 5,
                Name = "Башни Магов",
                Difficulty = 3,
                QuestObjective = "Защитить башню мага от нападения культистов",
                GoodCompletionText = "Волшебник в безопасности. Он поможет нам в финальной битве?",
                BadCompletionText = "Мы сбросили твоего героя с башни. Жалко ты этого не видел.",
                RewardToGive = 400,
                RewardToReceive = 700,
            },
            new Location
            {
                ID = 6,
                Name = "Городок",
                Difficulty = 4,
                QuestObjective = "Помочь ярлу в битве за город",
                GoodCompletionText = "Мы отбили нападение на город, где моя награда?",
                BadCompletionText = "Город захвачен. Герой мертв! Король Демонов скоро восстанет!",
                RewardToGive = 150,
                RewardToReceive = 350
            },
            new Location
            {
                ID = 7,
                Name = "Рыбацкая деревня",
                Difficulty = 5,
                QuestObjective = "Выяснить что стало с рыбаками",
                GoodCompletionText = "Рыбаков забрали в рабство, но я их освободил",
                BadCompletionText = "Мы забрали твоего героя в рабство. Теперь он служит нам.",
                RewardToGive = 250,
                RewardToReceive = 550
            },
            new Location
            {
                ID = 8,
                Name = "Пашня",
                Difficulty = 6,
                QuestObjective = "Защитить доставку зерна в город",
                GoodCompletionText = "Было жарко, но я помог отбить караван от культистов",
                BadCompletionText = "Твой герой был убит и воскрешен в качестве нежити.",
                RewardToGive = 100,
                RewardToReceive = 300
            },
            new Location
            {
                ID = 9,
                Name = "Озеро",
                Difficulty = 7,
                QuestObjective = "Отыскать древний меч убийцы демонов",
                GoodCompletionText = "Я отыскал древний клинок! Им я сражу Короля Демонов!",
                BadCompletionText = "Твой герой отыскал древний клинок, и был обезглавлен",
                RewardToGive = 200,
                RewardToReceive = 400
            },
            new Location
            {
                ID = 10,
                Name = "Чёртова Гора",
                Difficulty = 8,
                QuestObjective = "Победить Короля Демонов",
                GoodCompletionText = "Король Демонов уничтожен! Мир спасен! Время пировать!",
                BadCompletionText = "Твой герой послужил пищей для короля демонов! Он восстал и идет сюда!",
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