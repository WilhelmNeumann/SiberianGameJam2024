using System.Collections;
using System.Collections.Generic;
using Npc;
using Quests;
using TMPro;
using UnityEngine;
using Utils;

namespace System
{
    public class PostManager : Singleton<PostManager>
    {
        [SerializeField] private TMP_Text lettersCountText;

        [SerializeField] private Vector2 position;

        private Queue<String> _quests = new Queue<String>();

        private void Start()
        {
            UpdateCounter();
        }

        public void AddQuestCompleted(Quest quest, NpcData npc)
        {
            _quests.Enqueue(GetRewardLetterText(quest, npc));
            UpdateCounter();
        }

        public void AddHeroDiedLetter(Quest quest, NpcData npc)
        {
            _quests.Enqueue(GetHeroDiedLetterText(quest, npc));
            UpdateCounter();
        }

        public String GetQuest()
        {
            var nextText = _quests.Dequeue();
            UpdateCounter();
            return nextText;
        }

        public void UpdateCounter()
        {
            var questCount = _quests.Count;
            transform.position = questCount > 0 ? position : new Vector2(-1000, -1000);
            lettersCountText.text = questCount.ToString();
        }

        private string GetHeroDiedLetterText(Quest quest, NpcData npc)
        {
            //NPCName умер выполняя квест quest.objective
            return $"От старосты деревни:\n\n" +
                   $"С прискорбием сообщаем, что {npc.NpcName} умер пытаясь {quest.Objective}\n" +
                   $"Причина смерти: {DeathReasons[UnityEngine.Random.Range(0, DeathReasons.Count)]}";
        }

        private string GetRewardLetterText(Quest quest, NpcData npc)
        {
            GameManager.IncreaseGold(quest.Gold);
            return $"От старосты деревни:\n\n" +
                   $"Благодарим что отправили к нам героя.\n" +
                   $"{npc.NpcName} помог {quest.Objective.ToLower()}.\n" +
                   $"Передайте ему награду: {quest.Gold} золота";
        }

        private static readonly List<string> DeathReasons = new()
        {
            "Споткнулся о собственный меч и упал в пропасть.",
            "Решил попробовать странный на вид гриб и отравился.",
            "Напал на медведя, думая, что это просто крупная собака.",
            "Набрал слишком много лута и надорвался",
            "Попал в ловушку и не смог выбраться.",
            "Случайно поджег красную бочку, и взорвался",
            "Переоценил свои силы в битве с гигантским пауком.",
            "Заснул у костра и сгорел.",
            "Упал в реку, переплывая ее в доспехах.",
            "Попытался договориться с драконом, но тот был не в настроении.",
            "Случайно выпил яд, вместо зелья невидимости.",
            "Неудачно активировал древний магический артефакт.",
            "Попал под обвал в старой шахте.",
            "Решил спасти котенка, застрявшего на краю обрыва, и сорвался вниз.",
            "Слишком близко подошел к лаве во время вулканического извержения.",
            "Подорвался на забытой кем-то ловушке в старом замке.",
            "Решил перекусить, не заметив, что еда была проклята.",
            "Не смог убежать от стаи диких волков.",
            "Съел проклятую еду и стал нежитью.",
            "Наступил на грабли... десять раз подряд.",
            "Пытался открыть сундук, который оказался мимиком.",
            "Запутался в собственном плаще и задохнулся.",
            "Случайно продал свою душу дьяволу.",
            "Застрял в текстурах и умер от голода.",
            "Пытался съесть зелье целиком, не открыв крышку.",
            "Напоролся на свою же стрелу, стреляя по ветру.",
            "Наступил на муравейник и был забит до смерти армией муравьёв.",
            "Забыл, что у него кончились зелья здоровья... и жизнь.",
            "Споткнулся на ровной поверхности и сломал себе все кости.",
            "Попытался пообщаться с NPC, который оказался скрытым боссом.",
            "Бесконечно пытался подобрать выпавший предмет, пока не умер от истощения.",
            "Наступил на банановую кожуру и упал со скалы.",
            "Слишком долго любовался своим отражением в воде и утонул.",
            "Решил, что может прыгнуть с высоты без последствий. Ошибся.",
            "Перегрелся от своей новой огненной брони в жаркий день.",
            "Был убит гоблинами 80 уровня",
            "Решил, что сражаться с крысой — это слишком скучно. Ошибся.",
            "Подавился рыбной костью",
            "Долго пытался обнять кактус, потому что тот был «на самом деле мягким»."
        };
    }
}