using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private ParticleSystem destroyedParticles;
    public int size = 3;
    internal GameManager gameManager;
    public Asteroid asteroidPrefab; 

    private bool isDestroyed = false; 

    private void Start()
    {
        transform.localScale = Vector3.one * (0.5f * size);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Vector2 direction = Random.insideUnitCircle.normalized;
        float spawnSpeed = Random.Range(4f - size, 5f - size);

        if (rb != null)
            rb.AddForce(direction * spawnSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDestroyed) return; 

        if (collision == null) return;

        if (collision.CompareTag("Bullet"))
        {
            isDestroyed = true; 

            Destroy(collision.gameObject);


            if (size > 1 && gameManager != null)
            {
                for (int i = 0; i < 2; i++)
                {

                    Vector2 offset = Random.insideUnitCircle * 0.2f;
                    Vector2 spawnPos = (Vector2)transform.position + offset;


                    Asteroid newAst = gameManager.SpawnAsteroidAt(spawnPos, size - 1);

                    if (newAst != null)
                    {

                        Rigidbody2D newRb = newAst.GetComponent<Rigidbody2D>();
                        if (newRb != null)
                        {
                            Vector2 randomDir = Random.insideUnitCircle.normalized;
                            float s = Random.Range(1f, 3f);
                            newRb.AddForce(randomDir * s, ForceMode2D.Impulse);
                        }
                    }
                }
            }


            if (gameManager != null)
            {
                gameManager.AddScore(size);
                gameManager.OnAsteroidDestroyed();
            }
            else
            {
                Debug.LogWarning("Asteroid: gameManager jest null podczas niszczenia (nie powinno się zdarzyć).");
            }


            Instantiate(destroyedParticles, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
