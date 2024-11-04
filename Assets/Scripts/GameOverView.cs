using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace DefaultNamespace {
    public class GameOverView : MonoBehaviour {

        [SerializeField]
        private Transform restartButton;

        [SerializeField]
        private float speed = 1f;

        [SerializeField]
        private Transform gameOverBG;

        [SerializeField]
        private float xStop;

        private void Awake() {
            restartButton.gameObject.SetActive(false);
            MoveBG();
        }

        private void MoveBG() {
            var targetPosition = new Vector3(xStop, gameOverBG.position.y, gameOverBG.position.z);
            var originalPosition =  gameOverBG.transform.position;
            var destination = new Vector3(xStop, originalPosition.y, originalPosition.z);
            gameOverBG.transform.DOJump(destination, 1f, 1, 7f)
                .OnKill(() =>
                {
                    restartButton.gameObject.SetActive(true);
                })
                .SetEase(Ease.Linear)
                .SetAutoKill(true);
        }
        
        public void RestartGame() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}