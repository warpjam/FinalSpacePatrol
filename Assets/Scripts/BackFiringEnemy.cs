using UnityEngine;

public class BackFiringEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _backLaserPrefab;
    [SerializeField] private float _fireRate = 1f;
    private float _nextFire;
    [SerializeField] private float _speed = 4f;
    private Player _player;
    [SerializeField] private GameObject _explosionPrefab;

    

    private void Start()
    {
        _nextFire = Time.time + _fireRate;
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.Log("The player is Null!");
        }
    }

    private void Update()
    {
        Move();
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

        // Create a LayerMask for the Player layer
        LayerMask playerLayer = LayerMask.GetMask("Player");

        // Perform the Raycast and store the hit information, using the layer mask
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, playerLayer);

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


    private void Move()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

            if (transform.position.y < -6.4f)
            {
                float _randomX = Random.Range(-11, 11);
                transform.position = new Vector3(_randomX, 6, 0);
            }
    }


    private void FireBackwards()
    {
        Vector3 projectilePosition = new Vector3(transform.position.x, transform.position.y, 0);
        GameObject backLaser = Instantiate(_backLaserPrefab, projectilePosition, Quaternion.identity);
        Laser laserComponent = backLaser.GetComponent<Laser>();
        if (laserComponent != null)
        {
            laserComponent.AssignBackLaser();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }


            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _speed = 0;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Laser") || other.CompareTag("PlayerMissile"))
        {
            Destroy(other.gameObject);

            EnemyShield enemyShield = GetComponent<EnemyShield>();
            if (enemyShield != null && enemyShield.IsShieldActive())
            {
                enemyShield.DeactivateShield();
            }
            else
            {
                if (_player != null)
                {
                    _player.ScoreCalculator(10);
                }
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                _speed = 0;
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject);
            }
        }

        if (other.CompareTag("UniBeam"))
        {
            if (_player != null)
            {
                _player.ScoreCalculator(10);
            }
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _speed = 0;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject);
        }
    }


}