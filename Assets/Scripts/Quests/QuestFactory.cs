using System.Collections.Generic;
using Npc;
using UnityEngine;

namespace Quests
{
    public abstract class QuestFactory
    {
        /// Генерация рандомных квестов
        public static Quest GenerateQuest(string questGiverName, NpcType npcType)
        {
            var template = QuestTemplates[Random.Range(0, QuestTemplates.Count)];
            var (applicationText, description) = GenerateFromTemplates();
            return new Quest
            {
                Xp = Random.Range(1, 50),
                Gold = Random.Range(10, 500),
                Difficulty = Random.Range(1, 10),
                ShortDescription = description,
                ApplicationText = applicationText,
                QuestGiverName = questGiverName,
            };
        }

        // Абсурдные шаблоны квестов
        private static readonly List<string> QuestTemplates = new()
        {
            "Дела просто прекрасно, только вот {npc} потерял(а) {object} в {location}. Можешь помочь найти?",
            "Все нормально, только {npc} жалуется, что в {location} теперь завелись {creatures}. Кто бы их выгнал!",
            "Слушай, все отлично, но как только {npc} решил(а) организовать {event}, туда заявились {creatures}. Поможешь разобраться?",
            "{npc} утверждает, что его(ее) {object} похитили {creatures}. Они теперь спрятались в {location}. Надо их проучить!",
            "Дела не идут, потому что {npc} никак не может завершить свой магический эксперимент — {object} неожиданно ожили в {location}. Выручай!",
            "Да всё норм, если бы не {creatures}, которые теперь объявили {location} своей территорией. Поговори с ними, пожалуйста.",
            "{npc} рассказал(а), что в {location} видели говорящий {object}. Это ненормально, правда?",
            "Дела лучше некуда, вот только в {location} кто-то из местных запустил слух, что {creatures} теперь — официальные стражи города. {npc} слегка беспокоится об этом."
        };

        // Шаблоны для краткого описания квеста
        private static readonly List<string> DescriptionTemplates = new()
        {
            "Помоги {npc} с его странной проблемой в {location}.",
            "Похоже, в {location} что-то пошло не так. {npc} ждет твоей помощи.",
            "Присмотри за ситуацией в {location}, прежде чем она выйдет из-под контроля.",
            "Похоже, {npc} нуждается в твоей помощи из-за беспорядка в {location}.",
            "{npc} беспокоится о событиях в {location}. Проверь, что там происходит.",
            "На этот раз в {location} явно происходит что-то неладное. Помоги {npc}!",
            "Разберись с тем, что происходит в {location}, пока не стало еще хуже."
        };

        // Списки объектов для подстановки
        private static readonly List<string> Objects = new() { "волшебный носок", "леденец судьбы", "белка", "огромный свиток", "горшок с цветами", "копченая рыбка" };

        private static readonly List<string> Locations = new() { "старом колодце", "разрушенной мельнице", "волшебном лесу", "старой таверне", "некрополе старого мага" };

        private static readonly List<string> Creatures = new()
        {
            "боевые крысы", "гигантские муравьи", "сумасшедшие козы", "говорящие утки", "агрессивные гномы",
            "древесные духи"
        };

        private static readonly List<string> Npcs = new()
        {
            "странный старик", "местный алхимик", "продавец жареной рыбы", "бродячий поэт", "жрица непонятного культа",
            "рыцарь в ржавых доспехах"
        };

        private static readonly List<string> Events = new()
        {
            "турнир по поеданию пирогов", "фестиваль светлячков", "нелегальные гонки на тележках",
            "конкурс рифмоплетов", "церемония почитания капусты"
        };

        private static (string quest, string description) GenerateFromTemplates()
        {
            var template = QuestTemplates[Random.Range(0, QuestTemplates.Count)];
            var quest = template
                .Replace("{object}", Objects[Random.Range(0, Objects.Count)])
                .Replace("{location}", Locations[Random.Range(0, Locations.Count)])
                .Replace("{creatures}", Creatures[Random.Range(0, Creatures.Count)])
                .Replace("{npc}", Npcs[Random.Range(0, Npcs.Count)])
                .Replace("{event}", Events[Random.Range(0, Events.Count)]);

            // Выбираем случайный шаблон для описания
            var descriptionTemplate = DescriptionTemplates[Random.Range(0, DescriptionTemplates.Count)];
            var description = descriptionTemplate
                .Replace("{npc}", Npcs[Random.Range(0, Npcs.Count)])
                .Replace("{location}", Locations[Random.Range(0, Locations.Count)]);

            return (quest, description);
        }
    }
}