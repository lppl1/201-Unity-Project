using UnityEngine;
using UnityEngine.SceneManagement;

public class exit : MonoBehaviour{
    public string nextSceneName = "Level2"; //name of next level
    private bool isTransitioning = false; 
    
    void OnTriggerEnter(Collider other){
        // check if player touched exit
        if (other.CompareTag("Player") && !isTransitioning){
            isTransitioning = true;
            LoadNextLevel();
        }
    }
     void LoadNextLevel(){
        // Play sound if available but no sound for now
        /* if (exitSound != null)
        {
            AudioSource.PlayClipAtPoint(exitSound, transform.position);
        }
        */

        // Changing the time scale
        Time.timeScale = 1;
        
        // loading the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}