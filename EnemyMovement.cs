using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
 // Reference to the player's transform.
 public Transform player;

 // Reference to the NavMeshAgent component for pathfinding.
 private NavMeshAgent navMeshAgent;

// fix to ai enemy zones 
private Vector3 startPos;
public float maxDistance = 15f;

 // Start is called before the first frame update.
 void Start()
    {
 // Get and store the NavMeshAgent component attached to this object.
        navMeshAgent = GetComponent<NavMeshAgent>();
        startPos = transform.position;
    }

 // Update is called once per frame.
 void Update()
    {
      if (player != null){ 
      float pDFromStart = Vector3.Distance(player.position, startPos);
 // If there's a reference to the player...   
 // Set the enemy's destination to the player's current position.
         //maybe quick fix?
         if (pDFromStart < maxDistance) {
            navMeshAgent.SetDestination(player.position);
         } else {
            navMeshAgent.ResetPath();
         }
      }
    }
}