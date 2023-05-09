using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _enemySpeed = 4;
    private Player _player;
    private Animator _enemyExplosion;
    private AudioSource _audioSource;
    [SerializeField] private GameObject _enemyLaserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    [SerializeField] private int _enemyID; // 0 = Basic Enemy, 1 = S-Wave Enemy, 2- Missile, 3- Ramming 4- BackFire 
    private float sSpeed = 2f;
    private float sRange = 1f;
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
        EnemyFire();
    }

    private void EnemyMovement()
    {
        if(_enemyID == 1)
        {
            float y = transform.position.y - Time.deltaTime * _enemySpeed;

            // calculate the horizontal position based on time and sSpeed
            float x = Mathf.PingPong(Time.time * sSpeed, sRange * 2) - sRange;

            transform.position = new Vector3(x, y, 0f);

            if (transform.position.y < -6.4f)
            {
                float _randomX = Random.Range(-11, 11);
                transform.position = new Vector3(_randomX, 6, 0);
            }
        }
        else
        {
            transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

            if (transform.position.y < -6.4f)
            {
                float _randomX = Random.Range(-11, 11);
                transform.position = new Vector3(_randomX, 6, 0);
            }
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

            _enemyExplosion.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            _canShoot = false;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

        if (other.CompareTag("Laser") || other.CompareTag("UniBeam"))
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
                _enemyExplosion.SetTrigger("OnEnemyDeath");
                _audioSource.Play();
                _enemySpeed = 0;
                _canShoot = false;
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.0f);
            }
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

}
