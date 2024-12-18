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
        
        public Transform CreateNpc(NpcData npcData) {
            if (npcData.PrefabIndex == -1) {
                return CreateNewNpc(npcData);
            }
            var npc = Instantiate(npcPrefabs[npcData.PrefabIndex], spawnPosition, Quaternion.identity);
            var view = npc.GetComponent<NpcView>();
            view.SetSprites(npcData.NpcViewData, npcData.Level, npcData);
            return npc;
        }
        
        private Transform CreateNewNpc(NpcData npcData) {
            var npcType = npcData.NpcType;
            //For first 3 types of npc prefab is selected by index
            //For the last type of npc prefab is selected randomly from index 3
            var prefabIndex = npcType == NpcType.Hero ? Random.Range(3, npcPrefabs.Count) : (int) npcType;
            var npcPrefab = npcPrefabs[prefabIndex];
            var npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
            var npcViewData = new NpcViewData();
            if (npcType == NpcType.Hero) {
                npcViewData.Hat = _hats[Random.Range(0, _hats.Count)];
                npcViewData.Head = _heads[Random.Range(0, _heads.Count)];
                npcViewData.Weapon = _weapons[Random.Range(0, _weapons.Count)];
                npc.GetComponent<NpcView>().SetSprites(npcViewData, npcData.Level, npcData);
            }
            npcData.NpcViewData = npcViewData;
            npcData.PrefabIndex = prefabIndex;
            return npc;
        }

    }
}