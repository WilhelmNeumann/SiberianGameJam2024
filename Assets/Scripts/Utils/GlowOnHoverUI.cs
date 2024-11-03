using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utils
{
    public class GlowOnHoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Material glowOnHoverMaterial;

        private Material originalMaterial;

        private void SetMaterial(Material mat)
        {
            var rawImage = GetComponent<RawImage>();
            if (rawImage != null)
            {
                rawImage.material = mat;
            }
        }

        private Material GetMaterial()
        {
            var rawImage = GetComponent<RawImage>();
            return rawImage != null ? rawImage.material : null;
        }

        private void Start()
        {
            originalMaterial = GetMaterial();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetMaterial(glowOnHoverMaterial);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetMaterial(originalMaterial);
        }

        public void Blink(int times)
        {
            DOVirtual.Int(0, times * 2, 1f,
                    value =>
                    {
                        SetMaterial(value % 2 == 0 ? glowOnHoverMaterial : originalMaterial);
                    })
                .SetEase(Ease.Linear)
                .OnComplete(() => SetMaterial(originalMaterial));
        }
    }
}