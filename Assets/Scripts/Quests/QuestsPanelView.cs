using System;
using DefaultNamespace;
using UnityEngine;

namespace Quests
{
    public class QuestsPanelView : CloseablePanel
    {
        [SerializeField] private QuestView questViewPrefab;
        [SerializeField] private Transform content;

        private void OnEnable()
        {
            QuestJournal.Instance.SideQuests.ForEach(AddQuest);
        }

        private void AddQuest(Quest quest)
        {
            var questView = Instantiate(questViewPrefab, Vector2.zero, Quaternion.identity, content);
            questView.SetQuest(quest);
        }
        
    }
}