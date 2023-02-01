using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _enemySpeed = 4;
    private Player _player;
    private Animator _enemyExplosion;
    private AudioSource _audioSource;

    void Start()
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

    void Update()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -6.4f)
        {
            float _randomX = Random.Range(-11, 11);
            transform.position = new Vector3(_randomX,6,0);
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
            Destroy(GetComponent<Collider2D>());
            Debug.Log("I'm killed by the player");
            Destroy(this.gameObject, 2.8f);
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.ScoreCalculator(10);
            }
            _enemyExplosion.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _enemySpeed = 0;
            Destroy(GetComponent<Collider2D>());
            Debug.Log("I'm killed by the Laser");
            Destroy(this.gameObject,2.0f);
        }

        if (other.CompareTag("PowerUps"))
        {
            Debug.Log("Why!?");
        }

    }
}
