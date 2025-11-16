using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class NewPlayer : MonoBehaviour{
    // rigidbody
    private Rigidbody rb;

    // animator
    public Animator animator;

    // movement
    private float movementX;
    private float movementY;
    public float speed = 20f;

    // jump and dash
    public float jump = 15f;
    public float dash = 250f;

    // wintext
    public GameObject winTextObject;

    // gameover again
    private bool gameOver = false;

    void Start(){
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // hide win text at start
        if (winTextObject != null)
            winTextObject.SetActive(false);
        
        // lock cursor for fp camera
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnMove(InputValue movementValue){
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void FixedUpdate(){
        //had to research and edit code just to get movement with camera to work
        Transform cam = Camera.main.transform;
        Vector3 forward = cam.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = cam.right;
        right.y = 0;
        right.Normalize();
        // movement relative to camera
        Vector3 movement = forward * movementY + right * movementX;

        // scale by speed 
        rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, movement.z * speed);

        if (animator != null)
            animator.SetFloat("Speed", new Vector2(movementX, movementY).magnitude);

        // smooth rotation 
        if (movement.magnitude > 0.001f && movementY >= 0){
        Vector3 flatMove = new Vector3(movement.x, 0f, movement.z);
            Quaternion targetRotation = Quaternion.LookRotation(flatMove);
            float rotationSpeed = 360f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    void OnJump(InputValue value){
        rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
    }

    void Update(){
        // updated dash
        //same camera definition as before
        Transform cam = Camera.main.transform;
        if (Keyboard.current != null && Keyboard.current.leftShiftKey.wasPressedThisFrame){
            Vector3 dashDir = (movementX != 0 || movementY != 0)
            //multiline just so it's not as hard to read
            ? new Vector3(cam.forward.x * movementY + cam.right.x * movementX, 0, cam.forward.z * movementY + cam.right.z * movementX).normalized
            : transform.forward;
            //same adforce as before
            rb.AddForce(dashDir * dash, ForceMode.Impulse);
        }

        // restart scene if game over
        if (gameOver && Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame){
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        // FIXED: toggle cursor lock with ESC (using New Input System)
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame){
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void OnCollisionEnter(Collision otherCollision){
        if (otherCollision.gameObject.CompareTag("NewSceneEnemy"))
            LoseGame();
    }

    private void LoseGame(){
        gameOver = true;

        if (winTextObject != null){
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!\nPress R to Restart";
        }

        Time.timeScale = 0; // used to pause
        Cursor.lockState = CursorLockMode.None; // unlock cursor on game over
    }

    public void WinGame(){
        gameOver = true;

        if (winTextObject != null){
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Win!\nPress R to Restart";
        }

        Time.timeScale = 0; // used to pause
        Cursor.lockState = CursorLockMode.None; // unlock cursor on win
    }
}