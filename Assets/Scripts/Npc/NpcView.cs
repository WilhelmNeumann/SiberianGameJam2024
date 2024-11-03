using UnityEngine;

namespace Npc {
    public class NpcView : MonoBehaviour {

        [SerializeField]
        private SpriteRenderer hat;
        [SerializeField]
        private SpriteRenderer head;
        
        public void SetSprites(NpcViewData data) {
            hat.sprite = data.Hat;
            head.sprite = data.Head;
        }


    }
}