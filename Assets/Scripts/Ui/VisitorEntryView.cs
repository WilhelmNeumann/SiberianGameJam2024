using Npc;
using TMPro;
using UnityEngine;

namespace Ui {
    public class VisitorEntryView : MonoBehaviour {
        [SerializeField]
        private TMP_Text textDescription;
        
        public void SetVisitor(NpcData npc) {
            textDescription.text = GenerateQuestDescription(npc);
        }
        
        private static string GenerateQuestDescription(NpcData npc)
        {
            //NPCName level doing quest
            return $"{npc.NpcName}." +
                   $" \nLevel: {npc.Level}" +
                   $" \nStrength: {npc.Strength}" +
                   $" \nIntelligence: {npc.Intelligence}" +
                   $" \nCharisma: {npc.Charisma}";
        }
        

    }
}