using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour{
 // rigidbody of player
 private Rigidbody rb; 

 // variable for pickups
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
 // movement along x and y axis
 private float movementX;
 private float movementY;
//gameover?
private bool gameOver = false;
//respawn position
private Vector3 nextRespawnPos;
 // player speed
 public float speed = 0;
//no premature winning
private bool isDead = false;

 // UI text to count pickups
 public TextMeshProUGUI countText;

 // UI to display wintext
 public GameObject winTextObject;
 
 // exit portal/door 
 public GameObject exit;

 // start called before first frame update
   void Start(){
 // get and store rigidbody component to player
         rb = GetComponent<Rigidbody>();

 // intialize count to zero
         count = 0;

 // update count display
         SetCountText();

 // initially set text to false
         winTextObject.SetActive(false);

   //more animator stuff
         animator = GetComponent<Animator>();
   // lives stuff
      if (livesText != null)
         livesText.text = "Lives: " + lives;
   }
 
 // function is called when move input detected
   void OnMove(InputValue movementValue){
 // convert input value into Vector2
        Vector2 movementVector = movementValue.Get<Vector2>();

 // store x and y components into variables
        movementX = movementVector.x; 
        movementY = movementVector.y; 
   }

 // called once per frame to fix frame rate
   private void FixedUpdate(){
 // create 3D movement vector using x and y
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);

 // apply force to rigidbody to move player
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
      //pause
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
 // check if object collided with player and pickup tag
 if (other.gameObject.CompareTag("PickUp")) 
        {
 // deactivate collided object
      other.gameObject.SetActive(false);

 // increment pickup count
            count = count + 1;

 // update count display
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

 // function to update text with pickups collected
   void SetCountText() {
 // updating count text to current amount
        countText.text = "Count: " + count.ToString();

 // check if the count has reached or exceeded win condition
      if (count >= 12  && !isDead){
         //showing exit 
         if (exit != null){
            exit.SetActive(true);
         }
 // display exit text 
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "Find the Exit!";
 // destroy the enemy GameObject
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
      }
   }

   private void OnCollisionEnter(Collision collision){
      if (collision.gameObject.CompareTag("Enemy")){
 // just lose life
         LoseLife(true);
      }
   }
}