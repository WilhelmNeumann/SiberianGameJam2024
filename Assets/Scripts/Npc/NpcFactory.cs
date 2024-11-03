using System;
using System.Collections.Generic;
using Quests;
using Random = UnityEngine.Random;

namespace Npc
{
    public static class NpcFactory
    {
        private static int villagerCount = 0;
        private static int heroCount = 0;

        private static readonly List<NpcData> NpcsQueue = new()
        {
            // GetTaxCollectorFirstInteraction(),
            // GetVillagerFirstInteraction(),
            // GetCultistFirstInteraction()
        };

        // Выдаем нпс из списка, когда список заканчивается, генерим рандомного
        public static NpcData GetNextVisitor()
        {
            if (NpcsQueue.Count == 0)
            {
                return GenerateRandomNpc();
            }

            var npc = NpcsQueue[0];
            NpcsQueue.RemoveAt(0);

            if (NpcsQueue.Count < 3)
                NpcsQueue.Add(GenerateRandomNpc());
            return npc;
        }

        private static NpcData GenerateRandomNpc()
        {
            var npcType = GetRandomNpcType();
            return GenerateNpcOfType(npcType);
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
                GreetingsText = GenerateVillagerGreetingsText(),
                ByeText = new List<string> { "Ну да ладно, пойду а то дел по горло." }
            };
        }

        private static NpcType GetRandomNpcType()
        {
            var types = new List<NpcType> { NpcType.Villager, NpcType.Hero };

            // Рассчитаем вероятность для каждого типа
            var total = villagerCount + heroCount + 1; // +1 чтобы избежать деления на ноль в начале
            var villagerProbability =
                (heroCount + 1) / total; // Чем больше героев, тем выше вероятность деревенского жителя

            var roll = new System.Random().NextDouble();

            if (roll < villagerProbability)
            {
                villagerCount++;
                return NpcType.Villager;
            }
            else
            {
                heroCount++;
                return NpcType.Hero;
            }
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
                NpcType.Cultist => "Культист Короля Демонов",
                _ => throw new ArgumentOutOfRangeException(nameof(npcType), npcType, null)
            };
        }

        private static NpcData GetVillagerFirstInteraction() => new()
        {
            NpcType = NpcType.Villager,
            NpcName = "Староста деревни",
            IsIntro = true,
            GreetingsText = new List<string>
            {
                "Привет Трактирщик! Как ты знаешь, на наших землях пробуждается культ Короля демонов",
                "Так еще и других проблем у нас в округе хватает, повсюду бардак",
                "К тебе тут иногда захаживают Герои, можешь направлять их к нам на помощь?"
            },
            ByeText = new List<string> { "Спасибо тебе, бывай!" }
        };

        private static NpcData GetCultistFirstInteraction() => new()
        {
            NpcType = NpcType.Cultist,
            NpcName = "Демон Рафаэль",
            IsIntro = true,
            GreetingsText = new List<string>
            {
                "Здравствуй трактирщик! Наш культ пытается возродить Короля Демонов",
                "Для его воскрешения нам нужны души героев",
                "Если к тебе зайдет парочка, отправь их к нам, мы в долгу не останемся",
            },
            ByeText = new List<string> { "Спасибо тебе, бывай!" }
        };

        private static NpcData GetTaxCollectorFirstInteraction() => new()
        {
            NpcType = NpcType.TaxCollector,
            NpcName = "Налоговый инспектор",
            IsIntro = true,
            GreetingsText = new List<string>
            {
                "Приветствую!\n Эх, после вчерашней попойки до сих пор болит голова, так еще и новости плохие",
                "Культ Короля Демонов вновь набирает силу. Они захватывают наши земли и аванпосты.",
                "Поэтому Ярл поднимает налоги на нужны армии. Я буду приходить раз в неделю, так что готовь золотишко.",
            },
            ByeText = new List<string> { "Теперь пора идти к Кузнецу, бывай" }
        };

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

        private static List<string> GenerateGoodByeText() => new()
        {
            GoodByePart1[Random.Range(0, Greetings1.Length)],
            GoodByePart2[Random.Range(0, Greetings2.Length)],
        };


        private static readonly string[] GoodByePart1 =
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

        private static readonly string[] GoodByePart2 =
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

        private static List<string> GenerateVillagerGreetingsText() => new()
        {
            GreetingsVillagerPart1[Random.Range(0, Greetings1.Length)],
            GreetingsVillagerPart2[Random.Range(0, Greetings2.Length)],
        };

        private static readonly string[] GreetingsVillagerPart1 =
        {
            "Доброго дня, хозяин таверны!",
            "Эй, трактирщик! Рад тебя видеть.",
            "Приветствую, славный трактирщик!",
            "Здравствуй, добрый хозяин!",
            "Мир тебе, трактирщик!",
            "Славный хозяин, рад видеть тебя!",
            "Эй, трактирщик! Ты, как всегда, в полном порядке.",
            "Здрав будь, трактирщик!",
            "Добрый день, почтенный хозяин!",
            "Трактирщик, приветствую тебя!",
        };

        private static readonly string[] GreetingsVillagerPart2 =
        {
            "У тебя здесь всегда так уютно.",
            "Нигде нет такого тепла, как у тебя.",
            "Вижу, всё у тебя по-прежнему на месте.",
            "Твоя таверна — как дом родной.",
            "Ничего не меняется — всё такой же уют и покой.",
            "Всегда приятно к тебе заглянуть.",
            "Тут так пахнет, что забываешь обо всех бедах.",
            "Как всегда многолюдно и весело у тебя.",
            "Знать, не зря я сюда зашёл.",
            "Здесь всегда словно праздник начинается.",
        };
    }
}