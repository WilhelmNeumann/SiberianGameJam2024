using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Utils
{
    public class ClickableObject : MonoBehaviour
    {
        [SerializeField] private Material glowOnHoverMaterial;
        [SerializeField] private UnityEvent onMouseDownEvent;

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

        public void OnMouseDown()
        {
            onMouseDownEvent?.Invoke();
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