using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletliftime = 2f;

    private void Awake()
    {
        Destroy(gameObject, bulletliftime);
    }
}
