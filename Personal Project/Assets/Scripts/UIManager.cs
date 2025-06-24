using UnityEngine;
using UnityEngine.SceneManagement; // Importing SceneManagement namespace for scene management

public class UIManager : MonoBehaviour
{
    public Canvas startCanvas; // Reference to the start canvas
    public Canvas gameCanvas; // Reference to the game canvas
    public Canvas endCanvas; // Reference to the end canvas
    PlayerController playerController; // Reference to the PlayerController script
    GameManager gameManager; // Reference to the GameManager script
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager in the scene
        playerController = FindObjectOfType<PlayerController>(); // Find the PlayerController in the scene
        startCanvas.enabled = true; // Enable the start canvas
        gameCanvas.enabled = false; // Disable the game canvas
        endCanvas.enabled = false; // Ensure the end canvas is disabled at the start
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartGame()
    {
        startCanvas.enabled = false; // Disable the start canvas
        gameCanvas.enabled = true; // Enable the game canvas
        endCanvas.enabled = false; // Ensure the end canvas is disabled at the start
        playerController.enabled = true; // Enable the PlayerController script
        gameManager.enabled = true; // Enable the GameManager script
    }
    public void EndGame()
    {
        gameCanvas.enabled = false; // Disable the game canvas
        endCanvas.enabled = true; // Enable the end canvas
        playerController.enabled = false; // Disable the PlayerController script
        gameManager.StopSpawningItems(); // Stop spawning items in the GameManager
        gameManager.enabled = false; // Disable the GameManager script
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0); // Reload the current scene to restart the game
        playerController.lifes = 5; // Reset the player's lives to 5
        playerController.lifeText.text = "Lives: " + playerController.lifes; // Update
    }
}
