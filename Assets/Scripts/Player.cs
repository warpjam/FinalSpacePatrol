using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _playerSpeed = 5;
    [SerializeField] private bool _speedBoostActive;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    private bool _canFireLaser = true;
    private SpawnManager _spawnManager;
    [SerializeField] private bool _tripleShotActive;
    [SerializeField] private GameObject _playerThrustersPrefab;
    [SerializeField] private GameObject _damageLeft; 
    [SerializeField] private GameObject _damageRight;
    [SerializeField] private int _score;
    [SerializeField] private AudioClip _basicLaserSound;

    [Header("Thrusters")] 
    [SerializeField] private int _thrustPower = 100;
    [SerializeField] private bool _canThrust = true;
    
    [Header("Weapons")] 
    [SerializeField] private int _ammoCount = 15;
    [SerializeField] private AudioClip _emptyLaserSound;
    [SerializeField] private GameObject _uniBeamPrefab;
    [SerializeField] private bool _uniBeamActive;
    [SerializeField] private AudioClip _uniBeamSound;
    
    [Header("Shields-Lives-Damage")]
    [SerializeField] private int _playerLives = 3;
    private SpriteRenderer _shieldHitColor;
    [SerializeField] bool _shieldActive = false;
    [SerializeField] private int _shieldHits;
    [SerializeField] private GameObject _playerShieldPrefab;

    private UIManager _uiManager;
    private AudioSource _audioSource;
    private CameraShake _cameraShake;
    


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();

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
            if (_ammoCount == 0 && _tripleShotActive == !true)
            {
                AudioSource.PlayClipAtPoint(_emptyLaserSound, transform.position);
            }
            else 
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
            _ammoCount--;
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
        
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) && _speedBoostActive != true)
        {
            _thrustPower--;
            _uiManager.UpdateThrustSlider(_thrustPower);
            playerSpeed *= 1.25f;
            thrustersScale += 0.25f;
            if (_thrustPower == 0)
            {
                _canThrust = false;
                StartCoroutine(RechargeThrusters());
            }
        }

        transform.Translate(direction * playerSpeed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -5.0f,5.0f), 0);

        if (horizontalInput != 0 || verticalInput != 0)
        {
            _playerThrustersPrefab.SetActive(true);
            _playerThrustersPrefab.transform.localScale = new Vector3(thrustersScale, thrustersScale,thrustersScale);
        }
        else if (_speedBoostActive != true)
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

    IEnumerator RechargeThrusters()
    {
        yield return new WaitForSeconds(5f);
        while (_thrustPower < 100 && _canThrust == false)
        {
            _thrustPower = 100;
            _uiManager.UpdateThrustSlider(_thrustPower);
            _canThrust = true;
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
            _shieldHits--;

            switch (_shieldHits)
            {
                case 0:
                    _shieldActive = false;
                    _playerShieldPrefab.SetActive(false);
                    return;
                case 1:
                    _shieldHitColor.color = Color.red;
                    return;
                case 2:
                    _shieldHitColor.color = Color.green;
                    return;
                default:
                    break;
            }
          
            _shieldActive = false;
            _playerShieldPrefab.SetActive(false);
            return;
        }
        _playerLives -= 1;
        _uiManager.UpdateLives(_playerLives);
        _cameraShake.StartShaking();

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
        float thrustersScale = 0.5f;
        _speedBoostActive = true;
        _playerSpeed = 10;
        thrustersScale += 0.25f;
        _playerThrustersPrefab.SetActive(true);
        _playerThrustersPrefab.transform.localScale = new Vector3(thrustersScale, thrustersScale,thrustersScale);
 
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
        _shieldHitColor = _playerShieldPrefab.GetComponent<SpriteRenderer>();
        _shieldHitColor.color = Color.cyan;
        _shieldHits = 3;
        _shieldActive = true;
        _playerShieldPrefab.SetActive(true);
    }

    public void AmmoDrop()
    {
        _ammoCount = 15;
    }

    public void HealthPowerUp()
    {
        if (_playerLives == 1)
        {
            _playerLives++;
            _damageRight.SetActive(false);
            _uiManager.UpdateLives(_playerLives);
        }
        else if (_playerLives == 2)
        {
            _playerLives++;
            _damageLeft.SetActive(false);
            _uiManager.UpdateLives(_playerLives);
        }
        
    }

    public void UniBeamPowerUp()
    {
        _uniBeamActive = true;
        _canFireLaser = false;
        AudioSource.PlayClipAtPoint(_uniBeamSound, transform.position);
        _uniBeamPrefab.SetActive(true);
        StartCoroutine(UnibeamPowerDownRoutine());

    }

    IEnumerator UnibeamPowerDownRoutine()
    {
        while (_uniBeamActive == true)
        {
            yield return new WaitForSeconds(5.0f);
            _uniBeamActive = false;
            _canFireLaser = true;
            _uniBeamPrefab.SetActive(false);
        }
    }

    public void ScoreCalculator(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
