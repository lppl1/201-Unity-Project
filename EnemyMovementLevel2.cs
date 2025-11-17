using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementLevel2 : MonoBehaviour{
 // reference to player transform
 public Transform player;

 // reference to NavMeshAgent for pathfinding
 private NavMeshAgent navMeshAgent;

// fix to ai enemy zones 
private Vector3 startPos;
public float maxDistance = 15f;

 // start before first frame update
   void Start(){
 // get and store navmesh component
        navMeshAgent = GetComponent<NavMeshAgent>();
        startPos = transform.position;
   }

 // Update is called once per frame.
   void Update(){
      if (player != null){ 
      float pDFromStart = Vector3.Distance(player.position, startPos);
 // if there's a reference to the player   
 // set enemy's destination to player's current position
         //ok fix to rotation
         if (pDFromStart < maxDistance) {
            navMeshAgent.SetDestination(player.position);
         // face player
            Vector3 direction = (player.position - transform.position);
            direction.y = 0;
            if (direction != Vector3.zero){
               Quaternion targetRotation = Quaternion.LookRotation(direction);
               transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation * Quaternion.AngleAxis(90, Vector3.up), 180f * Time.deltaTime);
            }
         } else {
            navMeshAgent.ResetPath();
         }
      }
   }
}