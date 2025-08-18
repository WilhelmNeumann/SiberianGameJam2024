using System.Collections.Generic;
using System.Linq;
using Npc;
using UnityEngine;

namespace Quests
{
    public abstract class QuestFactory
    {
        /// Generate random quests
        public static Quest GenerateQuest(string questGiverName, NpcType npcType)
        {
            // objective and solution has same index
            var randomIndex = Random.Range(0, objectives.Count);
            var problemDescription = problems.ElementAt(randomIndex).Key;
            var mainSkill = problems[problemDescription];
            var q = new Quest
            {
                Xp = Random.Range(1, 50),
                Gold = Random.Range(10, 500),
                Difficulty = Random.Range(1, 5),
                Objective = objectives[randomIndex],
                ApplicationText =
                    $"{introductionPhrases[Random.Range(0, introductionPhrases.Count)]} {problemDescription}, {solutions[Random.Range(0, solutions.Count)]}",
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
            var location = Location.Locations.FirstOrDefault(x => x.State == LocationState.Neutral);
            
            // Return null if no more main quests are available
            if (location == null)
                return null;

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

        // Absurd quest templates
        private static List<string> introductionPhrases = new()
        {
            "Haven't you heard what's happening around here?",
            "People here are whispering that problems have started again.",
            "Listen, I need help.",
            "Something happened in the village, and I need help.",
            "Sorry, but I need support.",
            "Have you heard what troubles are ahead of us?",
            "Ugh, the village has become restless again.",
            "You won't believe what's happening with us!",
            "We have trouble here.",
            "It seems we can't do without your help.",
            "We need someone with strong nerves and a kind heart - we're in trouble.",
            "See how sad everything is? So I came here to complain.",
            "The village needs help, otherwise we'll be completely lost.",
            "It seems troubles have visited us again.",
            "Ugh, if only someone helped us with all this...",
            "We have trouble upon trouble, and no end in sight.",
            "You're a hero, aren't you? Then we have a task for you here."
        };

        private static Dictionary<string, MainSkill> problems = new()
        {
            { "The apothecary has rats in his basement", MainSkill.Strength },
            { "The apothecary's son was attacked by bears, and now he thirsts for revenge", MainSkill.Strength },
            { "A huge turnip grew on the square that scares children", MainSkill.Strength },
            { "The village elder's old rooster crows at night", MainSkill.Charisma },
            { "A talking crow flew into the tavern and teases everyone", MainSkill.Charisma },
            { "The miller's wife went into the forest and hasn't returned, she's been missing for a week", MainSkill.Intelligence },
            { "Strange laughter is heard in the meadow at night, and no one knows whose it is", MainSkill.Intelligence },
            { "The shepherd's sheep have become aggressive and attack people", MainSkill.Strength },
            { "A hut appeared in the forest that wasn't there before", MainSkill.Intelligence },
            { "There are suddenly too many frogs in the pond - they're scaring everyone away", MainSkill.Charisma },
            { "The milkmaid's milk jug suddenly ran away by itself", MainSkill.Intelligence },
            { "The well started gurgling strangely at night, and people are afraid to approach it", MainSkill.Intelligence },
            {
                "Cultists stole the farmer's beloved hoe that was passed down through generations",
                MainSkill.Strength
            },
        };

        private static List<string> solutions = new()
        {
            "we need to deal with this before it's too late.",
            "someone must put an end to this.",
            "we need to stop this before it gets out of control.",
            "we need help, and everyone will be grateful.",
            "this needs to be stopped before someone gets hurt.",
            "We need to sort this out before it grows into even bigger problems.",
            "We need to put an end to this before everyone goes crazy.",
            "someone needs to take care of this, otherwise there will be no peace.",
            "We can't just leave this be, someone must intervene.",
            "Before it gets worse, we need to solve this issue.",
            "We need to deal with this while there's still a chance for a peaceful life.",
            "It's better to stop this before it becomes a habit.",
            "If we don't sort it out now, it'll be even harder later.",
            "We'd better put an end to this, and everyone will feel better.",
            "It seems we need to intervene before they cause trouble.",
            "This needs to be resolved before rumors spread throughout the village."
        };

        private static List<string> objectives = new()
        {
            "Kill 50 rats in the apothecary's basement.",
            "Bring 100 bear pelts.",
            "Dig up and dispose of the huge turnip from the square.",
            "Calm the elder's rooster so it stops crowing.",
            "Banish the talking crow from the tavern.",
            "Find out what happened to the miller's wife",
            "Find the source of the strange laughter in the meadow",
            "Deal with the aggressive sheep",
            "Find out where the hut in the forest came from",
            "Deal with the frog swarm",
            "Find out why the milk ran away",
            "Deal with the gurgling well",
            "Return the family hoe to the farmer"
        };

        private static List<string> completions = new()
        {
            "I killed the rats in the apothecary's basement, tough little buggers they were. I came for my reward.",
            "I killed all the damn bears in the area, hopefully it was worth it. Give me my reward!",
            "The turnip is dug up and won't bother anyone anymore. There should be a reward.",
            "The elder's rooster no longer disturbs the village. He won't bother anyone anymore.",
            "The talking crow is gone, and there's silence in the tavern again.",
            "The miller's wife was torn apart by wolves, but I dealt with them.",
            "There was a ghost of an old drunk in the meadow who laughed at a joke that killed him. I dealt with him.",
            "The sheep were bewitched by cultists, but I killed them all. The sheep, not the cultists.",
            "I found out where the hut came from. It's all a wizard and his teleportation experiments.\nI teleported him far away.",
            "I talked to the frog king - he promised to lead his subjects away from the village",
            "Turns out it was a house spirit's doing - made a deal with him, now the milk will stay put.",
            "I solved the problem radically and destroyed the well. It will never dare to gurgle again.",
            "I infiltrated the cultist camp and killed them all for this hoe. I deserve a reward."
        };
    }
}