using UnityEngine;
using UnityEngine.EventSystems;

public class RammingEnemy : MonoBehaviour
{
    [SerializeField] private float _ramDistance = 5f;
    [SerializeField] private float _ramSpeed = 6f;
    [SerializeField] private int _enemySpeed = 3;
    [SerializeField] private GameObject _explosionPrefab;

    private Player _player;

    void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is not found!");
        }
    }

    private void Update()
    {
        Move();

        if (_player != null && Vector3.Distance(transform.position, _player.transform.position) <= _ramDistance)
        {
            RamPlayer();
        }
    }

    private void RamPlayer()
    {
        Vector3 direction = (_player.transform.position - transform.position).normalized;
        transform.Translate(direction * _ramSpeed * Time.deltaTime);
    }

    private void Move()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -6.4f)
        {
            float _randomX = Random.Range(8f, -8f);
            transform.position = new Vector3(_randomX, 6f, 0f);
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

            Destroy(GetComponent<Collider2D>());
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
            Destroy(GetComponent<Collider2D>());
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
}
