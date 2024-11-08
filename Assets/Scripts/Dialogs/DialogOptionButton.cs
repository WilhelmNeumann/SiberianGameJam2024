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

        private Func<string> _getDetailsText;

        public Action Action;

        public void OnClick()
        {
            Action?.Invoke();
            DialogWindow.Instance.ContinueDialog();
        }

        public void Init(string optionText, Action action, Func<string> getDetailsText)
        {
            Action = action;
            UiText.text = optionText;
            if (getDetailsText == null) return;
            _getDetailsText = getDetailsText;
            DetailsText.text = getDetailsText.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Image.material = Glow;

            if (_getDetailsText != null)
            {
                DetailsText.text = _getDetailsText.Invoke();
            }
            
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