using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IGameManager
{
    void AddScore(int asteroidSize);
    void GameOver();
    void OnAsteroidDestroyed();
    void ResetHighscore();
    Asteroid SpawnAsteroidAt(Vector2 position, int size);
}

public class GameManager : MonoBehaviour, IGameManager
{
    [SerializeField] private Asteroid asteroidPrefab;

    [Header("UI")]
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI highscoreText;

    private int score = 0;
    private int highscore = 0;
    private const string HighscoreKey = "AsteroidsHighscore_v1";

    public int asteroidCount = 0;
    private int level = 0;

    private void Awake()
    {
        // Wczytaj highscore z PlayerPrefs jak najwcześniej
        highscore = PlayerPrefs.GetInt(HighscoreKey, 0);
    }

    private void Start()
    {
        Time.timeScale = 1f;
        score = 0;
        UpdateScoreUI();
        UpdateHighscoreUI();

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
        // tutaj można dodać dodatkowe zachowanie przed restartem (np. ekran Game Over)
        StartCoroutine(Restart());
    }

    private IEnumerator Restart()
    {
        Debug.Log("Game Over!");
        yield return new WaitForSeconds(2f);
        // przed restartem zapisz highscore (na wszelki wypadek)
        SaveHighscore();
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

        // aktualizuj highscore jeśli potrzeba
        if (score > highscore)
        {
            highscore = score;
            UpdateHighscoreUI();
            SaveHighscore();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + score;
        }
    }

    private void UpdateHighscoreUI()
    {
        if (highscoreText != null)
        {
            highscoreText.text = "HIGHSCORE: " + highscore;
        }
    }

    private void SaveHighscore()
    {
        PlayerPrefs.SetInt(HighscoreKey, highscore);
        PlayerPrefs.Save();
    }

    // opcjonalne: metoda do resetu rekordu (możesz podpiąć do przycisku w UI)
    public void ResetHighscore()
    {
        highscore = 0;
        SaveHighscore();
        UpdateHighscoreUI();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ResetHighscore();
            Debug.Log("Highscore zresetowany");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }


    private bool isPaused = false;
    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            Debug.Log("PAUZA");
        }
        else
        {
            Time.timeScale = 1f;
            Debug.Log("WZNOWIENIE");
        }
    }
}