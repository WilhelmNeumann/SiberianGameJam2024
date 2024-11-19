using System;
using Quests;
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
        [SerializeField] public RawImage Icon;
        [SerializeField] public Sprite[] Icons;

        private Func<QuestDescription> _getDetailsText;

        public Action Action;

        public void OnClick()
        {
            Action?.Invoke();
            DialogWindow.Instance.ContinueDialog();
        }

        public void Init(string optionText, Action action, Func<QuestDescription> getDetailsText)
        {
            Action = action;
            UiText.text = optionText;
            if (getDetailsText == null) return;
            _getDetailsText = getDetailsText;
            DetailsText.text = getDetailsText.Invoke().Description;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Image.material = Glow;

            if (_getDetailsText != null)
            {
                DetailsText.text = _getDetailsText.Invoke().Description;
                Icon.texture = GetIcon(_getDetailsText.Invoke().MainSkill).texture;
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

        private Sprite GetIcon(MainSkill skill)
        {
            switch (skill)
            {
                case MainSkill.Strength:
                    return Icons[0];
                case MainSkill.Intelligence:
                    return Icons[1];
                case MainSkill.Charisma:
                    return Icons[2];
            }
            return Icons[0];
        }
        
    }
}