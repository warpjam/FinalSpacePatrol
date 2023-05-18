using UnityEngine;

public class JangoMine : MonoBehaviour
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private float _explosionRadius = 1f;
    private Player _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        Invoke("Explode", 5f);
        
        if (_player == null)
        {
            Debug.Log("The player is Null!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player.Damage();
            Explode();
        }
        else if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Explode();
        }
    }

    private void Explode()
    {
        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 2.5f);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                _player.Damage();
            }
        }
        
        Destroy(this.gameObject);
    }
}
