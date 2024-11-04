using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public class UIManager : Singleton<UIManager> {
        
        public bool IsPanelOpen { get; private set; }

        public void OpenPanel(RectTransform panel)
        {
            if (IsPanelOpen)
            {
                return;
            }
            panel.gameObject.SetActive(true);
            
            var originalPosition =  panel.transform.position;
            panel.transform.position = originalPosition + new Vector3(0, -2500, 0);
            panel.transform.DOJump(originalPosition, 1f, 1, .5f)
                .OnKill(() => panel.transform.position = originalPosition)
                .SetAutoKill(true);
            
            IsPanelOpen = true;
        }
        
        public void ClosePanel(RectTransform panel)
        {
            var originalPosition =  panel.transform.position;
            var destination = originalPosition + new Vector3(0, -2500, 0);
            panel.transform.DOJump(destination, 1f, 1, .5f)
                .OnKill(() =>
                {
                    panel.transform.position = originalPosition;
                    panel.gameObject.SetActive(false);
                    IsPanelOpen = false;
                })
                .SetEase(Ease.OutCubic)
                .SetAutoKill(true);
        }
        
    }
}