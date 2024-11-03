using System.Collections.Generic;
using System.Linq;
using Npc;
using UnityEngine;

namespace Quests
{
    public abstract class QuestFactory
    {
        /// Генерация рандомных квестов
        public static Quest GenerateQuest(string questGiverName, NpcType npcType)
        {
            var random = new System.Random();
            // objective and solution has same index
            var randomIndex = random.Next(0, objectives.Count);
            return new Quest
            {
                Xp = Random.Range(1, 50),
                Gold = Random.Range(10, 500),
                Difficulty = Random.Range(1, 5),
                Objective = objectives[randomIndex],
                ApplicationText =
                    $"{introductionPhrases[random.Next(introductionPhrases.Count)]} {problems[randomIndex]}, {solutions[random.Next(solutions.Count)]}",
                CompletionText = completions[randomIndex],
                QuestGiverName = questGiverName,
                QuestType = QuestType.SideQuest
            };
        }

        public static Quest GetNextMainQuest()
        {
            var location = Location.Locations.First(x => x.State == LocationState.Neutral);
            return new Quest
            {
                QuestType = QuestType.MainQuest,
                Location = location,
                Difficulty = location.Difficulty,
                Xp = Random.Range(1, 50),
                Gold = location.RewardToGive,
                Objective = location.QuestObjective,
                CompletionText = location.GoodCompletionText
            };
        }
        
        
        // Абсурдные шаблоны квестов
        private static List<string> introductionPhrases = new List<string>
        {
            "Ты не слышал, что происходит в округе?",
            "Тут народ шепчется, что опять начались проблемы.",
            "Послушай, нужна помощь.",
            "В деревне кое-что произошло, и мне нужна помощь.",
            "Ты уж извини, но я нуждаюсь в поддержке."
        };

        private static List<string> problems = new List<string>
        {
            "У аптекаря крысы в подвале завелись",
            "Кузнеца кто-то дразнит, кидая камни по ночам",
            "На площади выросла огромная репа, которая пугает детей",
            "Старый петух деревенского старосты кукарекает по ночам",
            "Говорящая ворона залетела в таверну и всех дразнит"
        };

        private static List<string> solutions = new List<string>
        {
            "нужно с этим разобраться, пока не стало слишком поздно.",
            "кто-то должен с этим покончить.",
            "надо это прекратить, пока не вышло из-под контроля.",
            "нужно помочь, и все будут благодарны.",
            "это надо прекратить, пока кто-нибудь не пострадал."
        };

        private static List<string> objectives = new List<string>
        {
            "Убить 50 крыс в подвале аптекаря.",
            "Найти и разобраться с таинственным злодеем.",
            "Выкопать и выбросить огромную репу с площади.",
            "Успокоить петуха старосты, чтобы тот больше не кукарекал.",
            "Изгнать говорящую ворону из таверны."
        };

        private static List<string> completions = new List<string>
        {
            "Я убил крыс в подвале аптекаря и пришел за наградой.",
            "Я поймал того, кто кидал камни в кузнеца. Что дальше?",
            "Репа выкопана и больше никому не помешает. Должна быть награда.",
            "Петух старосты больше не тревожит деревню.",
            "Говорящая ворона ушла, и в таверне снова тишина."
        };

        public static string[] GetMainStoryQuests() => new[]
        {
            "Собери 15 ромашек",
            "Спасти принцессу... а может, принца?",
            "Проникновение в логово Босса — ждем лута!",
            "Найти мифический меч... в куче мусора",
            "Отразить атаку куриц из Зельдарии",
            "Защитить квестового НПС, который всех бесит",
            "Доставить хлебушек через орочьи земли",
            "Найти и приручить магическую картошку",
            "Вызволить путника, который не учил заклинаний",
            "Сопровождение торговца… с 1 хп",
            "Сразиться с боссом, пока он не превратился в дракона",
            "Взорвать котел зелья и избежать наказания",
            "Саботировать базу... крафтом липкой бомбы",
            "Организовать побег из тюрьмы гоблинов (опять)",
            "Эпичная битва за топовый меч на +1 урон"
        };
    }
}