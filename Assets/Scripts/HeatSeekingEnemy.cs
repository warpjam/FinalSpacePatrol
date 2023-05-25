using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class HeatSeekingEnemy : MonoBehaviour
{
    [SerializeField] private int _enemySpeed = 3;
    [SerializeField] private GameObject _heatSeekingMissilePrefab;
    [SerializeField] private float _fireRate = 5.0f;
    [SerializeField] private GameObject _explosionPrefab; 
    private float _nextFire = -1f;
    
    //SWave Variables
    private float _sinCenterX;
    private float _xAmplitude = 1f;
    private float _xFrequency = 1f;
    private float _sinMove;
    private Vector2 _pos;
    private float _initialXPosition;
    

    private Player _player;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is Null!");
        }
        
        _sinCenterX = transform.position.x;
        _initialXPosition = Random.Range(8.9f, -7.9f); // Store the initial X position
        transform.position = new Vector3(_initialXPosition, 6f, 0f); // Set the initial position
    }

    void Update()
    {
        Move();
        FireHeatSeekingMissile();
    }
    
    
    private void FixedUpdate()
    {
        _pos = transform.position;
        _sinMove = Mathf.Sin(_pos.y * _xFrequency) * _xAmplitude;
        _pos.x = _sinCenterX + _sinMove + _initialXPosition; // Add the initial X position offset

        transform.position = _pos;
    }

    private void Move()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -6.4f)
        {
            float _randomX = Random.Range(-11f, 11f);
            transform.position = new Vector3(_randomX, 6f, 0f);
            _initialXPosition = _randomX; // Update the initial X position
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
            Destroy(gameObject);
        }

        if (other.CompareTag("Laser") || other.CompareTag("PlayerMissile"))
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.ScoreCalculator(10);
            }
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
        if (other.CompareTag("UniBeam"))
        {
            if (_player != null)
            {
                _player.ScoreCalculator(20);
            }
            Destroy(GetComponent<Collider2D>());
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }


    private void FireHeatSeekingMissile()
    {
        if (Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;
            Instantiate(_heatSeekingMissilePrefab, transform.position, Quaternion.identity);
        }
    }
}
