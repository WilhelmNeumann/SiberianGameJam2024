using UnityEngine;
using Utils;

namespace DefaultNamespace
{
    public class CloseablePanel : MonoBehaviour
    {
        
        public void ClosePanel() {
            UIManager.Instance.ClosePanel(transform.parent as RectTransform);
        }
        
    }
}