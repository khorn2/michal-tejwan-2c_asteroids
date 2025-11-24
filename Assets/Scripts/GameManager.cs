using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Asteroid asteroidPrefab;

    public int asteroidCount = 0;
    private int level = 0;

    private void Start()
    {
        StartNewLevel();
    }

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
            // spawn w losowym miejscu przy krawędzi
            SpawnAsteroid();
        }
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

        // użyj metody, która stworzy prefab, ustawi referencje i zwiększy licznik
        SpawnAsteroidAt(worldSpawnPosition, asteroidPrefab != null ? asteroidPrefab.size : 3);
    }

    // nowa metoda do tworzenia asteroid (używana też przez Asteroid do rozbicia)
    public Asteroid SpawnAsteroidAt(Vector2 position, int size)
    {
        if (asteroidPrefab == null)
        {
            Debug.LogError("GameManager: asteroidPrefab nie jest przypisany w inspectorze!");
            return null;
        }

        Asteroid a = Instantiate(asteroidPrefab, position, Quaternion.identity);
        a.size = size;
        a.gameManager = this;
        a.asteroidPrefab = asteroidPrefab; // żeby mniejsze mogły wiedzieć, co instantiować
        asteroidCount++; // GameManager kontroluje licznik
        return a;
    }
    
}