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
            yield return TavernNpc.Instance.EnterTheTavern();
            var npcData = TavernNpc.Instance.NpcData;
            DialogWindow.Instance.NpcTalk(npcData.GreetingsText, npcData.NpcName);
        }

        public void Continue()
        {

        }
    }
}
