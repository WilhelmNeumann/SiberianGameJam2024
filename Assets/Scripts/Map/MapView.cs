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
            _text.text = $"Сюжет пройден на {Location.GetStoryCompletePercent()}%\n" +
                         $"Захвачено культистами: {Location.GetFailedCount()}/10\n" +
                         $"Защищено от культистов: {Location.GetGoodCount()}/10";
        }
    }
}