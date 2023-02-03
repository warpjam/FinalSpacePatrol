using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _playerSpeed = 5;
    [SerializeField] private bool _speedBoostActive;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    private bool _canFireLaser = true;
    [SerializeField] private int _playerLives = 3;
    private SpawnManager _spawnManager;
    [SerializeField] private bool _tripleShotActive;
    [SerializeField] bool _shieldActive = false;
    [SerializeField] private GameObject _playerShieldPrefab;
    [SerializeField] private GameObject _playerThrustersPrefab;
    [SerializeField] private GameObject _damageLeft; 
    [SerializeField] private GameObject _damageRight;
    [SerializeField] private int _score;
    [SerializeField] private AudioClip _basicLaserSound;

    private UIManager _uiManager;
    private AudioSource _audioSource;
    

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.Log("The Spawn manager is NULL!");
        }

        if (_uiManager == null)
        {
            Debug.Log("The UI Manager is NULL!");
        }

        if (_audioSource == null)
        {
            Debug.Log("The AudioSource on the Player is Null");
        }
        else
        {
            _audioSource.clip = _basicLaserSound;
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
        
        _audioSource.Play();
    }

    private void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        float playerSpeed = _playerSpeed;
        float thrustersScale = 0.5f;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            playerSpeed *= 2;
            thrustersScale += 0.5f;
        }

        transform.Translate(direction * playerSpeed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -5.0f, 5.0f), 0);

        if (horizontalInput != 0 || verticalInput != 0)
        {
            _playerThrustersPrefab.SetActive(true);
            _playerThrustersPrefab.transform.localScale = new Vector3(thrustersScale, thrustersScale, thrustersScale);
        }
        else
        {
            _playerThrustersPrefab.SetActive(false);
        }

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
        if (_shieldActive == true)
        {
            _shieldActive = false;
            _playerShieldPrefab.SetActive(false);
            return;
        }
        _playerLives--;
        _uiManager.UpdateLives(_playerLives);

        if (_playerLives == 2)
        {
            _damageLeft.SetActive(true);
        }
        else if (_playerLives == 1)
        {
            _damageRight.SetActive(true);
        }
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

    public void SpeedBoostActive()
    {
        _speedBoostActive = true;
        _playerSpeed = 10;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        while (_speedBoostActive == true)
        {
            yield return new WaitForSeconds(6.0f);
            _speedBoostActive = false;
            _playerSpeed = 5;
        }
    }

    public void ShieldsUp()
    {
        _shieldActive = true;
        _playerShieldPrefab.SetActive(true);
    }

    public void ScoreCalculator(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
