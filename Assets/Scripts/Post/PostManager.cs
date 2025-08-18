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
            //NPCName died completing quest quest.objective
            return $"From the village elder:\n\n" +
                   $"We regret to inform you that {npc.NpcName} died trying to {quest.Objective}\n" +
                   $"Cause of death: {DeathReasons[UnityEngine.Random.Range(0, DeathReasons.Count)]}";
        }

        private string GetRewardLetterText(Quest quest, NpcData npc)
        {
            GameManager.IncreaseGold(quest.Gold);
            return $"From the village elder:\n\n" +
                   $"Thank you for sending us a hero.\n" +
                   $"{npc.NpcName} helped {quest.Objective.ToLower()}.\n" +
                   $"Please give him the reward: {quest.Gold} gold";
        }

        private static readonly List<string> DeathReasons = new()
        {
            "Tripped over his own sword and fell into a chasm.",
            "Decided to try a strange-looking mushroom and got poisoned.",
            "Attacked a bear, thinking it was just a large dog.",
            "Collected too much loot and strained his back",
            "Got caught in a trap and couldn't escape.",
            "Accidentally lit a red barrel on fire and exploded",
            "Overestimated his strength in battle with a giant spider.",
            "Fell asleep by the campfire and burned up.",
            "Fell into a river while crossing it in armor.",
            "Tried to negotiate with a dragon, but it wasn't in the mood.",
            "Accidentally drank poison instead of an invisibility potion.",
            "Unsuccessfully activated an ancient magical artifact.",
            "Got caught in a cave-in in an old mine.",
            "Decided to save a kitten stuck on a cliff edge and fell down.",
            "Got too close to lava during a volcanic eruption.",
            "Stepped on a forgotten trap in an old castle.",
            "Decided to have a snack, not noticing the food was cursed.",
            "Couldn't outrun a pack of wild wolves.",
            "Ate cursed food and became undead.",
            "Stepped on a rake... ten times in a row.",
            "Tried to open a chest that turned out to be a mimic.",
            "Got tangled in his own cloak and suffocated.",
            "Accidentally sold his soul to the devil.",
            "Got stuck in textures and died of starvation.",
            "Tried to drink a potion whole without opening the cap.",
            "Impaled himself on his own arrow while shooting into the wind.",
            "Stepped on an anthill and was beaten to death by an army of ants.",
            "Forgot that he ran out of health potions... and life.",
            "Tripped on a flat surface and broke all his bones.",
            "Tried to talk to an NPC who turned out to be a hidden boss.",
            "Endlessly tried to pick up a dropped item until he died of exhaustion.",
            "Stepped on a banana peel and fell off a cliff.",
            "Stared at his reflection in the water too long and drowned.",
            "Thought he could jump from a height without consequences. He was wrong.",
            "Overheated from his new fire armor on a hot day.",
            "Was killed by level 80 goblins",
            "Decided that fighting a rat was too boring. He was wrong.",
            "Choked on a fish bone",
            "Spent a long time trying to hug a cactus because it was 'actually soft'."
        };
    }
}