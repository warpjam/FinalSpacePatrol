using System;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.Translate(transform.up * -_speed * Time.deltaTime); // Added a negative sign to move the laser downwards
    
        if (transform.position.y < -7.3f)
        {
            Destroy((this.gameObject));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject);
        }
    }
}
