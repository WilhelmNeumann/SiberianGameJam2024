using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;
using Utils;

namespace Ui
{
    public class NewQuestPopup : Singleton<NewQuestPopup>
    {
        [SerializeField] MMF_Player _player;

        private new void Awake()
        {
            base.Awake();
            gameObject.SetActive(false);
            GetComponent<Canvas>().enabled = true;
        }
        
        private void OnEnable()
        {
            StartCoroutine(ShowAndHidePopup());
        }

        private IEnumerator ShowAndHidePopup()
        {
            _player.PlayFeedbacks();
            yield return new WaitForSeconds(_player.TotalDuration);
            gameObject.SetActive(false);
        }
    }
}