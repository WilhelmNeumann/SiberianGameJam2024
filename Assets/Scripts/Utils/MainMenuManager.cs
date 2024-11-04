using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils {
    public class MainMenuManager : MonoBehaviour {
        public void StartGame() {
            SceneManager.LoadScene("Game");
        }

        public void ExitGame() {
            Debug.Log("Game exited");
        }
        
        public void OpenCredits() {
            Debug.Log("Credits opened");
        }

        

    }
}