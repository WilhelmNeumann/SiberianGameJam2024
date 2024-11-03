using UnityEngine;

namespace Utils
{
    public class UIManager : Singleton<UIManager>
    {
        public bool IsPanelOpen { get; private set; }

        public void OpenPanel(RectTransform panel)
        {
            if (IsPanelOpen)
            {
                return;
            }
            panel.gameObject.SetActive(true);
            IsPanelOpen = true;
        }
        
        public void ClosePanel(RectTransform panel)
        {
            panel.gameObject.SetActive(false);
            IsPanelOpen = false;
        }
        
    }
}