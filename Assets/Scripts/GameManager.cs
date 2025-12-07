using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Asteroid asteroidPrefab;

    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    private int score = 0;

    public int asteroidCount = 0;
    private int level = 0;

    private void Start()
    {
        StartNewLevel();
            score = 0;
        UpdateScoreUI();
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
        asteroidCount = 0;
        
        level++;
        int numAsteroids = 2 + (2 * level);

        for (int i = 0; i < numAsteroids; i++)
        {
        
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

        SpawnAsteroidAt(worldSpawnPosition, asteroidPrefab != null ? asteroidPrefab.size : 3);
    }

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
        a.asteroidPrefab = asteroidPrefab; 
        asteroidCount++; 
        return a;
    }
    
    public void GameOver()
    {
        StartCoroutine(Restart());
    }
    private IEnumerator Restart()
    {
        Debug.Log("Game Over!");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return null;
    }

        public void AddScore(int asteroidSize)
    {
        int points = 0;

        switch (asteroidSize)
        {
            case 3: points = 20; break; 
            case 2: points = 50; break; 
            case 1: points = 100; break; 
        }

        score += points;
        UpdateScoreUI();
    }
        private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + score;
        }
    }

}