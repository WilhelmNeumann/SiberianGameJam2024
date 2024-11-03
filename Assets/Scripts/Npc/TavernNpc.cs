using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Npc
{
    public class TavernNpc : MonoBehaviour
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
            Destroy(gameObject);
        }

        private void TurnAround()
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }
}