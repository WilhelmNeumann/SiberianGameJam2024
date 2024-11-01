using System.Collections;
using Dialogs;
using MoreMountains.Feedbacks;
using UnityEngine;
using Utils;

namespace Npc
{
    public class Npc : Singleton<Npc>
    {
        private NpcData _npcData;
        [SerializeField] public MMF_Player Player;

        public void EnterTheTavern()
        {
            StartCoroutine(InteractWithTheInnKeeper());
        }

        public IEnumerator InteractWithTheInnKeeper()
        {
            _npcData = NpcFactory.GenerateNpc();
            yield return StartCoroutine(WalkIn());
            DialogWindow.Instance.NpcTalk(_npcData.GreetingsText, _npcData.NpcName);
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