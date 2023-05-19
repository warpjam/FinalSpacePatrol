using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private int _laserSpeed = 8;
    [SerializeField] private GameObject _explosionPrefab;
    
    public bool _isEnemyLasers = false;
    private bool _isBackLaser = false;


    void Update()
    {
        if (_isEnemyLasers == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }


    private void MoveUp()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
        if (transform.position.y > 7.3f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);
        
        if (transform.position.y < -7.3f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }    
            Destroy((this.gameObject));
        }
    }

    public void AssignBackLaser()
    {
        _isBackLaser = true;
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLasers = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && (_isEnemyLasers == true || _isBackLaser == true))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
        }
        else if (other.CompareTag("PowerUps") && (_isEnemyLasers == true || _isBackLaser == true))
        {
            // Instantiate the explosion prefab at the powerup's location
            if (_explosionPrefab != null)
            {
                Instantiate(_explosionPrefab, other.transform.position, Quaternion.identity);
            }

            Destroy(other.gameObject); // destroy the powerup
            if (!_isBackLaser)
            {
                Destroy(this.gameObject); // destroy the laser itself if it's not a back laser
            }
        }
        else if (other.CompareTag("DodgingEnemy") && !_isEnemyLasers)
        {
            DodgingEnemy dodgingEnemy = other.GetComponent<DodgingEnemy>();
            if (dodgingEnemy != null && transform.position.y < dodgingEnemy.transform.position.y)
            {
                dodgingEnemy.Dodge();
            }
        }
    }
}
