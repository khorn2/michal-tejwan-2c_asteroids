using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Ship parameters")]
    [SerializeField] private float shipAcceleration = 10f;
    [SerializeField] private float shipMaxVelocity = 10f;
    [SerializeField] private float shipRotationSpeed = 100f;
    [SerializeField] private float bulletSpeed = 8f;

    [Header("Object references")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Rigidbody2D bulletPrefab;

    private Rigidbody2D shipRigidbody;
    private bool isAlive = true;
    private bool isAccelerating = false;

    private void Start()
    {
        shipRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isAlive)
        {
            HandleShipAcceleration();
            HandleShipRotation();
            HandleShooting();
        }
    }

    private void FixedUpdate()
    {
        if (isAlive && isAccelerating)
        {
            shipRigidbody.AddForce(shipAcceleration * transform.up);
            shipRigidbody.velocity = Vector2.ClampMagnitude(shipRigidbody.velocity, shipMaxVelocity);
        }
    }

    private void HandleShipAcceleration()
    {
        isAccelerating = Input.GetKey(KeyCode.UpArrow);
    }

    private void HandleShipRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * shipRotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.back * shipRotationSpeed * Time.deltaTime);
        }
    }

    private void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Utwórz pocisk w pozycji działka
            Rigidbody2D bullet = Instantiate(bulletPrefab, bulletSpawn.position, transform.rotation);

            // Kierunek w którym statek aktualnie patrzy
            Vector2 shipDirection = transform.up;

            // Oblicz prędkość statku w kierunku, w którym leci
            float shipForwardSpeed = Vector2.Dot(shipRigidbody.velocity, shipDirection);

            // Jeśli statek porusza się do tyłu, nie dodawaj ujemnej prędkości
            if (shipForwardSpeed < 0)
                shipForwardSpeed = 0;

            // Nadaj pociskowi prędkość równą prędkości statku + prędkość pocisku
            bullet.velocity = shipRigidbody.velocity + shipDirection * bulletSpeed;
        }
    }
}


