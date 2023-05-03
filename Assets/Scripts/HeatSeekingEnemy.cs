using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class HeatSeekingEnemy : MonoBehaviour
{
    [SerializeField] private int _enemySpeed = 4;
    [SerializeField] private GameObject _heatSeekingMissilePrefab;
    [SerializeField] private int _enemyID; // 0 = Basic Enemy, 1 = S-Wave Enemy, 2 = Hornet_HeatSeeker
    private float sSpeed = 2f;
    private float sRange = 1f;
    [SerializeField] private float _fireRate = 3.0f;
    private float _nextFire = -1f;
    

    private Player _player;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is Null!");
        }
    }

    void Update()
    {

        ZigzagMovement();
        FireHeatSeekingMissile();
        
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        if (transform.position.y < -6.4f)
        {
            float _randomX = Random.Range(-11, 11);
            transform.position = new Vector3(_randomX, 6, 0);
        }
    }
    
    private void ZigzagMovement()
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

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            Destroy(gameObject);
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.ScoreCalculator(10);
            }

            Destroy(gameObject);
        }
        
        if (other.CompareTag("UniBeam"))
        {
            if (_player != null)
            {
                _player.ScoreCalculator(10);
            }
            //TODO find another explosion ani _enemyExplosion.SetTrigger("OnEnemyDeath");
            //TODO link the sound to _audioSource.Play();
            _enemySpeed = 0;
            //_canShoot = false;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject,2.0f);
        }
    }


    private void FireHeatSeekingMissile()
    {
        if (Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;
            Instantiate(_heatSeekingMissilePrefab, transform.position, Quaternion.identity);
        }
        // Basic missile firing
        // We'll improve this to fire heat-seeking missiles later
    }
}
