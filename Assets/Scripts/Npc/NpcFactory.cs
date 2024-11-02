using System;
using System.Collections.Generic;
using System.Linq;
using Quests;
using Random = UnityEngine.Random;

namespace Npc
{
    public abstract class NpcFactory
    {
        private static readonly List<NpcType> Npcs = new()
        {
            NpcType.Villager,
            NpcType.TaxCollector,
            NpcType.Hero,
            NpcType.Villager
        };

        // Выдаем нпс из списка, когда список заканчивается, генерим рандомного
        public static NpcData GetNextVisitor()
        {
            if (Npcs.Count == 0)
            {
                var npcType = GetRandomNpcType();
                return GenerateNpcOfType(npcType);
            }

            var npc = Npcs.Last();
            Npcs.RemoveAt(Npcs.Count - 1);
            return GenerateNpcOfType(npc);
        }

        private static NpcData GenerateNpcOfType(NpcType npcType) => npcType switch
        {
            NpcType.Hero => GenerateRandomHeroNpc(),
            NpcType.TaxCollector => GenerateRandomTaxCollectorNpc(),
            NpcType.Villager => GenerateRandomVillagerNpc(),
            NpcType.Cultist => null,
            _ => throw new ArgumentOutOfRangeException()
        };

        private static NpcData GenerateRandomHeroNpc()
        {
            const NpcType npcType = NpcType.Hero;
            var npcName = GenerateNpcName(npcType);
            return new NpcData
            {
                NpcType = npcType,
                Level = Random.Range(1, 5),
                NpcName = npcName,
                GreetingsText = GenerateGreetingsText(),
                ByeText = GenerateGoodByeText()
            };
        }

        private static NpcData GenerateRandomTaxCollectorNpc()
        {
            const NpcType npcType = NpcType.TaxCollector;
            const string npcName = "Налоговый инспектор";
            return new NpcData
            {
                NpcType = npcType,
                NpcName = npcName,
                GreetingsText = new List<string> { "Доброго дня трактирщик, надеюсь, дела идут хорошо?" },
                ByeText = new List<string> { "Вот и славненько" }
            };
        }

        private static NpcData GenerateRandomVillagerNpc()
        {
            var npcType = NpcType.Villager;
            var npcName = GenerateNpcName(npcType);
            var quest = QuestFactory.GenerateQuest(npcName, npcType);
            return new NpcData
            {
                NpcType = npcType,
                NpcName = npcName,
                Quest = quest,
                GreetingsText = new List<string> { "Начало приветственной фразы", "конец приветственной фразы" },
                ByeText = new List<string> { "Ну да ладно, пойду а то дел по горло." }
            };
        }

        private static NpcType GetRandomNpcType()
        {
            var values = Enum.GetValues(typeof(NpcType));
            var random = Random.Range(0, values.Length);
            return (NpcType)values.GetValue(random);
        }

        private static List<string> GenerateGreetingsText()
        {
            return new List<string>()
            {
                Greetings1[Random.Range(0, Greetings1.Length)],
                Greetings2[Random.Range(0, Greetings2.Length)],
            };
        }

        private static string GenerateNpcName(NpcType npcType)
        {
            return npcType switch
            {
                NpcType.Hero => GetRandomHeroName(),
                NpcType.TaxCollector => "Сборщик налогов",
                NpcType.Villager => "Деревенский житель",
                NpcType.Cultist => "Культист ктулху",
                _ => throw new ArgumentOutOfRangeException(nameof(npcType), npcType, null)
            };
        }


        private static readonly string[] Greetings1 =
        {
            "Приветствую тебя, добрый трактирщик!",
            "Доброго тебе дня, страж питейного заведения!",
            "Здравствуй, хранитель очага и усталых душ!",
            "Мир тебе и твоему дому, великий хозяин таверны!",
            "Тебе привет, могучий повелитель кружек!",
            "Трактирщик, рад видеть твое приветливое лицо!",
            "Доброго времени суток, мастер угощений!",
            "Да хранит тебя судьба, доблестный хозяин этого места!",
            "Мир и покой тебе, о мудрый трактирщик!",
            "Да здравствует хозяин этого славного заведения!",
        };

