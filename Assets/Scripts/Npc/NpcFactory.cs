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
                NpcName = npcName,
                GreetingsText = new List<string>
                {
                    "Доброго дня трактирщик, недавно я стал избранным",
                    "я великий воин света и на мне лежит миссия по спасению мира"
                },
                ByeText = new List<string> { "Приключения ждут!" }
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

        private static List<string> GenerateGreetingsText(NpcType npcType)
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
            "Эй, трактирщик, что нового?",
            "Добрый день! Есть что-то освежающее?",
            "Приветствую, хозяин! Как идут дела?",
            "Сегодня снова полон дом, да?",
            "Что у тебя есть для уставшего путника?",
            "Хозяин, как поживаете?",
            "Эй, трактирщик, налей чего покрепче!",
            "Трактирщик, твой эль по-прежнему лучший!",
            "Что слышно в округе, друг?",
            "Есть свежие слухи для меня, старина?",
            "Доброе утро! Есть что перекусить?",
            "Сегодня много народу, да?",
            "Привет, хозяин, давно не виделись!",
            "Эй, трактирщик, я снова здесь!",
            "Как поживают твои бочки с элем?",
            "Устал с дороги. Чем порадуешь?",
            "Мир дому твоему, хозяин.",
            "Эй, трактирщик, я к тебе за гостеприимством!",
            "Сегодня у тебя людно, как никогда!",
            "Есть что-то новенькое на ужин?"
        };

        private static readonly string[] Greetings2 =
        {
            "...вчерашний эль был крепче, чем обычно!",
            "...сегодня снова слышал о банде разбойников в лесу.",
            "...надо бы узнать, какие слухи ходят в городе.",
            "...как всегда, ищу добрую еду и крепкий напиток!",
            "...пожалуй, начнем с кружки твоего лучшего эля.",
            "...холод сегодня пробирает до костей. Нужен согревающий эль!",
            "...а может, у тебя есть что-нибудь покрепче обычного?",
            "...расскажи, были ли интересные гости за последний час?",
            "...слышал, у тебя вчера побывал бард? Много народу собралось?",
            "...неужели ты забыл моего любимого блюда?",
            "...готовишь ли ты еще те жареные свиные ребрышки?",
            "...давно хотел послушать, что нового у наших старых друзей.",
            "...говорят, ты мастерски готовишь мясо на вертеле. Попробуем?",
            "...а с кем у тебя сейчас в таверне можно сыграть в кости?",
            "...не подскажешь, кто из местных может одолжить лошадь?",
            "...не слышал ли ты о таинственных следах у заброшенной шахты?",
            "...мне бы чашку горячего вина с пряностями!",
            "...если будешь мимо проезжать, заезжай ко мне на ферму.",
            "...искал здесь одного путешественника. Не появлялся ли он?",
            "...дружище, расскажи, что нового в наших краях за последнее время."
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
    }
}