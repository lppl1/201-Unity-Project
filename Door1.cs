using UnityEngine;

public class Door1 : MonoBehaviour{
    public PlayerController playerController;
    public int requiredPickups = 2;

    // Update is called once per frame
    void Update(){
        if (playerController.stage1Count >= requiredPickups){
            gameObject.SetActive(false);
        }
    }
}