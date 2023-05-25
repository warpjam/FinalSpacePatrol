using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class FinalBoss : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _bossHealth = 100;
    [SerializeField] private Vector2 _xBounds = new Vector2(-9f, 9f);
    [SerializeField] private Vector2 _yBounds = new Vector2(9f, 5f);
    [SerializeField] private GameObject _bossLaserPrefab;
    [SerializeField] private GameObject _bossMissilePrefab;
    [SerializeField] private GameObject _bossMinePrefab;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _hitExplosionPrefab;
    [SerializeField] private int _numMines = 1;
    [SerializeField] private float _missileFireRate = 1f;
    private Vector2 _targetPosition;
    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private bool _canAttack;

    private void Start()
    {
        _canAttack = true;
        StartCoroutine(AttackRoutine());
        _targetPosition = new Vector2(Random.Range(_xBounds.x, _xBounds.y), Random.Range(_yBounds.x, _yBounds.y));
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, _targetPosition) <= 0.1f)
        {
            // If we've arrived at the target position, set a new one
            _targetPosition = new Vector2(Random.Range(_xBounds.x, _xBounds.y), Random.Range(_yBounds.x, _yBounds.y));
        }
        else
        {
            // Move towards the target position
            float step = _speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, step);
        }
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (_canAttack)
        {
            int attack = Random.Range(0, 3);

            switch (attack)
            {
                case 0:
                    LaserBarrage();
                    break;
                case 1:
                    MissileBarrage();
                    break;
                case 2:
                    MineLayer();
                    break;
            }

            yield return new WaitForSeconds(4f);
        }
    }
    
    
    private void LaserBarrage()
    {
        int numLasers = 15; // This determines the number of lasers in each barrage

        float minAngle = -75f;
        float maxAngle = 75f;
        float angleStep = (maxAngle - minAngle) / (numLasers - 1);

        for (int i = 0; i < numLasers; i++)
        {
            // This will create lasers pointing from -75 to +75 degrees
            float rotationZ = minAngle + angleStep * i;
            Quaternion rotation = Quaternion.Euler(0, 0, rotationZ);
            GameObject laser = Instantiate(_bossLaserPrefab, transform.position, rotation);
        }
    }
    
    private void MissileBarrage()
    {
        int numMissiles = 5; // This determines the number of missiles in each barrage

        float minAngle = -75f;
        float maxAngle = 75f;
        float angleStep = (maxAngle - minAngle) / (numMissiles - 1);

        for (int i = 0; i < numMissiles; i++)
        {
            // This will create missiles pointing from -75 to +75 degrees
            float rotationZ = minAngle + angleStep * i;
            Quaternion rotation = Quaternion.Euler(0, 0, rotationZ);
            GameObject missile = Instantiate(_bossMissilePrefab, transform.position, rotation);
        }
    }

    private void MineLayer()
    {
        Vector2 spawnRangeX = new Vector2(-10f, 10f);
        Vector2 spawnRangeY = new Vector2(-6f, 1f);

        for (int i = 0; i < _numMines; i++)
        {
            Vector3 positionToSpawn = new Vector3(
                Random.Range(spawnRangeX.x, spawnRangeX.y),
                Random.Range(spawnRangeY.x, spawnRangeY.y),
                0);

            GameObject mine = Instantiate(_bossMinePrefab, positionToSpawn, Quaternion.identity);
        }
    }

    public void TakeDamage(float damage, Vector3 position)
    {
        Instantiate(_hitExplosionPrefab, position, Quaternion.identity);
        _bossHealth -= Mathf.RoundToInt(damage);
        if (_bossHealth <= 0)
        {
            _canAttack = false;
            StartCoroutine(DestructionSequence());
            _spawnManager.StopAllCoroutines();
            
        }
    }

    IEnumerator DestructionSequence()
    {
        int numExplosions = 15;
        float explosionDelay = 0.2f;
        Vector2 explosionArea = new Vector2(20, 5);

        for (int i = 0; i < numExplosions; i++)
        {
            Vector2 randomPosition = new Vector2(
                transform.position.x + Random.Range(-explosionArea.x / 2, explosionArea.x / 2),
                transform.position.y + Random.Range(-explosionArea.y / 2, explosionArea.y / 2)
                );

            Instantiate(_explosionPrefab, randomPosition, Quaternion.identity);

            yield return new WaitForSeconds(explosionDelay);
        } 
        
        Destroy(gameObject);
        _uiManager.GameWonSequence();
    }
    

}
