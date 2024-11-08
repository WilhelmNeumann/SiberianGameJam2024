using System.Collections;
using DG.Tweening;
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
            if (currentNpc is { NpcType: NpcType.Hero, ActivePotion: PotionType.None })
            {
                StartCoroutine(GivePotion(currentNpc));
            }
        }

        private IEnumerator GivePotion(NpcData npcData)
        {
            transform.DOPunchPosition(new Vector2(320, -232), .5f, 5, .5f);
            transform.DOPunchScale(new Vector2(2, 2), .5f, 5, .5f);
            yield return new WaitForSeconds(0.2f);
            GameManager.GivePotionTo(PotionType, npcData);
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