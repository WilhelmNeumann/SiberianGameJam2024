using System.Collections;
using Dialogs;
using Npc;
using UnityEngine;

namespace Utils
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] public int Gold = 1000;

        public IEnumerator Start()
        {
            DialogWindow.Instance.OnContinue += Continue;
            
            var npcData = NpcFactory.GenerateNpc();
            yield return TavernNpc.Instance.WalkIn();
            DialogWindow.Instance.NpcTalk(npcData.GreetingsText[0], npcData.NpcName);
            
        }

        public void Continue()
        {
            
        }
    }
}
