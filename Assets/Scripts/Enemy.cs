using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int _enemySpeed = 4;
    private Player _player;
    private Animator _enemyExplosion;
    private AudioSource _audioSource;
    [SerializeField] private GameObject _enemyLaserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    private bool _canShoot = true;

    protected virtual void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _enemyExplosion = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        if (_player == null)
        {
            Debug.Log("The player is Null!");
        }

        if (_enemyExplosion == null)
        {
            Debug.Log("The Enemy Explosion is Null!");
        }

        if (_audioSource == null)
        {
            Debug.Log("The enemy AudioSource is null!");
        }

    }

    protected virtual void Update()
    {
        EnemyMovement();

        // Detect powerups in front of the enemy
        GameObject detectedPowerUp = DetectPowerUp();
        if (detectedPowerUp != null)
        {
            Debug.Log("Must Kill PowerUps");
            EnemyFire();
        }
        else if (Time.time > _canFire)
        {
            // Fire randomly if no powerup detected
            EnemyFire();
            _canFire = Time.time + _fireRate;
        }
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
            
            EnemyShield enemyShield = GetComponent<EnemyShield>();
            if (enemyShield != null && enemyShield.IsShieldActive())
            {
                enemyShield.DeactivateShield();
                
            }
            
            _enemyExplosion.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            _canShoot = false;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

        if (other.CompareTag("Laser") || other.CompareTag(("PlayerMissile")))
        {
            Destroy(other.gameObject);

            EnemyShield enemyShield = GetComponent<EnemyShield>();
            if (enemyShield != null && enemyShield.IsShieldActive())
            {
                enemyShield.DeactivateShield();
                
            }
            else if (dodgingEnemy == null) // Only destroy the enemy if it's not a DodgingEnemy
            {
                if (_player != null)
                {
                    _player.ScoreCalculator(10);
                }

                _enemyExplosion.SetTrigger("OnEnemyDeath");
                _audioSource.Play();
                _enemySpeed = 0;
                _canShoot = false;
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.0f);
            }
        }

        if (other.CompareTag("UniBeam"))
        {
            if (_player != null)
            {
                _player.ScoreCalculator(10);
            }

            _enemyExplosion.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            _canShoot = false;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.0f);
            
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
