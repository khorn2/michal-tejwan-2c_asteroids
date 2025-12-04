using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private ParticleSystem destroyedParticles;
    public int size = 3;
    internal GameManager gameManager;
    public Asteroid asteroidPrefab; // przypisywane przez GameManager przy spawnie

    private bool isDestroyed = false; // zapobiega podwójnemu reagowaniu

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
        if (isDestroyed) return; // już w trakcie niszczenia

        if (collision == null) return;

        if (collision.CompareTag("Bullet"))
        {
            isDestroyed = true; // blokujemy dalsze przetwarzanie tej asteroidy

            Destroy(collision.gameObject);

            // Rozpad na mniejsze
            if (size > 1 && gameManager != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    // drobny losowy offset, żeby nie kolidował z pociskiem dnia 0
                    Vector2 offset = Random.insideUnitCircle * 0.2f;
                    Vector2 spawnPos = (Vector2)transform.position + offset;

                    // Tworzymy prefab przez GameManager (on zwiększy licznik i ustawi referencje)
                    Asteroid newAst = gameManager.SpawnAsteroidAt(spawnPos, size - 1);

                    if (newAst != null)
                    {
                        // nadajemy małym asteroidom losowy impuls, żeby nie stały w miejscu
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

            // Zgłoś GameManagerowi zniszczenie tej konkretnej asteroidy (jeśli nie robiliśmy tego wcześniej)
            if (gameManager != null)
            {
                gameManager.OnAsteroidDestroyed();
            }
            else
            {
                Debug.LogWarning("Asteroid: gameManager jest null podczas niszczenia (nie powinno się zdarzyć).");
            }

            // Odpowiada za pojawienie się particli przy zniszczeniu asteroidy
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
