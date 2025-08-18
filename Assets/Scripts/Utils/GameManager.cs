using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Dialogs;
using Npc;
using Quests;
using TMPro;
using Ui;
using UnityEngine;
using Random = System.Random;

namespace Utils
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] public TextMeshProUGUI GoldTextMesh;
        [SerializeField] public int Gold = 1000;
        [SerializeField] public int TaxToPay = 500;
        [SerializeField] private RectTransform gameOverPanel;
        [SerializeField] private RectTransform goodGamePanel;
        [SerializeField] private RectTransform badGamePanel;

        [SerializeField] public TextMeshProUGUI strengthPotionsText;
        [SerializeField] public TextMeshProUGUI charismaPotionsText;
        [SerializeField] public TextMeshProUGUI intelligencePotionsText;

        public static NpcData CurrentNpcData;
        public static TavernNpc CurrentTavernNpc;

        private static Dictionary<PotionType, int> Potions = new Dictionary<PotionType, int>()
        {
            { PotionType.Charisma, 0 },
            { PotionType.Intelligence, 0 },
            { PotionType.Strength, 0 },
        };

        public IEnumerator Start()
        {
            SetPotionValue(PotionType.Charisma, 1);
            SetPotionValue(PotionType.Intelligence, 1);
            SetPotionValue(PotionType.Strength, 1);

            while (true)
            {
                yield return GameplayLoop();
            }
        }

        private static bool IsHeroFailedSideQuest(NpcData npcData) =>
            npcData.NpcType == NpcType.Hero &&
            npcData.Quest is { QuestState: QuestState.Failed };


        private static IEnumerator GameplayLoop()
        {
            var npcData = NpcFactory.GetNextVisitor();
            CurrentNpcData = npcData;

            npcData.PreVisitAction?.Invoke();
            npcData.PreVisitAction = null;

            if (IsHeroFailedSideQuest(npcData))
            {
                PostManager.Instance.AddHeroDiedLetter(npcData.Quest, npcData);
                yield return null;
            }
            else
            {
                var npc = NpcManager.Instance.CreateNpc(npcData);
                var tavernNpc = npc.GetComponent<TavernNpc>();
                CurrentTavernNpc = tavernNpc;
                yield return tavernNpc.WalkIn();

                var scenario = GetDialogScenario(npcData);

                foreach (var dialogLine in scenario)
                {
                    DialogWindow.Instance.NpcTalk(dialogLine.Text, npcData.NpcName);
                    yield return new WaitUntil(() => DialogWindow.Instance.CanContinue);

                    if (dialogLine.ResponseOptions != null)
                    {
                        DialogWindow.Instance.ShowPlayerDialogOptions(dialogLine.ResponseOptions);
                        yield return new WaitUntil(() => DialogWindow.Instance.CanContinue);
                    }
                }

                DialogWindow.Instance.Hide();
                yield return tavernNpc.WalkOut();
                npcData.PostVisitAction?.Invoke();
                npcData.PostVisitAction = null;
                yield return null;
            }
        }


        // Get dialog scenario with all lines
        private static List<DialogLine> GetDialogScenario(NpcData npcData)
        {
            var scenario = new List<DialogLine>();
            var greetings = npcData.GreetingsText.Select(ToNpcTalkDialogLine).ToList();
            AddGreetingsResponse(npcData, greetings);
            scenario.AddRange(greetings);

            var npcDialog = npcData.NpcType switch
            {
                NpcType.Hero => GetDialogWithHero(npcData),
                NpcType.TaxCollector => GetDialogWithTaxCollector(npcData),
                NpcType.Villager => GetDialogWithVillager(npcData),
                NpcType.Cultist => GetDialogWithCultist(npcData),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (npcDialog != null)
                scenario.Add(npcDialog);

            var goodBye = npcData.ByeText.Select(ToNpcTalkDialogLine).ToList();
            scenario.AddRange(goodBye);
            return scenario;
        }

        private static void AddGreetingsResponse(NpcData npcData, List<DialogLine> greetings)
        {
            DialogOption option;
            // Cultist arrives after hero died
            if (npcData.NpcType == NpcType.Cultist && npcData.Quest != null)
            {
                option = new()
                {
                    Text = "I make no promises",
                    Action = () => { IncreaseGold(npcData.Quest.Location.RewardToReceive); }
                };
            }
            // Hero arrives who completed a side quest
            else if (npcData.Quest is { QuestState: QuestState.Success, QuestType: QuestType.SideQuest })
            {
                var reward = npcData.Quest.Gold;
                option = new DialogOption
                {
                    // If hero completed side quest, client pays us and we take a cut
                    Text = $"Here's your reward.\n [Pay {reward / 10} gold]",
                    Action = () => { ReduceGold(reward / 10); }
                };

                var questCopy = new Quest
                {
                    Objective = npcData.Quest.Objective,
                    Gold = npcData.Quest.Gold
                };
                var npcCopy = new NpcData { NpcName = npcData.NpcName };
                npcData.PostVisitAction = () => { PostManager.Instance.AddQuestCompleted(questCopy, npcCopy); };
            }
            // Hero arrives who completed main quest
            else if (npcData.Quest is { QuestState: QuestState.Success, QuestType: QuestType.MainQuest })
            {
                var reward = npcData.Quest.Gold;
                option = new DialogOption
                {
                    // If hero completed main quest, we pay from our own pocket
                    Text =
                        $"Thank you, {npcData.Quest.Location.Name} can sleep peacefully.\n [Pay {npcData.Quest.Gold} gold]",
                    Action = () =>
                    {
                        ReduceGold(reward);
                        AddRandomPotion();
                    }
                };
            }
            else
            {
                // Everyone else gets a greeting, except intro characters
                option = npcData.IsIntro
                    ? new DialogOption { Text = "Alright, I'll see what I can do" }
                    : new DialogOption { Text = "Welcome to \"The Cracked Barrel\", how can I help you?" };
            }

            greetings.Last().ResponseOptions = new List<DialogOption> { option };
        }

        private static void LevelUpHero(NpcData npcData)
        {
            // Level up hero, each level gives 2 skill points. One always goes to highest stat
            // Second one is distributed randomly
            npcData.Level += 1;

            if (npcData.Strength >= npcData.Intelligence && npcData.Strength >= npcData.Charisma)
                npcData.Strength++;
            else if (npcData.Intelligence >= npcData.Strength && npcData.Intelligence >= npcData.Charisma)
                npcData.Intelligence++;
            else
                npcData.Charisma++;

            var rand = UnityEngine.Random.Range(0, 3);
            switch (rand)
            {
                case 0:
                    npcData.Strength++;
                    break;
                case 1:
                    npcData.Intelligence++;
                    break;
                case 2:
                    npcData.Charisma++;
                    break;
            }
        }

        private static DialogLine GetDialogWithHero(NpcData npcData)
        {
            DialogLine line;
            // If hero came after completing a quest
            if (npcData.Quest != null)
            {
                // Quest completed, got any more work?
                line = ToNpcTalkDialogLine("Thank you.\nDo you have any more work for me?");
                // If main quest, change state
                if (npcData.Quest.QuestType == QuestType.MainQuest)
                {
                    Location.GetById(npcData.Quest.Location.ID).State = LocationState.Good;
                }

                npcData.Quest = null;
                LevelUpHero(npcData);
            }
            else
            {
                line = ToNpcTalkDialogLine("Got any work for me?");
            }

            var dialogOptionsForQuests = GenerateDialogOptionsForSideQuests(npcData);
            
            // Try to get next main quest, but handle case where all are completed
            var mainQuest = QuestFactory.GetNextMainQuest();
            
            var dialogOptionNoQuest = new List<DialogOption>()
            {
                new()
                {
                    Text = "I don't have any work for you today",
                    Action = () =>
                        DialogWindow.Instance.NpcTalk("I'll go find adventures on my own!", npcData.NpcName)
                }
            };

            dialogOptionsForQuests.AddRange(dialogOptionNoQuest);
            
            // Only offer main quest if hero doesn't have an active quest AND main quest is available
            if ((npcData.Quest == null || npcData.Quest.QuestState != QuestState.None) && mainQuest != null)
            {
                var dialogOptionForMainQuest = new DialogOption
                {
                    Text = mainQuest.Objective,
                    Action = () =>
                    {
                        DialogWindow.Instance.NpcTalk("Evil will be destroyed!", npcData.NpcName);
                        npcData.Quest = mainQuest;
                        var success = IsQuestSuccessfullyCompleted(npcData, mainQuest);
                        if (success)
                        {
                            var quest = npcData.Quest;
                            quest.QuestState = QuestState.Success;
                            NpcFactory.AddNpcToQueue(npcData);

                            var questCopy = new Quest
                            {
                                QuestState = quest.QuestState,
                                Location = quest.Location
                            };

                            // Action executes before next visit
                            npcData.PreVisitAction = () =>
                            {
                                questCopy.Location.State = LocationState.Good;
                                Location.GetById(questCopy.Location.ID).State = LocationState.Good;

                                var text = $"Story progress: {Location.GetStoryCompletePercent()}%\n" +
                                           $"Open map for details";

                                MapUpdatePopup.Instance.SetText(text);
                                MapUpdatePopup.Instance.gameObject.SetActive(true);
                            };
                        }
                        else
                        {
                            // Main quest failed, spawn demon and change location state
                            npcData.Quest.QuestState = QuestState.Failed;
                            npcData.Quest.Location.State = LocationState.Bad;
                            NpcFactory.AddDemonToTheQueue(npcData);
                        }

                        NpcFactory.AddHeroToLogs(npcData);
                    },
                    GetDetailsText = () => GenerateQuestDescriptionWithSuccessRate(npcData, mainQuest)
                };
                
                dialogOptionsForQuests.Add(dialogOptionForMainQuest);
            }
            
            line.ResponseOptions = dialogOptionsForQuests;
            return line;
        }

        private static List<DialogOption> GenerateDialogOptionsForSideQuests(NpcData npcData)
        {
            // Don't offer quests to heroes who already have an active quest
            if (npcData.Quest != null && npcData.Quest.QuestState == QuestState.None)
            {
                return new List<DialogOption>();
            }
            
            var quests = QuestJournal.Instance.SideQuests;
            return quests.Select(q => new DialogOption
            {
                Text = q.Objective,
                Action = () =>
                {
                    DialogWindow.Instance.NpcTalk("I'll be back when I complete the mission!", npcData.NpcName);
                    npcData.Quest = q;
                    
                    // Remove quest from journal immediately when assigned to prevent double assignment
                    if (q.QuestType == QuestType.SideQuest)
                    {
                        QuestJournal.Instance.SideQuests.Remove(q);
                    }
                    
                    NpcFactory.AddHeroToLogs(npcData);
                    var chance = CalculateSuccessChance(npcData, q);
                    var roll = UnityEngine.Random.Range(0, 100);
                    if (roll < chance)
                    {
                        npcData.Quest.QuestState = QuestState.Success;
                        // In case of success, add him back to queue
                        NpcFactory.AddNpcToQueue(npcData);
                    }
                    else
                    {
                        npcData.Quest.QuestState = QuestState.Failed;
                        NpcFactory.AddNpcToQueue(npcData);
                    }
                },
                GetDetailsText = () => GenerateQuestDescriptionWithSuccessRate(npcData, q)
            }).ToList();
        }

        private static bool IsQuestSuccessfullyCompleted(NpcData npcData, Quest quest)
        {
            var chance = CalculateSuccessChance(npcData, quest);
            npcData.ActivePotion = PotionType.None;
            var roll = UnityEngine.Random.Range(0, 100);
            return roll < chance;
        }

        private static DialogLine GetDialogWithTaxCollector(NpcData npcData)
        {
            if (npcData.IsIntro) return null;
            var line = ToNpcTalkDialogLine($"I've come to collect taxes! Pay {Instance.TaxToPay} gold");
            line.ResponseOptions = GetTaxCollectorDialogOptions(npcData, Instance.TaxToPay);
            return line;
        }


        private static List<DialogOption> GetTaxCollectorDialogOptions(NpcData npcData, int taxToPay)
        {
            DialogOption option;
            if ((Instance.Gold - taxToPay) < 0)
            {
                option = new DialogOption
                {
                    Text = "I don't have that kind of money",
                    Action = () => { DialogWindow.Instance.NpcTalk("Then your tavern is closed", npcData.NpcName); }
                };

                npcData.PostVisitAction = () => Instance.GameOver();
            }
            else
            {
                option = new DialogOption
                {
                    Text = $"Alright, here's your money. [Pay {Instance.TaxToPay} gold]",
                    Action = () =>
                    {
                        ReduceGold(Instance.TaxToPay);
                        DialogWindow.Instance.NpcTalk("The Jarl thanks you!", npcData.NpcName);
                    }
                };
            }

            return new List<DialogOption> { option };
        }


        private static DialogLine GetDialogWithCultist(NpcData npcData)
        {
            if (npcData.IsIntro) return null;
            IncreaseGold(npcData.Quest.Location.RewardToReceive);
            return null;
        }

        private static DialogLine GetDialogWithVillager(NpcData npcData)
        {
            if (npcData.Quest == null) return null;

            var quest = ToNpcTalkDialogLine(npcData.Quest.ApplicationText);
            quest.ResponseOptions = new List<DialogOption>()
            {
                new()
                {
                    Text = "I think I know someone who can help you for a small fee",
                    Action = () =>
                    {
                        QuestJournal.Instance.AddSideQuest(npcData.Quest);
                        NewQuestPopup.Instance.Init(npcData.Quest.Objective);
                        NewQuestPopup.Instance.gameObject.SetActive(true);
                        DialogWindow.Instance.NpcTalk("Excellent!", npcData.NpcName);
                    },
                    GetDetailsText = () =>
                        new QuestDescription(GenerateQuestDescription(npcData.Quest), npcData.Quest.MainSkill)
                },
                new()
                {
                    Text = "Sorry buddy, I can't help you. \nThe reward is too small.",
                    Action = () => { DialogWindow.Instance.NpcTalk("Ahh, and I had such high hopes...", npcData.NpcName); }
                }
            };

            return quest;
        }

        private void GameOver()
        {
            gameOverPanel.gameObject.SetActive(true);
        }

        private static DialogLine ToNpcTalkDialogLine(string text) => new() { Text = text };

        public static string GenerateQuestDescription(Quest quest)
        {
            var details = "Quest\n\n";
            if (quest.QuestType == QuestType.MainQuest)
                details += "Type: <color=green>Story Quest</color>\n" +
                           $"Region: {quest.Location.Name}\n";
            else
                details += "Type: <color=green>Side Quest</color>\n";

            return details +
                   $"Objective: {quest.Objective}\n" +
                   $"Experience: +{quest.Xp} Xp\n" +
                   $"Reward: {quest.Gold} gold\n" +
                   $"Difficulty: {quest.Difficulty}/10\n";
        }


        public static QuestDescription GenerateQuestDescriptionWithSuccessRate(NpcData npcData, Quest quest)
        {
            var details = GenerateQuestDescription(quest);
            var chance = (int)CalculateSuccessChance(npcData, quest);
            var color = chance switch
            {
                <= 30 => "red",
                <= 70 => "orange",
                <= 100 => "green",
                _ => "black"
            };

            details += $"\nSuccess Chance: <color={color}>{chance}%</color>";
            return new QuestDescription(details, quest.MainSkill);
        }

        private static double CalculateSuccessChance(NpcData character, Quest quest)
        {
            try
            {
                // Calculate the ratio of each characteristic to the required value
                double strengthRatio = (double)character.Strength / quest.RequiredStrength;
                double intelligenceRatio = (double)character.Intelligence / quest.RequiredIntelligence;
                double charismaRatio = (double)character.Charisma / quest.RequiredCharisma;

                // Use a linear scale to calculate the base probability
                double baseProbability = (strengthRatio + intelligenceRatio + charismaRatio) / 3;

                // Apply potion effect if any
                switch (character.ActivePotion)
                {
                    case PotionType.Strength:
                        baseProbability += 0.2;
                        break;
                    case PotionType.Intelligence:
                        baseProbability += 0.2;
                        break;
                    case PotionType.Charisma:
                        baseProbability += 0.2;
                        break;
                }

                // Clamp the probability between 0 and 1
                baseProbability = Math.Clamp(baseProbability, 0, 1);
                return baseProbability * 100;
            }
            catch
            {
                return 50d;
            }
        }

        /*private static double CalculateSuccessChance(NpcData npcData, Quest quest)
        {
            var heroLevel = npcData.Level;
            var questDifficulty = quest.Difficulty;
            // Calculate success probability
            var successProbability = 0.5 + 0.1 * (heroLevel - questDifficulty);
            // Limit success probability between 0 and 1

            if (npcData.ActivePotion != PotionType.None)
            {
                successProbability += 0.2;
            }

            return Math.Clamp(successProbability, 0.0, 1.0) * 100;
        }*/

        private static void ReduceGold(int amount)
        {
            if ((Instance.Gold - amount) < 0)
            {
                Instance.GameOver();
            }

            DOVirtual.Int(Instance.Gold, Instance.Gold - amount, 1f,
                    value =>
                    {
                        Instance.GoldTextMesh.text = value.ToString();
                        Instance.Gold = value;
                    })
                .SetEase(Ease.InOutSine)
                .SetAutoKill(true);
        }

        public static void IncreaseGold(int amount) =>
            DOVirtual.Int(Instance.Gold, Instance.Gold + amount, 1f,
                    value =>
                    {
                        Instance.GoldTextMesh.text = value.ToString();
                        Instance.Gold = value;
                    })
                .SetEase(Ease.InOutSine)
                .SetAutoKill(true);

        // Get new potion
        private static void AddPotion(PotionType type, int amount)
        {
            Potions[type] += amount;
            SetPotionValue(type, Potions[type]);
        }

        // Give potion to someone
        public static void GivePotionTo(PotionType type, NpcData npcData)
        {
            if (Potions[type] == 0) return;

            SetPotionValue(type, Potions[type] - 1);
            npcData.ActivePotion = type;
            CurrentTavernNpc.GivePotion();
        }

        private static void SetPotionValue(PotionType type, int value)
        {
            Potions[type] = value;
            var textField = type switch
            {
                PotionType.Strength => Instance.strengthPotionsText,
                PotionType.Intelligence => Instance.intelligencePotionsText,
                PotionType.Charisma => Instance.charismaPotionsText,
                PotionType.None => null,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            if (textField != null)
                textField.text = Potions[type].ToString();
        }

        private static void AddRandomPotion()
        {
            var r = UnityEngine.Random.Range(0, 2);
            switch (r)
            {
                case 0:
                    AddPotion(PotionType.Strength, 1);
                    break;
                case 1:
                    AddPotion(PotionType.Intelligence, 1);
                    break;
                case 2:
                    AddPotion(PotionType.Charisma, 1);
                    break;
            }
        }

        public void GoodEnd()
        {
            goodGamePanel.gameObject.SetActive(true);
        }

        public void BadEnd()
        {
            badGamePanel.gameObject.SetActive(true);
        }
    }

    public enum PotionType
    {
        None,
        Strength,
        Intelligence,
        Charisma,
    }
}