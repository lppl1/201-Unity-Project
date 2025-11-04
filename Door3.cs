using UnityEngine;

public class Door3 : MonoBehaviour
{
    public PlayerController playerController;
    public int requiredPickups = 3;

    // Update is called once per frame
    void Update()
    {
        if (playerController.stage3Count >= requiredPickups){
            gameObject.SetActive(false);
        }
    }
}