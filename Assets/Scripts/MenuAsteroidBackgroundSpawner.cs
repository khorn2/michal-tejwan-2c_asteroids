using UnityEngine;

public class MenuAsteroidSpawner : MonoBehaviour
{
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private int asteroidCount = 8;

    private void Start()
    {
        SpawnBackgroundAsteroids();
    }

    private void SpawnBackgroundAsteroids()
    {
        for (int i = 0; i < asteroidCount; i++)
        {
            Vector2 viewportPos = new Vector2(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
            );

            Vector2 worldPos = Camera.main.ViewportToWorldPoint(viewportPos);

            Asteroid a = Instantiate(asteroidPrefab, worldPos, Quaternion.identity);

            // wyłączamy logikę gry (rozbijanie, punkty itd.)
            a.enabled = false;

            Rigidbody2D rb = a.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Random.insideUnitCircle.normalized * Random.Range(0.5f, 2f);
                rb.angularVelocity = Random.Range(-40f, 40f);
            }
        }
    }
}
