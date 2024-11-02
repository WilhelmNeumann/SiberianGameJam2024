using System;
using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public class GlowOnHover : MonoBehaviour
    {
        [SerializeField] private Material glowOnHoverMaterial;

        private Material originalMaterial;

        private void Start()
        {
            originalMaterial = GetComponent<SpriteRenderer>().material;
        }

        public void OnMouseEnter()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.material = glowOnHoverMaterial;
        }

        public void OnMouseExit()
        {
            GetComponent<SpriteRenderer>().material = originalMaterial;
        }

        public void Blink(int times)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();

            DOVirtual.Int(0, times * 2, 1f,
                    value => { spriteRenderer.material = value % 2 == 0 ? glowOnHoverMaterial : originalMaterial; })
                .SetEase(Ease.Linear)
                .OnComplete(() => spriteRenderer.material = originalMaterial);
        }
    }
}