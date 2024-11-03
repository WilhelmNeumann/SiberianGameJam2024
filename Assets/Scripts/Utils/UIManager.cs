using UnityEngine;

namespace Utils
{
    public class UIManager : Singleton<UIManager>
    {
        public void OpenPanel(RectTransform panel)
        {
            panel.gameObject.SetActive(true);
        }
        
        public void ClosePanel(RectTransform panel)
        {
            panel.gameObject.SetActive(false);
        }
        
    }
}