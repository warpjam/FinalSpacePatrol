using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSeekingMissile : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _detectionRange = 5f;
    [SerializeField] private GameObject _explosion;
    private Player _player;

    void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is not found!");
        }

        Destroy(gameObject, 4f); // Self destruct after 4 seconds
    }

    void Update()
    {
        if (_player != null && Vector3.Distance(transform.position, _player.transform.position) <= _detectionRange)
        {
            Vector3 direction = _player.transform.position - transform.position;
            direction.Normalize();
            transform.Translate(direction * _speed * Time.deltaTime, Space.World);
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);

        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Damage player
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Laser"))
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}