using System;
using DefaultNamespace;
using Npc;
using Ui;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quests
{
    public class JournalPanelView : CloseablePanel
    {
        [SerializeField] private VisitorEntryView visitorEntryViewPrefab;
        [SerializeField] private Transform content;

        private void OnEnable()
        {
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
            
            NpcFactory.GetAllHerosOnQuest().ForEach(AddNPC);
        }

        private void AddNPC(NpcData npc)
        {
            var journalView = Instantiate(visitorEntryViewPrefab, Vector2.zero, Quaternion.identity, content);
            journalView.SetVisitor(npc);
        }
        
    }
}