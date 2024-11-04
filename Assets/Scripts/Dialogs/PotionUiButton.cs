using Npc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace Dialogs
{
    public class PotionUiButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] public GameObject Details;
        [SerializeField] public PotionType PotionType;
        [SerializeField] public Material Glow;

        public void OnClick()
        {
            var currentNpc = GameManager.CurrentNpcData;
            if (currentNpc is { NpcType: NpcType.Hero } && currentNpc.ActivePotion == PotionType.None)
            {
                GameManager.GivePotionTo(PotionType, currentNpc);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Details.GetComponent<Image>().material = Glow;
            Details.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Details.GetComponent<Image>().material = null;
            Details.gameObject.SetActive(false);
        }
    }
}