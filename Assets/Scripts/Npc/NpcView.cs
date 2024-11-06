using System;
using TMPro;
using UnityEngine;

namespace Npc
{
    public class NpcView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer hat;
        [SerializeField] private SpriteRenderer head;
        [SerializeField] private TMP_Text lvlText;
        [SerializeField] private GameObject details;
        private NpcData _npcData;
        
        public void SetSprites(NpcViewData data, int lvl, NpcData npcData)
        {
            hat.sprite = data.Hat;
            head.sprite = data.Head;
            _npcData = npcData;
            lvlText.text = $"LVL: {npcData.Level}" +
                           $"\nSTRENGTH: {npcData.Strength}\n" +
                           $"CHARISMA: {npcData.Charisma}\n" +
                           $"INTEL: {npcData.Intelligence}";
        }

        public Transform GetTextTransform()
        {
            return lvlText == null ? null : lvlText.transform;
        }

        public void OnMouseEnter()
        {
            details?.SetActive(true);
        }

        public void OnMouseExit()
        {
            details?.SetActive(false);
        }
    }
}