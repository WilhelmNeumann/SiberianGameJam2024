using System.Collections.Generic;
using Npc;
using UnityEngine;

namespace Utils {
    public class NpcManager : Utils.Singleton<NpcManager> {

        [SerializeField]
        private List<Sprite> _hats;
        
        [SerializeField]
        private List<Sprite> _heads;
        
        [SerializeField]
        private List<Sprite> _weapons;
        
        [SerializeField]
        private List<Transform> npcPrefabs;
        
        [SerializeField]
        private Vector2 spawnPosition;
        
        public Transform CreateNpc(NpcType npcType) {
            //For first 3 types of npc prefab is selected by index
            //For the last type of npc prefab is selected randomly from index 3
            var npcPrefab = npcPrefabs[npcType == NpcType.Hero ? Random.Range(3, npcPrefabs.Count) : (int) npcType];
            var npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
            var npcViewData = new NpcViewData();
            if (npcType == NpcType.Hero) {
                npcViewData.Hat = _hats[Random.Range(0, _hats.Count)];
                npcViewData.Head = _heads[Random.Range(0, _heads.Count)];
                npcViewData.Weapon = _weapons[Random.Range(0, _weapons.Count)];
                npc.GetComponent<NpcView>().SetSprites(npcViewData);
            }
            return npc;
        }

    }
}