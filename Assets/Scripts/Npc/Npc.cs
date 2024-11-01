using MoreMountains.Feedbacks;
using UnityEngine;

namespace Npc
{
    public class Npc : MonoBehaviour
    {
        [SerializeField] public NpcType Type;
        [SerializeField] public string NpcName;
        [SerializeField] public MMF_Player Player;

        public void WalkIn()
        {
            Player.PlayFeedbacks();
        }

        public void WalkOut()
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            Player.PlayFeedbacksInReverse();
        }
    }
}