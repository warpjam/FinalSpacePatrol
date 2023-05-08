using UnityEngine;

public class BackFiringEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _backLaserPrefab;
    [SerializeField] private float _fireRate = 1f;
    private float _nextFire;

    private void Start()
    {
        _nextFire = Time.time + _fireRate;
    }

    private void Update()
    {
        if (IsBehindPlayer() && Time.time > _nextFire)
        {
            FireBackwards();
            _nextFire = Time.time + _fireRate;
        }
    }
    private bool IsBehindPlayer()
    {
        // Define the direction of the Raycast (upwards)
        Vector2 direction = transform.up;

        // The length of the Raycast (e.g., 10 units)
        float distance = 10f;

        // Perform the Raycast and store the hit information
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance);

        // Check if the Raycast hit something
        if (hit.collider != null)
        {
            // Check if the hit object has the "Player" tag
            if (hit.collider.CompareTag("Player"))
            {
                // The player is behind the enemy
                return true;
            }
        }

        // The player is not behind the enemy
        return false;
    }


    private void FireBackwards()
    {
        Vector3 projectilePosition = new Vector3(transform.position.x, transform.position.y, 0);
        Instantiate(_backLaserPrefab, projectilePosition, Quaternion.identity);
    }
}