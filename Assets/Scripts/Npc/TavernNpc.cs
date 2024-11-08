using System;
using System.Collections;
using System.Linq;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;

namespace Npc
{
    public class TavernNpc : MonoBehaviour
    {
        public NpcData NpcData;
        [SerializeField] public MMF_Player Player;
        [SerializeField] public Transform LevelText;
        [SerializeField] public Transform view;
        [SerializeField] public Material potionEffectMaterial;

        private Material _defaultMat;

        public void Start()
        {
            
        }

        public IEnumerator WalkIn()
        {
            Player.PlayFeedbacks();
            yield return new WaitForSeconds(Player.TotalDuration);
        }

        public IEnumerator WalkOut()
        {
            TurnAround();
            Player.Revert();
            Player.PlayFeedbacks();
            yield return new WaitForSeconds(Player.TotalDuration);
            Destroy(gameObject);
        }

        private void TurnAround()
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);

            var view = GetComponent<NpcView>();
            if (view == null) return;
            var textTransform = view.GetTextTransform();
            if (textTransform == null) return;
            textTransform.localScale = new Vector2(textTransform.localScale.x, textTransform.localScale.y);
        }

        public void GivePotion()
        {
            var allSprites = view.GetComponentsInChildren<SpriteRenderer>().ToList();
            allSprites.ForEach(x => x.material = potionEffectMaterial);
        }
    }
}