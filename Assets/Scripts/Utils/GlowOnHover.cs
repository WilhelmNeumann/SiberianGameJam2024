using System;
using UnityEngine;

namespace Utils
{
    public class GlowOnHover: MonoBehaviour
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
    }
}