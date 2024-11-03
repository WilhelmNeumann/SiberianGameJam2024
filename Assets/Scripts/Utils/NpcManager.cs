using System.Collections.Generic;
using Npc;
using UnityEngine;

namespace Utils {
    public class NpcManager : Utils.Singleton<NpcManager> {

        [SerializeField]
        private List<Sprite> _hats;
        
        [SerializeField]
        private List<Sprite> _bodies;
        
        [SerializeField]
        private List<Sprite> _heads;
        
        [SerializeField]
        private List<Sprite> _leftHands;
        
        [SerializeField]
        private List<Sprite> _rightHands;
        
        [SerializeField]
        private List<Sprite> _weapons;
        
        [SerializeField]
        private Transform npcPrefab;
        
        [SerializeField]
        private Vector2 spawnPosition;
        
        public Transform CreateNpc(NpcType npcType) {
            var npc = GameObject.Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
            var npcViewData = new NpcViewData();
            npcViewData.Hat = _hats[Random.Range(0, _hats.Count)];
            npcViewData.Body = _bodies[Random.Range(0, _bodies.Count)];
            npcViewData.Head = _heads[Random.Range(0, _heads.Count)];
            npcViewData.LeftHand = _leftHands[Random.Range(0, _leftHands.Count)];
            npcViewData.RightHand = _rightHands[Random.Range(0, _rightHands.Count)];
            
            npcViewData.Weapon = npcType is NpcType.Villager ? null : _weapons[Random.Range(0, _weapons.Count)];
            npc.GetComponent<NpcView>().SetSprites(npcViewData);
            return npc;
        }

    }
}