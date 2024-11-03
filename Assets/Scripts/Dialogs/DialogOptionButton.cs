using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dialogs
{
    public class DialogOptionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] public Text UiText;
        [SerializeField] public Image Image;
        [SerializeField] public Material Glow;
        [SerializeField] public GameObject Details;
        [SerializeField] public Text DetailsText;
        
        public Action Action;

        public void OnClick()
        {
            Action?.Invoke();
            DialogWindow.Instance.ContinueDialog();
        }

        public void Init(string optionText, Action action, string detailsText)
        {
            Action = action;
            UiText.text = optionText;
            DetailsText.text = detailsText;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Image.material = Glow;
            if (DetailsText.text.Length > 0)
            {
                Details.SetActive(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Image.material = null;
            Details.SetActive(false);
        }
    }
}