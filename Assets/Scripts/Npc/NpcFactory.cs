using System;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using NUnit.Framework;
using Quests;
using Ui;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Npc
{
    public static class NpcFactory
    {
        private static int villagerCount = 0;
        private static int heroCount = 0;
        private static int phase = 1;

        private const int _taxCollectorInterval = 7;
        private static int _npcCount = 0;

        private static readonly List<NpcData> NpcsQueue = new()
        {
            // GetTaxCollectorFirstInteraction(),
            // GetVillagerFirstInteraction(),
            // GetCultistFirstInteraction(),
        };

        public static Queue<NpcData> HeroLogs { get; private set; } = new Queue<NpcData>();

        public static void AddHeroToLogs(NpcData npcData)
        {
            if (npcData.NpcType != NpcType.Hero) return;
            if (HeroLogs.Contains(npcData)) return;
            HeroLogs.Enqueue(npcData);
            if (HeroLogs.Count > 5)
            {
                HeroLogs.Dequeue();
            }
        }

        // Выдаем нпс из списка, когда список заканчивается, генерим рандомного
        public static NpcData GetNextVisitor()
        {
            // Считаем кол-вл НПС. Если больше чем _taxCollectorInterval, то спавним коллектора
            _npcCount += 1;
            if (_npcCount >= _taxCollectorInterval)
            {
                _npcCount = 0;
                phase++;
                NpcsQueue.Add(GenerateNpcOfType(NpcType.TaxCollector));
            }

            if (NpcsQueue.Count < 3)
            {
                while (NpcsQueue.Count < 3)
                {
                    NpcsQueue.Add(GenerateRandomNpc());
                }
            }

            var npc = NpcsQueue[0];
            NpcsQueue.RemoveAt(0);

            if (NpcsQueue.Count < 2)
                NpcsQueue.Add(GenerateRandomNpc());
            return npc;
        }

        public static void AddNpcToQueue(NpcData npc)
        {
            if (npc.Quest is { Location: { ID: 10 }, QuestType: QuestType.MainQuest, QuestState: QuestState.Success })
            {
                GameManager.Instance.GoodEnd();
                return;
            }

            npc.GreetingsText = new List<string>()
            {
                GetQuestCompletionGreetings(),
                npc.Quest.CompletionText,
                "И еще, ты должен купить мой лут"
            };
            NpcsQueue.Add(npc);
        }

        private static int GenerateLevel(int phasa)
        {
            int[] levels;
            float[] weights;
            switch (phasa)
            {
                case 1:
                    levels = new[] { 1, 2 };
                    weights = new[] { 0.6f, 0.4f };
                    return GetWeightedRandomLevel(levels, weights);
                case 10:
                    levels = new[] { 9, 10 };
                    weights = new[] { 0.4f, 0.6f };
                    return GetWeightedRandomLevel(levels, weights);
            }

            levels = new[] { phasa - 1, phasa, phasa + 1 };
            weights = new[] { 0.2f, 0.6f, 0.2f };
            return GetWeightedRandomLevel(levels, weights);
        }


        private static int GetWeightedRandomLevel(int[] levels, float[] weights)
        {
            var totalWeight = 0f;
            for (var i = 0; i < weights.Length; i++)
                totalWeight += weights[i];

            var randomValue = Random.Range(0, totalWeight);
            var cumulativeWeight = 0f;

            for (var i = 0; i < levels.Length; i++)
            {
                cumulativeWeight += weights[i];
                if (randomValue < cumulativeWeight)
                    return levels[i];
            }

            return levels[^1];
        }

        public static void AddDemonToTheQueue(NpcData deadHeroData)
        {
            var nextLocation = deadHeroData.Quest.Location.ID + 1;
            if (nextLocation > Location.Locations.Count)
            {
                GameManager.Instance.BadEnd();
                return;
            }

            var demon = new NpcData
            {
                NpcType = NpcType.Cultist,
                Level = 0,
                IsIntro = true,
                NpcName = "Демон Рафаэль",
                GreetingsText = new List<string>
                {
                    $"{deadHeroData.NpcName} пришел к нам в {deadHeroData.Quest.Location.Name} и несен в жертву.",
                    deadHeroData.Quest.Location.BadCompletionText,
                    $"Отправляй к нам больше душ и Король Демонов Тебя вознаградит. Вот твое золото. [{deadHeroData.Quest.Location.RewardToReceive}]"
                },
                ByeText = new List<string>
                {
                    $"Следующую жертву приведи в {Location.GetById(nextLocation).Name}"
                },
                Quest = deadHeroData.Quest,
                PreVisitAction = () =>
                {
                    var text = $"Прогресс сюжета: {Location.GetStoryCompletePercent()}%\n" +
                               $"Откройте карту для подробностей";
                            
                    MapUpdatePopup.Instance.SetText(text);
                    MapUpdatePopup.Instance.gameObject.SetActive(true);
                }
            };
            NpcsQueue.Add(demon);
        }

        private static NpcData GenerateRandomNpc()
        {
            if (QuestJournal.Instance.SideQuests.Count >= 3)
            {
                return GenerateNpcOfType(NpcType.Hero);
            }

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

            var npc = new NpcData
            {
                NpcType = npcType,
                Level = GenerateLevel(phase),
                NpcName = npcName,
                GreetingsText = GenerateGreetingsText(),
                ByeText = GenerateGoodByeText()
            };

            DistributeSkillPoints(npc);
            return npc;
        }

        private static void DistributeSkillPoints(NpcData npcData)
        {
            // Calculate the total skill points based on the character level.
            var totalSkillPoints = npcData.Level * 2; // Example: 5 points per level

            // Assign primary characteristic with a weight of 0.4 and others with 0.3 each.
            const float primaryWeight = 0.4f;
            var secondaryWeight = 0.3f;

            // Select primary characteristic randomly.
            var primaryCharacteristic = Random.Range(0, 3);

            // Distribute points based on chosen primary characteristic.
            int primaryPoints = Mathf.RoundToInt(totalSkillPoints * primaryWeight);
            int secondaryPoints = Mathf.RoundToInt(totalSkillPoints * secondaryWeight);

            if (primaryCharacteristic == 0)
            {
                npcData.Strength = primaryPoints;
                npcData.Intelligence = secondaryPoints;
                npcData.Charisma = totalSkillPoints - (primaryPoints + secondaryPoints);
            }
            else if (primaryCharacteristic == 1)
            {
                npcData.Intelligence = primaryPoints;
                npcData.Strength = secondaryPoints;
                npcData.Charisma = totalSkillPoints - (primaryPoints + secondaryPoints);
            }
            else
            {
                npcData.Charisma = primaryPoints;
                npcData.Strength = secondaryPoints;
                npcData.Intelligence = totalSkillPoints - (primaryPoints + secondaryPoints);
            }
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
                ByeText = new List<string> { "Но сильно не расслабляйся,\nя вернусь через неделю." }
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

            return types[Random.Range(0, types.Count)];

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
                "Эй трактирщик!\n Наш культ возрождает Короля Демонов",
                "Для его воскрешения нам нужно собрать 7 душ героев. \nОтправляй их к нам в засаду и темный владыка тебя вознаградит",
                "Будешь получать кошель монет за каждого",
            },
            ByeText = new List<string> { "Мой темный владыка ждет. А я жду от тебя героев." }
        };

        private static NpcData GetTaxCollectorFirstInteraction() => new()
        {
            NpcType = NpcType.TaxCollector,
            NpcName = "Налоговый инспектор",
            IsIntro = true,
            GreetingsText = new List<string>
            {
                "Приветствую!\nЭх, после вчерашней попойки до сих пор болит голова, так еще и новости плохие",
                "Культ Короля Демонов вновь набирает силу.\nОни захватывают наши земли и аванпосты по всему королевству.",
                $"Поэтому Ярл поднимает налоги на нужды армии.\nЯ буду приходить раз в неделю, так что готовь золотишко. [{GameManager.Instance.TaxToPay}]",
            },
            ByeText = new List<string> { "Я скоро вернусь, бывай!" }
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


        private static readonly List<string> HeroNames = new()
        {
            "Гервант из Рыбии",
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
            "Лерой Дженкинс",
            "Фримен Легендарный",
            "Дэйн Изгнанный Провидец",
            "Грей Громовой Всадник",
            "Ивар Волчий Воин",
            "Магистр-Щит Ульфрик"
        };

        private static List<string> _heroNamesToPick = new();

        private static string GetRandomHeroName()
        {
            if (_heroNamesToPick.Count == 0)
            {
                _heroNamesToPick = HeroNames.ToList();
                _heroNamesToPick.MMShuffle();
            }
            
            var name = _heroNamesToPick[0];
            _heroNamesToPick.RemoveAt(0);
            return name;
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

        private static List<string> GenerateVillagerGreetingsText()
        {
            return new List<string>()
            {
                GreetingsVillagerPart1[Random.Range(0, Greetings1.Length)] + "\n" +
                GreetingsVillagerPart2[Random.Range(0, Greetings2.Length)]
            };
        }

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

        public static string GetQuestCompletionGreetings()
        {
            var x = new string[]
            {
                "Я вернулся с задания, потрепанный но живой!",
                "Очередной подвиг завершен!",
                "Справился быстрее, чем ожидал.",
                "Опасность ликвидирована. Обзавелся новым лутом!",
                "Все прошло гладко, как всегда.",
                "Вот и вернулся живым и невредимым.",
                "Я вернулся! Ну и что, что я весь в грязи? Таковы реалии героизма.",
                "Легко, как и предполагалось!",
                "На один квест меньше в мире.",
                "Вернулся целым и невредимым, а вот ботинки — не повезло.",
                "Еще один квест в копилку! Где тут моя медаль?",
                "Еще один шаг к 80 уровню!",
                "Квест выполнен, миссия завершена.",
                "Это было проще, чем ожидал!",
                "Квест пройден! Теперь точно заслужил перекус.",
                "Квест выполнен, можно я теперь просто прилягу?",
                "Не спрашивайте, как, но я справился. Ну, почти без жертв!",
            };

            return x[Random.Range(0, x.Length - 1)];
        }
    }
}