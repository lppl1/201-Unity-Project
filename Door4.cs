using UnityEngine;

public class Door4 : MonoBehaviour{
    public PlayerController playerController;
    public int requiredPickups = 3;

    // Update is called once per frame
    void Update(){
        if (playerController.stage4Count >= requiredPickups){
            gameObject.SetActive(false);
        }
    }
}