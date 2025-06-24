using UnityEngine;

public class MoveDown : MonoBehaviour
{
    public float speed = 5f;
    private float zBound = -10f;
    public AudioSource lifeLostSound; // Reference to the AudioSource component for sound effects
    UIManager uiManager; // Reference to the UIManager to update the uncollected items count
    PlayerController playerController; // Reference to the PlayerController to manage player interactions
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>(); // Find the PlayerController in the scene
        uiManager = FindObjectOfType<UIManager>(); // Find the UIManager in the scene
        lifeLostSound = GetComponent<AudioSource>(); // Get the AudioSource component attached to the object
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
        if (transform.position.z < zBound)
        {
            if (gameObject.CompareTag("Items"))
            {
                AudioSource.PlayClipAtPoint(lifeLostSound.clip, transform.position); // Play the life lost sound effect
                playerController.lifes -= 1;
                playerController.lifeText.text = "Lives: " + playerController.lifes; // Update the lives text in PlayerController
            }
            Destroy(gameObject);
            if (playerController.lifes <= 0)
            {
                uiManager.EndGame(); // Call the EndGame method in UIManager
            }
        }
    }
}
