using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class GlowOnHover : MonoBehaviour
    {
        [SerializeField] private Material glowOnHoverMaterial;

        private Material originalMaterial;

        private void SetMaterial(Material mat)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.material = mat;
                return;
            }
            
            var image = GetComponent<Image>();
            image.material = mat;
        }

        private Material GetMaterial()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            return spriteRenderer != null
                ? spriteRenderer.material
                : GetComponent<Image>().material;
        }

        private void Start()
        {
            originalMaterial = GetMaterial();
        }

        public void OnMouseEnter()
        {
            SetMaterial(glowOnHoverMaterial);
        }

        public void OnMouseExit()
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