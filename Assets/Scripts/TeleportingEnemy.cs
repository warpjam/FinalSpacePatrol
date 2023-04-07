using System.Collections;
using UnityEngine;

public class TeleportingEnemy : MonoBehaviour
{
    [SerializeField] private int _enemySpeed = 4;
    [SerializeField] private GameObject _laserSpike;
    private float _teleportTimerMin = 3f;
    private float _teleportTimerMax = 12f;
    private Player _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is Null!");
        }
        StartCoroutine(TeleportRoutine());
    }

    private void Update()
    {
        MoveDown();
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

    private IEnumerator TeleportRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_teleportTimerMin, _teleportTimerMax));

            Teleport();
            //StartCoroutine(ActivateLaserSpike());
        }
    }

    private void Teleport()
    {
        float randomX = Random.Range(-11, 11);
        float randomY = Random.Range(-4, 4);

        transform.position = new Vector3(randomX, randomY, 0);
    }

    private IEnumerator ActivateLaserSpike()
    {
        _laserSpike.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        _laserSpike.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_player != null)
            {
                _player.Damage();
            }
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.ScoreCalculator(10);
            }
            Destroy(this.gameObject);
        }
    }
}
