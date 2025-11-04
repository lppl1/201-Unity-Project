using UnityEngine;

public class Door2 : MonoBehaviour
{
    public PlayerController playerController;
    public int requiredPickups = 2;

    // Update is called once per frame
    void Update()
    {
        if (playerController.stage2Count >= requiredPickups){
            gameObject.SetActive(false);
        }
    }
}
