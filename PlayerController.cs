using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour{
 // Rigidbody of the player.
 private Rigidbody rb; 

 // Variable to keep track of collected "PickUp" objects.
 public int count;
//troubleshooting
public int stage1Count = 0;
public int stage2Count = 0;
public int stage3Count = 0;
public int stage4Count = 0;
//animation for player
public Animator animator;
//jump + dash
public float jump = 5f;
public float dash = 50f;
//audio stuff
public AudioClip pickupSound;
//time
public float timeLeft = 100f;
public TextMeshProUGUI timerText;
//the lives system
public int lives = 3;
public Transform respawn;
//livetext UI
 public TextMeshProUGUI livesText;
 // Movement along X and Y axes.
 private float movementX;
 private float movementY;
//gameover?
private bool gameOver = false;
//respawn position
private Vector3 nextRespawnPos;
 // Speed at which the player moves.
 public float speed = 0;
//no premature winning
private bool isDead = false;

 // UI text component to display count of "PickUp" objects collected.
 public TextMeshProUGUI countText;

 // UI object to display winning text.
 public GameObject winTextObject;

 // Start is called before the first frame update.
 void Start(){
 // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();

 // Initialize count to zero.
        count = 0;

 // Update the count display.
        SetCountText();

 // Initially set the win text to be inactive.
        winTextObject.SetActive(false);

   //more animator stuff
         animator = GetComponent<Animator>();
   // lives stuff
   if (livesText != null)
        livesText.text = "Lives: " + lives;
}
 
 // This function is called when a move input is detected.
 void OnMove(InputValue movementValue)
    {
 // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

 // Store the X and Y components of the movement.
        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }

 // FixedUpdate is called once per fixed frame-rate frame.
 private void FixedUpdate() 
    {
 // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);

 // Apply force to the Rigidbody to move the player.
        rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, movement.z * speed); 

         float movementMagnitude = new Vector2(movementX, movementY).magnitude;
         //safety check
         if (animator != null)
            animator.SetFloat("Speed", movementMagnitude);

         if (movementX != 0 || movementY != 0) {
            transform.forward = movement.normalized;
         }
       
    }
   //adding jump, dash, and pause
   void OnJump(InputValue value){
    rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
   }
   void Update(){
      if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) {
         Time.timeScale = Time.timeScale == 0 ? 1 : 0;
      }
      if (Keyboard.current != null && Keyboard.current.leftShiftKey.wasPressedThisFrame) {
         rb.AddForce(new Vector3(movementX, 0, movementY).normalized * dash, ForceMode.Impulse);
      }
      //countdown
      if (!gameOver && Time.timeScale > 0) {
        timeLeft -= Time.deltaTime;
        if (timerText != null)
            timerText.text = "Time: " + timeLeft.ToString("0");

        if (timeLeft <= 0){
            LoseLife();
        }
      }
      //restart button
      if (gameOver && Keyboard.current.rKey.wasPressedThisFrame){
         Time.timeScale = 1;
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      }
   }
   public void LoseLife(bool hitEnemy = false){
      lives--;
      isDead = true;
      if (livesText != null){
        livesText.text = "Lives: " + lives;
      }
      if (lives <= 0){
         GameOver();
         return;
      }
      //auto respawn
      transform.position = respawn.position;
      rb.linearVelocity = Vector3.zero;
      isDead = false;
   }
   //game over function
   private void GameOver() {
      gameOver = true;
      winTextObject.SetActive(true);
      winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!\nPress R to Restart";
      Time.timeScale = 0; // Pause
   } 
 
 void OnTriggerEnter(Collider other) {
 // Check if the object the player collided with has the "PickUp" tag.
 if (other.gameObject.CompareTag("PickUp")) 
        {
 // Deactivate the collided object (making it disappear).
      other.gameObject.SetActive(false);

 // Increment the count of "PickUp" objects collected.
            count = count + 1;

 // Update the count display.
            SetCountText();
        }
//extra if/else for door managament & pickup music
   if (other.CompareTag("Stage1Pickup")){
      AudioSource audio = GetComponent<AudioSource>();
        if (pickupSound != null && audio != null){
            audio.PlayOneShot(pickupSound, 1.0f);
        }
      other.gameObject.SetActive(false);
      stage1Count++;
      count++;
      SetCountText();
   }else if (other.CompareTag("Stage2Pickup")){
      AudioSource audio = GetComponent<AudioSource>();
        if (pickupSound != null && audio != null){
            audio.PlayOneShot(pickupSound, 1.0f);
        }
      other.gameObject.SetActive(false);
      stage2Count++;
      count++;
      SetCountText();
   }else if (other.CompareTag("Stage3Pickup")){
      AudioSource audio = GetComponent<AudioSource>();
        if (pickupSound != null && audio != null){
            audio.PlayOneShot(pickupSound, 1.0f);
        }
      other.gameObject.SetActive(false);
      stage3Count++;
      count++;
      SetCountText();
   }else if (other.CompareTag("Stage4Pickup")){
      AudioSource audio = GetComponent<AudioSource>();
        if (pickupSound != null && audio != null){
            audio.PlayOneShot(pickupSound, 1.0f);
        }
      other.gameObject.SetActive(false);
      stage4Count++;
      count++;
      SetCountText();
   }
   //killfloor
   if (other.CompareTag("KillFloor")) {
      LoseLife(); 
   }
 }

 // Function to update the displayed count of "PickUp" objects collected.
 void SetCountText() 
    {
 // Update the count text with the current count.
        countText.text = "Count: " + count.ToString();

 // Check if the count has reached or exceeded the win condition.
 if (count >= 12  && !isDead)
        {
 // Display the win text.
            gameOver = true;
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Win!\nPress R to Restart";
 // Destroy the enemy GameObject.
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }

   private void OnCollisionEnter(Collision collision){
   if (collision.gameObject.CompareTag("Enemy")){
 // Destroy the current object 
 //not now because respawn
 // Update the winText to display "You Lose!"
      LoseLife(true);
   }
   }
}