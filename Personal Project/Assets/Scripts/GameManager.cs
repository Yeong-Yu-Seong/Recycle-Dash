using UnityEngine;
using System.Collections; // Importing System.Collections namespace for IEnumerator

public class GameManager : MonoBehaviour
{
    public GameObject[] items;
    private float spawnInterval = 5f; // Interval for spawning items
    private float timePassed = 0f; // Timer to track the time passed since the last item spawn
    private float spawnStartDelay = 2f; // Delay before the first item spawn
    public GameObject[] powerUpItems; // Array of powerup items to spawn
    public int powerUpCount = 0; // Counter for powerup items spawned
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnItems", spawnStartDelay, spawnInterval); // Start spawning items after 2 seconds and repeat every 2 seconds
        StartCoroutine(PowerUpSpawner()); // Start the coroutine to spawn powerup items
    }

    // Update is called once per frame
        void Update()
    {
        timePassed += Time.deltaTime; // Increment the timer by the time passed since the last frame
        if (timePassed > 10f)
        {
            if (spawnInterval > 1f) // Ensure the spawn interval does not go below 1 second
            {
                spawnInterval -= 0.5f; // Decrease the spawn interval by 1 second every 10 seconds
                spawnStartDelay = 1f;
            }
            CancelInvoke("SpawnItems"); // Stop the previous invocation of SpawnItems
            InvokeRepeating("SpawnItems", spawnStartDelay, spawnInterval); // Start a new invocation with the updated spawn interval
            timePassed = 0f; // Reset the timer
        }
    }
    private void SpawnItems()
    {
        // Randomly select an item from the items array
        int randomIndex = Random.Range(0, items.Length);
        GameObject itemToSpawn = items[randomIndex];

        // Set a random position for the item
        float randomX = Random.Range(-10f, 10f);
        Vector3 spawnPosition = new Vector3(randomX, 0, 10);

        // Instantiate the item at the random position
        Instantiate(itemToSpawn, spawnPosition, itemToSpawn.transform.rotation);
    }
    public void StopSpawningItems()
    {
        CancelInvoke("SpawnItems"); // Stop invoking the SpawnItems method
        StopCoroutine(PowerUpSpawner()); // Stop the PowerUpSpawner coroutine
    }
    private void PowerUpSpawn()
    {
        int randomIndex = Random.Range(0, powerUpItems.Length);
        GameObject powerUpToSpawn = powerUpItems[randomIndex];

        // Set a random position for the item
        float randomX = Random.Range(-10f, 10f);
        Vector3 spawnPosition = new Vector3(randomX, 0, -4);

        // Instantiate the item at the random position
        Instantiate(powerUpToSpawn, spawnPosition, powerUpToSpawn.transform.rotation);
        powerUpCount++; // Increment the powerup count
    }
    private IEnumerator PowerUpSpawner()
    {
        while (true)
        {
            if (powerUpCount < 5)
            {
                PowerUpSpawn();
            }

            yield return new WaitForSeconds(10f); // Check every 10 seconds
        }
    }
}
