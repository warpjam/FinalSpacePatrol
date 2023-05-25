using UnityEngine;


public class DodgingEnemy : MonoBehaviour
{
    [SerializeField] private float _dodgeDistance = 5.0f;
    [SerializeField] private float _enemySpeed = 3.0f;
    private bool _dodgeLeft = true;
    private Player _player;
    [SerializeField] private GameObject _enemyExplosion;
    private float _canFire = -1;
    private bool _canShoot = true;
    [SerializeField] private float _fireRate = 1;
    [SerializeField] private GameObject _enemyLaserPrefab;
    

    void Start()
    {
        _dodgeLeft = Random.value < 0.5f;
    }

    void Update()
    {
        EnemyMovement();
        EnemyFire();
    }
    
    public void Dodge()
    {
        // Dodges by teleporting enemy along the x-axis.
        // It can dodge to left or right depending on _dodgeLeft.
        float dodgeDirection = _dodgeLeft ? -1 : 1;
        Vector3 dodgeVector = new Vector3(dodgeDirection * _dodgeDistance, 0, 0);
        transform.position += dodgeVector;

        // After dodging, it should dodge to the other direction next time.
        _dodgeLeft = !_dodgeLeft;
    }
    
    private void EnemyMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -6.4f)
        {
            float _randomX = Random.Range(-11, 11);
            transform.position = new Vector3(_randomX, 6, 0);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        DodgingEnemy dodgingEnemy = GetComponent<DodgingEnemy>();

        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            Instantiate(_enemyExplosion, transform.position, Quaternion.identity);
            _enemySpeed = 0;
            _canShoot = false;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject);
        }

        if (other.CompareTag("PlayerMissile"))
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.ScoreCalculator((20));
            }

            Instantiate(_enemyExplosion, transform.position, Quaternion.identity);
            _enemySpeed = 0;
            _canShoot = false;
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject);
        }
        
        if (other.CompareTag("UniBeam"))
        {

            if (_player != null)
            {
                _player.ScoreCalculator((20));
            }

            Instantiate(_enemyExplosion, transform.position, Quaternion.identity);
            _enemySpeed = 0;
            _canShoot = false;
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject);
        }
        
        
    }


    private void EnemyFire()
    {
        if (_canShoot && Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

        }
    }
    
    private GameObject DetectPowerUp()
    {
        // Define the direction of the Raycast (downwards)
        Vector2 direction = -transform.up;

        // The length of the Raycast
        float distance = 10f;

        // Create a LayerMask for the PowerUp layer or use the "PowerUps" tag
        int powerUpLayer = LayerMask.NameToLayer("PowerUps"); // Replace "PowerUps" with the name of the layer, if you created one
        LayerMask powerUpLayerMask = 1 << powerUpLayer;

        // Perform the Raycast and store the hit information, using the layer mask
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, powerUpLayerMask);

        // Check if the Raycast hit something
        if (hit.collider != null)
        {
            // Check if the hit object has the "PowerUps" tag
            if (hit.collider.CompareTag("PowerUps"))
            {
                // A powerup is in front of the enemy
                return hit.collider.gameObject;
            }
        }

        // No powerup detected
        return null;
    }
}