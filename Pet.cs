using UnityEngine;

public class Pet : MonoBehaviour{
    public Transform player;
    public float speed = 5f;
    public float height = 1.5f;

    // Update is called once per frame
    void Update(){
        if (player != null){
            Vector3 targetPosition = player.position + new Vector3(0, height, 0);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
