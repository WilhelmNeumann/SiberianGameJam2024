using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace Ui
{
    public class NewQuestPopup : Singleton<NewQuestPopup>
    {
        [SerializeField] private MMF_Player _player;
        [SerializeField] private Text _text;
        [SerializeField] private GlowOnHover _board;

        private new void Awake()
        {
            base.Awake();
            gameObject.SetActive(false);
            GetComponent<Canvas>().enabled = true;
        }

        public void Init(string text)
        {
            _text.text = text;
        }

        private void OnEnable()
        {
            StartCoroutine(ShowAndHidePopup());
        }

        private IEnumerator ShowAndHidePopup()
        {
            _player.PlayFeedbacks();
            yield return new WaitForSeconds(_player.TotalDuration / 2);
            _board.Blink(4);
            yield return new WaitForSeconds(_player.TotalDuration / 2);
            gameObject.SetActive(false);
        }
    }
}