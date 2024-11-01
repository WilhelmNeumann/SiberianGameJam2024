using System.Collections;
using Dialogs;
using MoreMountains.Feedbacks;
using UnityEngine;
using Utils;

namespace Npc
{
    public class TavernNpc : Singleton<TavernNpc>
    {
        public NpcData NpcData;
        [SerializeField] public MMF_Player Player;

        public IEnumerator EnterTheTavern()
        {
            yield return InteractWithTheInnKeeper();
        }

        public IEnumerator InteractWithTheInnKeeper()
        {
            NpcData = NpcFactory.GenerateNpc();
            yield return StartCoroutine(WalkIn());
        }

        private IEnumerator WalkIn()
        {
            Player.PlayFeedbacks();
            yield return new WaitForSeconds(Player.TotalDuration);
        }

        public void WalkOut()
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            Player.PlayFeedbacksInReverse();
        }

        public void Next()
        {
            var npc = NpcFactory.GenerateNpc();
        }
    }
}