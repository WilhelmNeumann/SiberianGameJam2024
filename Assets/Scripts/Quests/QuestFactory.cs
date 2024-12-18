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
            var problemDescription = problems.ElementAt(randomIndex).Key;
            var mainSkill = problems[problemDescription];
            var q = new Quest
            {
                Xp = Random.Range(1, 50),
                Gold = Random.Range(10, 500),
                Difficulty = Random.Range(1, 5),
                Objective = objectives[randomIndex],
                ApplicationText =
                    $"{introductionPhrases[random.Next(introductionPhrases.Count)]} {problemDescription}, {solutions[random.Next(solutions.Count)]}",
                CompletionText = completions[randomIndex],
                QuestGiverName = questGiverName,
                QuestType = QuestType.SideQuest,
                QuestState = QuestState.None,
                MainSkill = mainSkill
            };

            DistributeSkillPoints(q);
            return q;
        }

        public static Quest GetNextMainQuest()
        {
            var location = Location.Locations.First(x => x.State == LocationState.Neutral);

            var quest = new Quest
            {
                QuestType = QuestType.MainQuest,
                Location = location,
                Difficulty = location.Difficulty,
                Xp = Random.Range(1, 50),
                Gold = location.RewardToGive,
                Objective = location.QuestObjective,
                CompletionText = location.GoodCompletionText,
            };

            DistributeSkillPoints(quest);
            return quest;
        }

        private static void DistributeSkillPoints(Quest quest)
        {
            // Calculate the total skill points based on the quest difficulty.
            var totalSkillPoints = quest.Difficulty * 10; // Example: 10 points per difficulty level

            // Assign primary characteristic with a weight of 0.5 and others with 0.25 each.
            const float primaryWeight = 0.5f;
            const float secondaryWeight = 0.25f;

            // Distribute points based on MainSkill property.
            int primaryPoints = Mathf.RoundToInt(totalSkillPoints * primaryWeight);
            int secondaryPoints = Mathf.RoundToInt(totalSkillPoints * secondaryWeight);

            switch (quest.MainSkill)
            {
                case MainSkill.Strength:
                    quest.RequiredStrength = primaryPoints;
                    quest.RequiredIntelligence = secondaryPoints;
                    quest.RequiredCharisma = secondaryPoints;
                    break;
                case MainSkill.Intelligence:
                    quest.RequiredIntelligence = primaryPoints;
                    quest.RequiredStrength = secondaryPoints;
                    quest.RequiredCharisma = secondaryPoints;
                    break;
                case MainSkill.Charisma:
                    quest.RequiredCharisma = primaryPoints;
                    quest.RequiredStrength = secondaryPoints;
                    quest.RequiredIntelligence = secondaryPoints;
                    break;
            }
        }

        // Абсурдные шаблоны квестов
        private static List<string> introductionPhrases = new()
        {
            "Ты не слышал, что происходит в округе?",
            "Тут народ шепчется, что опять начались проблемы.",
            "Послушай, нужна помощь.",
            "В деревне кое-что произошло, и мне нужна помощь.",
            "Ты уж извини, но я нуждаюсь в поддержке.",
            "Слыхал, какие беды у нас на носу?",
            "Эх, в деревне снова неспокойно стало.",
            "Ты не поверишь, что у нас творится!",
            "У нас тут случилась беда.",
            "Кажется, без твоей помощи нам не обойтись.",
            "Нужен кто-то с крепкими нервами и добрым сердцем — у нас беда.",
            "Видишь, как все грустно? Вот и пришел сюда жаловаться.",
            "Помощь нужна деревне, иначе совсем пропадем.",
            "Кажется, беды опять к нам пожаловали.",
            "Эх, если бы кто-то помог нам со всем этим...",
            "Беда на беде у нас, и конца-края им не видно.",
            "Ты ведь герой, не так ли? Тогда у нас тут для тебя задание."
        };

        private static Dictionary<string, MainSkill> problems = new()
        {
            { "У аптекаря крысы в подвале завелись", MainSkill.Strength },
            { "На сына аптекаря напали медведи, и теперь он жаждет мести", MainSkill.Strength },
            { "На площади выросла огромная репа, которая пугает детей", MainSkill.Strength },
            { "Старый петух деревенского старосты кукарекает по ночам", MainSkill.Charisma },
            { "Говорящая ворона залетела в таверну и всех дразнит", MainSkill.Charisma },
            { "Жена мельника ушла в лес и не вернулась, ее не могут найти уже неделю", MainSkill.Intelligence },
            { "На лугу ночью слышен странный смех, и никто не знает, чей он", MainSkill.Intelligence },
            { "У пастуха овцы стали агрессивными и кидаются на людей", MainSkill.Strength },
            { "В лесу появилась хижина, которой там раньше не было", MainSkill.Intelligence },
            { "В пруду вдруг стало слишком много лягушек — они всех распугивают", MainSkill.Charisma },
            { "У молочницы кувшин с молоком вдруг побежал сам по себе", MainSkill.Intelligence },
            { "Колодец стал странно булькать ночью, и люди боятся подходить к нему", MainSkill.Intelligence },
            {
                "Культисты украли у пахаря его любимую мотыгу, которая передавалась из поколения в поколение",
                MainSkill.Strength
            },
        };

        private static List<string> solutions = new()
        {
            "нужно с этим разобраться, пока не стало слишком поздно.",
            "кто-то должен с этим покончить.",
            "надо это прекратить, пока не вышло из-под контроля.",
            "нужно помочь, и все будут благодарны.",
            "это надо прекратить, пока кто-нибудь не пострадал.",
            "С этим пора разобраться, пока оно не обросло еще большими проблемами.",
            "Нужно положить этому конец, прежде чем все сорвутся с катушек.",
            "нужно чтоб кто-то этим занялся, иначе покоя не будет.",
            "Нельзя оставлять это просто так, кто-то должен вмешаться.",
            "Пока не стало хуже, нужно решить этот вопрос.",
            "Надо этим заняться, пока ещё есть шанс на спокойную жизнь.",
            "Это лучше остановить, пока не стало привычкой.",
            "Если не разобраться сейчас, потом будет ещё сложнее.",
            "Скорее бы положить этому конец, и всем станет легче.",
            "Кажется, нужно вмешаться, пока бед не натворили.",
            "Это надо решить, пока не разнесли слух по всему селу."
        };

        private static List<string> objectives = new()
        {
            "Убить 50 крыс в подвале аптекаря.",
            "Принести 100 медвежьих шкуры.",
            "Выкопать и выбросить огромную репу с площади.",
            "Успокоить петуха старосты, чтобы тот больше не кукарекал.",
            "Изгнать говорящую ворону из таверны.",
            "Выяснить что случилось с женой мельника",
            "Выяснить источник странного смеха на лугу",
            "Разобраться с агрессивными овцами",
            "Выяснить откуда взялась хижина в лесу",
            "Разобраться со стаей лягушек",
            "Выяснить почему убежало молоко",
            "Разобраться с булькающим колодцем",
            "Вернуть семейную мотыгу пахарю"
        };

        private static List<string> completions = new()
        {
            "Я убил крыс в подвале аптекаря, живучие оказались заразы. Я пришел за наградой.",
            "Я убил всех чертовых медведей в округе, надеюсь, оно того стоило. Давай награду!",
            "Репа выкопана и больше никому не помешает. Должна быть награда.",
            "Петух старосты больше не тревожит деревню. Он больше никого не потревожит.",
            "Говорящая ворона ушла, и в таверне снова тишина.",
            "Жену мельника волки загрызли, но я с ними разобрался. ",
            "На лугу завелся призрак старого пьяницы, который смеляся с шутки, которая его убила. Я с ним разобрался.",
            "Овец заколдовали культисты, но я их всех перебил. Овец, а не культистов.",
            "Я выяснил откуда взялась хижина. Это все чародей и его эксперименты с телепортацией.\nЯ телепортировал его куда подальше.",
            "Я поговорил с лягушачьим королем — обещал увести своих подданных подальше от деревни",
            "Оказалось, это проделки домового — договорился с ним, теперь молоко останется на месте.",
            "Я решил проблему радикально и разрушил колодец. Он больше никогда не посмеет булькать.",
            "Я пробрался в лагерь культистов, и перебил их всех ради этой мотыги. Мне полагается награда."
        };
    }
}