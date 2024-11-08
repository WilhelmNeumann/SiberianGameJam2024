using System.Linq;
using DefaultNamespace;
using Quests;
using UnityEngine;
using Utils;

namespace Map {
    public class MapHandler : CloseablePanel {
        
        [SerializeField] 
        private MapRegion[] regions;
        
        [SerializeField] 
        private MapRegion[] goodRegions;
        
        private void OnEnable() {
            foreach (var region in regions) {
                region.gameObject.SetActive(false);
            }
            foreach (var region in goodRegions) {
                region.gameObject.SetActive(false);
            }
            //For every Location.Locations set the region active if locationstate is bad
            foreach (var location in Location.Locations) {
                if (location.State == LocationState.Bad) {
                    regions[location.ID - 1].gameObject.SetActive(true);
                } 
                if (location.State == LocationState.Good) {
                    goodRegions[location.ID - 1].gameObject.SetActive(true);
                } 
            }
        }
    }
}