using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private int _laserSpeed = 8;
    private bool _isEnemyLasers = false;
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
    }

}
