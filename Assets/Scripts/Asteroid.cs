using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int size = 3;

    private void Start()
    {
        transform.localScale = Vector3.one * (0.5f * size);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Losowy kierunek ruchu
        Vector2 direction = Random.insideUnitCircle.normalized;

        // Losowa prędkość w zależności od rozmiaru asteroidy
        float spawnSpeed = Random.Range(4f - size, 5f - size);

        rb.AddForce(direction * spawnSpeed, ForceMode2D.Impulse);
    }
}
