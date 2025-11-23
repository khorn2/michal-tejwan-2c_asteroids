using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Asteroid asteroidPrefab;

    public int asteroidCount = 0;
    private int level = 0;

    public GameManager()
    {
    }

    private void Start()
    {
        StartNewLevel();
    }

    // Wywoływane, gdy asteroida zostaje zniszczona
    public void OnAsteroidDestroyed()
    {
        asteroidCount--;

        if (asteroidCount <= 0)
        {
            StartNewLevel();
        }
    }

    private void StartNewLevel()
    {
        level++;
        int numAsteroids = 2 + (2 * level);

        for (int i = 0; i < numAsteroids; i++)
        {
            SpawnAsteroid();
        }

        asteroidCount = numAsteroids;
    }

    private void SpawnAsteroid()
    {
        float offset = Random.Range(0f, 1f);
        Vector2 viewportSpawnPosition = Vector2.zero;

        int edge = Random.Range(0, 4);

        switch (edge)
        {
            case 0: viewportSpawnPosition = new Vector2(offset, 0); break;
            case 1: viewportSpawnPosition = new Vector2(offset, 1); break;
            case 2: viewportSpawnPosition = new Vector2(0, offset); break;
            case 3: viewportSpawnPosition = new Vector2(1, offset); break;
        }

        Vector2 worldSpawnPosition =
            Camera.main.ViewportToWorldPoint(viewportSpawnPosition);

        Asteroid asteroid = Instantiate(asteroidPrefab, worldSpawnPosition, Quaternion.identity);
        asteroid.gameManager = this;
    }
}
