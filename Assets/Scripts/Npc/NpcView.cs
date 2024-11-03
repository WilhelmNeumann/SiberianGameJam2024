using UnityEngine;

namespace Npc {
    public class NpcView : MonoBehaviour {

        [SerializeField]
        private SpriteRenderer hat;
        [SerializeField]
        private SpriteRenderer body;
        [SerializeField]
        private SpriteRenderer head;
        [SerializeField]
        private SpriteRenderer leftHand;
        [SerializeField]
        private SpriteRenderer rightHand;
        [SerializeField]
        private SpriteRenderer weapon;
        
        public void SetSprites(NpcViewData data) {
            hat.sprite = data.Hat;
            body.sprite = data.Body;
            head.sprite = data.Head;
            leftHand.sprite = data.LeftHand;
            rightHand.sprite = data.RightHand;
            weapon.sprite = data.Weapon;
        }


    }
}