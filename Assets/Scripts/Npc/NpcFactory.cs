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

        // Generate NPCs from list, when list ends, generate random ones
        public static NpcData GetNextVisitor()
        {
            // Count NPCs. If more than _taxCollectorInterval, spawn collector
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
                "And also, you should buy my loot"
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
                NpcName = "Demon Raphael",
                GreetingsText = new List<string>
                {
                    $"{deadHeroData.NpcName} came to us in {deadHeroData.Quest.Location.Name} and was sacrificed.",
                    deadHeroData.Quest.Location.BadCompletionText,
                    $"Send us more souls and the Demon King will reward you. Here's your gold. [{deadHeroData.Quest.Location.RewardToReceive}]"
                },
                ByeText = new List<string>
                {
                    $"Bring the next sacrifice to {Location.GetById(nextLocation).Name}"
                },
                Quest = deadHeroData.Quest,
                PreVisitAction = () =>
                {
                    var text = $"Story progress: {Location.GetStoryCompletePercent()}%\n" +
                               $"Open map for details";
                            
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
            const string npcName = "Tax Inspector";
            return new NpcData
            {
                NpcType = npcType,
                NpcName = npcName,
                GreetingsText = new List<string> { "Good day innkeeper, I hope business is going well?" },
                ByeText = new List<string> { "But don't relax too much,\nI'll be back in a week." }
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
                ByeText = new List<string> { "Well anyway, I'll go, I'm up to my neck in work." }
            };
        }

        private static NpcType GetRandomNpcType()
        {
            var types = new List<NpcType> { NpcType.Villager, NpcType.Hero };

            return types[Random.Range(0, types.Count)];

            // Calculate probability for each type
            var total = villagerCount + heroCount + 1; // +1 to avoid division by zero at start
            var villagerProbability =
                (heroCount + 1) / total; // The more heroes, the higher probability of villager

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
                NpcType.TaxCollector => "Tax Collector",
                NpcType.Villager => "Village Dweller",
                NpcType.Cultist => "Demon King Cultist",
                _ => throw new ArgumentOutOfRangeException(nameof(npcType), npcType, null)
            };
        }

        private static NpcData GetVillagerFirstInteraction() => new()
        {
            NpcType = NpcType.Villager,
            NpcName = "Village Elder",
            IsIntro = true,
            GreetingsText = new List<string>
            {
                "Hello Innkeeper! As you know, the Demon King cult is awakening on our lands",
                "Plus we have enough other problems around, chaos everywhere",
                "Heroes sometimes visit you, can you direct them to help us?"
            },
            ByeText = new List<string> { "Thank you, farewell!" }
        };

        private static NpcData GetCultistFirstInteraction() => new()
        {
            NpcType = NpcType.Cultist,
            NpcName = "Demon Raphael",
            IsIntro = true,
            GreetingsText = new List<string>
            {
                "Hey innkeeper!\n Our cult is reviving the Demon King",
                "To resurrect him we need to collect 7 hero souls. \nSend them to our ambush and the dark lord will reward you",
                "You'll get a bag of coins for each one",
            },
            ByeText = new List<string> { "My dark lord waits. And I wait for heroes from you." }
        };

        private static NpcData GetTaxCollectorFirstInteraction() => new()
        {
            NpcType = NpcType.TaxCollector,
            NpcName = "Tax Inspector",
            IsIntro = true,
            GreetingsText = new List<string>
            {
                "Greetings!\nUgh, my head still hurts from yesterday's drinking, and now there's bad news too",
                "The Demon King cult is gaining strength again.\nThey're capturing our lands and outposts throughout the kingdom.",
                $"Therefore the Jarl is raising taxes for the army's needs.\nI'll come once a week, so prepare your gold. [{GameManager.Instance.TaxToPay}]",
            },
            ByeText = new List<string> { "I'll be back soon, farewell!" }
        };

        private static readonly string[] Greetings1 =
        {
            "I greet you, good innkeeper!",
            "Good day to you, guardian of the drinking establishment!",
            "Greetings, keeper of hearth and weary souls!",
            "Peace to you and your house, great tavern master!",
            "Greetings to you, mighty lord of mugs!",
            "Innkeeper, glad to see your welcoming face!",
            "Good time of day, master of treats!",
            "May fortune protect you, valiant owner of this place!",
            "Peace and tranquility to you, oh wise innkeeper!",
            "Long live the master of this glorious establishment!",
        };

        private static readonly string[] Greetings2 =
        {
            "I am the Chosen One, come to fulfill my destiny.",
            "I am a hero whose name has already become legend... at least in one village.",
            "I am he who by prophecy is called to save the world... or at least eat well.",
            "My name is the Chosen One, and I have a destiny I'm on my way to fulfill.",
            "I am the Chosen One! Time is short, but there's always time for a mug or two.",
            "I am he who is destined to do great deeds... and check the menu.",
            "I am the Chosen One, walking the paths of fate... and peeking into taverns.",
            "I am he whom thousands have awaited to taste your glorious ale.",
            "I am the Chosen One, arrived here, for nothing will stop me from a good meal!",
            "I am he to whom the stars predicted a great mission... and a hearty dinner at the tavern.",
        };


        private static readonly List<string> HeroNames = new()
        {
            "Gervant of Rivelia",
            "Stark the Light Keeper",
            "Lance Gleaming Sword",
            "Kratars Wrath of Olympians",
            "Artis the Chosen",
            "Solarius Light Seeker",
            "Well Frontier Blade",
            "Björn Storm Warrior",
            "Orlan Sky Guardian",
            "Takao Spirit of Dusk",
            "Mason Dark Wolf",
            "Pate Shadow Catcher",
            "Sylvester Iron Hand",
            "Leeroy Jenkins",
            "Freeman the Legendary",
            "Dane the Exiled Seer",
            "Grey Thunder Rider",
            "Ivar Wolf Warrior",
            "Master-Shield Ulfric"
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
            "Well then, innkeeper, it's time for me to go.",
            "Time doesn't wait, and I'm setting off on my journey.",
            "Farewell, good tavern master!",
            "Thank you for the shelter, but I must go.",
            "Goodbye, guardian of good drinks and food!",
            "The road calls, which means it's time for me to go.",
            "I leave you in peace, good innkeeper!",
            "I must meet my destiny, but I won't forget this tavern.",
            "My path calls me further, and I bid farewell.",
            "With this I bid farewell, innkeeper!",
        };

        private static readonly string[] GoodByePart2 =
        {
            "May your tavern prosper while I save the world.",
            "Remember my name, for it will enter legends... possibly local ones.",
            "When the world is saved, I'll return for another jug of ale.",
            "Wait for me back when the next monster is defeated... or if I want a snack.",
            "May peace be with you while I fulfill my great destiny.",
            "And if you need a hero again, know that I'm somewhere nearby.",
            "I'll return with new feats... or at least with a couple of interesting stories.",
            "And may your ale remain as strong as my resolve to save the world.",
            "And may your tavern remain my refuge from hardships and harsh prophecies.",
            "When villages are saved, I will surely return here for a feast."
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
            "Good day, tavern master!",
            "Hey, innkeeper! Glad to see you.",
            "Greetings, glorious innkeeper!",
            "Hello, good master!",
            "Peace to you, innkeeper!",
            "Glorious master, glad to see you!",
            "Hey, innkeeper! You're looking as good as ever.",
            "Greetings, innkeeper!",
            "Good day, honored master!",
            "Innkeeper, I greet you!",
        };

        private static readonly string[] GreetingsVillagerPart2 =
        {
            "You always have such a cozy place here.",
            "There's no warmth like yours anywhere.",
            "I see everything's still in its place with you.",
            "Your tavern is like a home away from home.",
            "Nothing changes - still the same comfort and peace.",
            "It's always pleasant to drop by your place.",
            "It smells so good here that you forget all troubles.",
            "As always, crowded and merry at your place.",
            "I knew I came to the right place.",
            "It always feels like a celebration is starting here.",
        };

        public static string GetQuestCompletionGreetings()
        {
            var x = new string[]
            {
                "I returned from the mission, battered but alive!",
                "Another feat completed!",
                "Finished faster than expected.",
                "Danger eliminated. Got some new loot!",
                "Everything went smoothly, as always.",
                "Here I am back, alive and well.",
                "I'm back! So what if I'm covered in mud? Such are the realities of heroism.",
                "Easy, as expected!",
                "One less quest in the world.",
                "Returned whole and unharmed, but my boots weren't so lucky.",
                "Another quest for the collection! Where's my medal?",
                "Another step toward level 80!",
                "Quest completed, mission accomplished.",
                "It was easier than I expected!",
                "Quest completed! Now I've definitely earned a snack.",
                "Quest finished, can I just lie down now?",
                "Don't ask how, but I managed it. Well, almost without casualties!",
            };

            return x[Random.Range(0, x.Length - 1)];
        }
    }
}