        private static readonly string[] Greetings2 =
        {
            "Я — Избранный, пришедший исполнить свое предназначение.",
            "Я — герой, чьё имя уже стало легендой… хотя бы в одной деревне.",
            "Я тот, кто по пророчеству призван спасти мир… или хотя бы неплохо поесть.",
            "Меня зовут Избранный, и у меня есть судьба, с которой я как раз в пути.",
            "Я — Избранный! Времени мало, но для кружки-другой всегда найдется.",
            "Я тот, кому предначертано вершить великие дела... и проверять меню.",
            "Я — Избранный, ходящий по тропам судьбы... и заглядывающий в таверны.",
            "Я тот, кого ждали тысячи лет, чтобы вкусить твой славный эль.",
            "Я — Избранный, прибыл сюда, ибо ничто не остановит меня от доброй трапезы!",
            "Я тот, кому звезды предсказали великую миссию... и сочный ужин в таверне.",
        };


        private static string GetRandomHeroName()
        {
            var names = new List<string>
            {
                "Драгар Сын Дракона",
                "Старк Хранитель Света",
                "Ланс Сверкающий Меч",
                "Кратарс Гнев Олимпийцев",
                "Артис Избранный",
                "Соларий Искатель Света",
                "Уелл Клинок Фронтира",
                "Бьёрн Войн Стихий",
                "Орлан Страж Небес",
                "Такао Дух Сумрака",
                "Масон Тёмный Волк",
                "Пэйт Ловец Теней",
                "Сильвестр Железная Рука",
                "Элея Охотница за Правдой",
                "Фримен Легендарный",
                "Дэйн Изгнанный Провидец",
                "Грей Громовой Всадник",
                "Ивара Волчий Воин",
                "Магистр-Щит Ульфрик"
            };

            return names[Random.Range(0, names.Count)];
        }
        
        private static List<string> GenerateGoodByeText()
        {
            return new List<string>()
            {
                GoodByePart1[Random.Range(0, Greetings1.Length)],
                GoodByePart2[Random.Range(0, Greetings2.Length)],
            };
        }

        private static string[] GoodByePart1 =
        {
            "Ну что ж, трактирщик, пора мне идти.",
            "Время не ждёт, и я отправляюсь в путь.",
            "На этом прощай, добрый хозяин таверны!",
            "Благодарю за приют, но мне пора.",
            "До свидания, страж добрых напитков и блюд!",
            "Дорога зовёт, а значит, мне пора идти.",
            "Оставляю тебя с миром, добрый трактирщик!",
            "Мне пора на встречу с судьбой, но я не забуду эту таверну.",
            "Мой путь зовет меня дальше, и я прощаюсь.",
            "На этом я прощаюсь, трактирщик!",
        };

        private static string[] GoodByePart2 =
        {
            "Пусть твоя таверна процветает, пока я спасаю мир.",
            "Запомни моё имя, ибо оно войдёт в легенды… возможно, местные.",
            "Когда мир спасён, я вернусь за ещё одним кувшином эля.",
            "Жди меня обратно, когда очередной монстр будет побеждён… или если захочу перекусить.",
            "Да пребудет с тобой мир, пока я исполняю своё великое предназначение.",
            "И если тебе снова понадобится герой, знай, что я где-то поблизости.",
            "Вернусь с новыми подвигами… или как минимум с парой интересных историй.",
            "И пусть твой эль остаётся столь же крепким, как моя решимость спасать мир.",
            "И пусть твоя таверна останется моим убежищем от невзгод и суровых пророчеств.",
            "Когда деревни будут спасены, я непременно вернусь сюда на пир."
        };
    }
}