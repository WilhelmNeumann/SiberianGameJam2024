using Quests;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class MapView : MonoBehaviour
    {
        [SerializeField] private Text _text;

        private void OnEnable()
        {
            _text.text = $"Story completed: {Location.GetStoryCompletePercent()}%\n" +
                         $"Captured by cultists: {Location.GetFailedCount()}/10\n" +
                         $"Protected from cultists: {Location.GetGoodCount()}/10";
        }
    }
}