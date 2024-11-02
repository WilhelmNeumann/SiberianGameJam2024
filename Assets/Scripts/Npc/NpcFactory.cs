using System;
using System.Collections.Generic;
using Quests;
using Random = UnityEngine.Random;

namespace Npc
{
    public abstract class NpcFactory
    {
        public static NpcData GenerateNpc()
        {
            var npcType = GetRandomNpcType();
            var npcName = GenerateNpcName(npcType);
            var quest = QuestFactory.GenerateQuest(npcName, npcType);
            var greetingsText = GenerateGreetingsText(npcType);

            return new NpcData
            {
                NpcType = npcType,
                NpcName = npcName,
                Quest = quest,
                GreetingsText = greetingsText,
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
                NpcType.Hero => "Избранный крокодилорожденный",
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
    }
}