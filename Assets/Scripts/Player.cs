using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _playerSpeed = 5;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    private bool _canFireLaser = true;
    [SerializeField] private int _playerLives = 3;
    private SpawnManager _spawnManager;
    [SerializeField] private bool _tripleShotActive;
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.Log("The Spawn manager is NULL!");
        }

    }


    void Update()
    {
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.Space) && _canFireLaser)
        {
            FireMainWeapons();
        }
        
    }

    private void FireMainWeapons()
    {
        if (_tripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 0f, 0), Quaternion.identity);
            _canFireLaser = false;
            StartCoroutine(ReloadLaserTimer());
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
            _canFireLaser = false;
            StartCoroutine(ReloadLaserTimer()); 
        }
        
    }

    private void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _playerSpeed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -5.0f, 5.0f), 0);

        if (transform.position.x > 11f)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11f)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    IEnumerator ReloadLaserTimer()
    {
        yield return new WaitForSeconds(0.7f);
        _canFireLaser = true;
    }

    public void Damage()
    {
        _playerLives--;
        if (_playerLives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        while (_tripleShotActive == true)
        {
           yield return new WaitForSeconds(5.0f);
            _tripleShotActive = false; 
        }
        
    }
}
