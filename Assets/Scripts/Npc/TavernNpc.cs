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

        public void WalkOut()
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            Player.PlayFeedbacksInReverse();
        }
    }
}