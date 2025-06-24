using UnityEngine;
using TMPro; // Importing TextMeshPro namespace for text display
using System.Collections; // Importing System.Collections namespace for IEnumerator

public class PlayerController : MonoBehaviour
{
    private float speed = 10f;
    private float zBound = 10f;
    public AudioSource collectSound; // Reference to the AudioSource component for sound effects
    public TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI component to display the score
    int score = 0; // Variable to keep track of the score
    Scoring scoring; // Reference to the Scoring script to keep track of the score
    bool powerupActive = false; // Variable to check if a powerup is active
    private float powerupDuration = 5f; // Duration for which the powerup is active
    UIManager uiManager; // Reference to the UIManager to handle game state
    private float powerupTimer = 0f; // Timer to track the duration of the powerup
    public GameObject powerUpIndicator; // Reference to the powerup indicator GameObject
    public TextMeshProUGUI powerupText; // Reference to the TextMeshProUGUI component to display powerup status
    public AudioSource badItemSound; // Reference to the AudioSource component for bad item sound effects
    public ParticleSystem badItemEffect; // Reference to the ParticleSystem for powerup effect
    public int lifes = 5;
    public TextMeshProUGUI lifeText; // Reference to the TextMeshProUGUI component to display lives
    GameManager gameManager; // Reference to the GameManager to manage game items
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = "Score: 0"; // Initialize the score text to "Score: 0"
        powerupText.text = ""; // Initialize the powerup text to an empty string
        uiManager = FindObjectOfType<UIManager>(); // Find the UIManager in the scene
        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager in the scene
        lifeText.text = "Lives: " + lifes; // Initialize the life text to display the number of lives
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        Constraints();
        if (score < 0)
        {
            EndGame(); // Call the EndGame method if the score is less than 0
        }
        if (powerupActive)
        {
            powerupTimer += Time.deltaTime; // Increment the powerup timer by the time passed since the last frame
            float timeLeft = powerupDuration - powerupTimer; // Calculate the time left for the powerup
            powerupText.text = "Power Up Time: " +(timeLeft.ToString("F1") + "s"); // Update the powerup text to indicate that a powerup is active
            if (powerupTimer >= powerupDuration)
            {
                powerupActive = false; // Set powerup active to false after the duration
                powerupText.text = ""; // Clear the powerup text
                powerupTimer = 0f; // Reset the powerup timer
            }
        }
        else
        {
            powerupText.text = ""; // Clear the powerup text if no powerup is active
        }
    }
    /// <summary>
    /// Moves the player left and right based on user input.
    /// </summary>
    void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * moveHorizontal * speed * Time.deltaTime);
    }
    void Constraints()
    {
        if (transform.position.x < -zBound)
        {
            transform.position = new Vector3(-zBound, transform.position.y, transform.position.z);
        }
        if (transform.position.x > zBound)
        {
            transform.position = new Vector3(zBound, transform.position.y, transform.position.z);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Items"))
        {
            scoring = collision.gameObject.GetComponent<Scoring>(); // Get the Scoring component from the collided item
            Destroy(collision.gameObject);
            if (powerupActive)
            {
                scoring.scoreamount *= 2; // Double the score amount if powerup is active
            }
            score += scoring.scoreamount; // Increment the score by 1
            scoreText.text = "Score: " + score;
            collectSound.Play(); // Play the collection sound effect
        }
        else if (collision.gameObject.CompareTag("trash"))
        {
            Destroy(collision.gameObject); // Destroy the trash AFTER detaching the effect
            if (powerupActive)
            {
                score -= 0;
            }
            else
            {
                score -= 3;
            }
            scoreText.text = "Score: " + score;
            badItemEffect.Play(); // Play the bad item effect
            badItemSound.Play(); // Play the bad item sound
        }
    }
    private IEnumerator PowerupDuration()
    {
        yield return new WaitForSeconds(powerupDuration); // Wait for the powerup duration
        powerupActive = false; // Set powerup active to false after the duration
        powerUpIndicator.SetActive(false); // Deactivate the powerup indicator
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("powerup"))
        {
            Destroy(other.gameObject);
            powerUpIndicator.SetActive(true); // Activate the powerup indicator
            powerupActive = true; // Set powerup active to true
            collectSound.Play(); // Play the collection sound effect
            StartCoroutine(PowerupDuration()); // Start the coroutine for powerup duration
            gameManager.powerUpCount--; // Decrement the powerup count in GameManager
        }
        else if (other.gameObject.CompareTag("life"))
        {
            Destroy(other.gameObject);
            if (lifes < 5) // Check if lives are less than the maximum allowed
            {
                lifes += 1; // Increment the number of lives
                lifeText.text = "Lives: " + lifes; // Update the life text
            }
            collectSound.Play(); // Play the collection sound effect
            gameManager.powerUpCount--; // Decrement the powerup count in GameManager
        }
    }
    private void EndGame()
    {
        // Logic to end the game, e.g., show game over screen, stop player movement, etc.
        Debug.Log("Game Over! Final Score: " + score);
        // You can call UIManager's EndGame method here if needed
        uiManager.EndGame(); // Call the EndGame method in UIManager to handle game over state
    }
}
