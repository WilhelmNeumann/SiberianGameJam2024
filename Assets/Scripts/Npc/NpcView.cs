using TMPro;
using UnityEngine;

namespace Npc {
    public class NpcView : MonoBehaviour {

        [SerializeField]
        private SpriteRenderer hat;
        [SerializeField]
        private SpriteRenderer head;
        [SerializeField]
        private TMP_Text lvlText;
        
        public void SetSprites(NpcViewData data, int lvl) {
            hat.sprite = data.Hat;
            head.sprite = data.Head;
            lvlText.text = lvl.ToString();
        }

        public Transform GetTextTransform() => lvlText.transform;
    }
}