using UnityEngine;

public class PlayerMissile : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _rotationSpeed = 5f; 
    private GameObject _target;

    void Start()
    {
        // Find the closest enemy
        _target = FindClosestEnemy();
        if (_target == null)
        {
            Debug.Log("No enemies found.");
        }
    }

    void Update()
    {
        GameObject enemy = FindClosestEnemy();
    
        if (enemy != null)
        {
            Vector3 direction = (enemy.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // Subtract 90 to make the missile point upwards
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
        }
    
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    private GameObject FindClosestEnemy()
    {
        GameObject closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        // First check for DodgingEnemies
        GameObject[] dodgingEnemies = GameObject.FindGameObjectsWithTag("DodgingEnemy");
        if (dodgingEnemies.Length > 0)
        {
            foreach (GameObject enemy in dodgingEnemies)
            {
                Vector3 directionToTarget = enemy.transform.position - currentPosition;
                float distanceSqrToTarget = directionToTarget.sqrMagnitude;
                if (distanceSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqrToTarget;
                    closestEnemy = enemy;
                }
            }
            return closestEnemy;
        }

        // If no DodgingEnemies found, check for regular Enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Vector3 directionToTarget = enemy.transform.position - currentPosition;
            float distanceSqrToTarget = directionToTarget.sqrMagnitude;
            if (distanceSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqrToTarget;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }
}
