using UnityEngine;

namespace Utils
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] public int Gold = 1000;

        public void Start()
        {
            Npc.Npc.Instance.EnterTheTavern();
        }
    }
}