using System.Collections;
using UnityEngine;

namespace Utils
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] public int Gold = 1000;

        [SerializeField] public Npc.Npc Npc;

        public IEnumerator Start()
        {
            while (true)
            {
                Npc.WalkIn();
                yield return new WaitForSeconds(5f);
                Npc.WalkOut();
                yield return new WaitForSeconds(5f);
            }
        }
    }
}