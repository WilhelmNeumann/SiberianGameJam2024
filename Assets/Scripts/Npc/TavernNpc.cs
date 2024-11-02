using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using Dialogs;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;

namespace Npc
{
    public class TavernNpc : Utils.Singleton<TavernNpc>
    {
        public NpcData NpcData;
        [SerializeField] public MMF_Player Player;
        
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
            Player.Revert();
            TurnAround();
        }

        private void TurnAround()
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }
}