using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private Image _livesImg;
    [SerializeField] private TMP_Text _gameOverTxt;
    [SerializeField] private TMP_Text _restartTxt;
    [SerializeField] private Slider _thrustSlider;
    [SerializeField] private TMP_Text _waveTxt;
    [SerializeField] private TMP_Text _missileCountText;
    [SerializeField] private TMP_Text _winText;
    private GameManager _gameManager;
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoText.text = "Ammo: " + 15 + "/15";
        _gameOverTxt.gameObject.SetActive(false);
        _waveTxt.gameObject.SetActive(false);
        _ammoText.color = Color.green;
        _missileCountText.text = "Missiles: " + 3;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.Log("Game Manager is NULL");
        }


    }

    public void UpdateScore(int scoreCount)
    {
        _scoreText.text = "Score: " + scoreCount.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _livesSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }

    }
  

    public void UpdateThrustSlider(float charge)
    {
        _thrustSlider.value = charge;
    }

    private void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverTxt.gameObject.SetActive(true);
        _restartTxt.gameObject.SetActive(true);

        StartCoroutine(GameOverFlickerRoutine());
    }
    
    public void UpdateMissileCount(int missileCount)
    {
        _missileCountText.text = "Missiles: " + missileCount.ToString();
        if (missileCount <= 0)
        {
            _missileCountText.color = Color.grey;
        }
    }

    public void UpdateAmmoCount(int ammoCount)
    {
        _ammoText.text = "Ammo: " + ammoCount.ToString() + "/15";
        if (ammoCount <= 0)
        {
            _ammoText.color = Color.grey;
        }
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverTxt.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverTxt.text = " ";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ShowWaveNumber(int waveNumber)
    {
        _waveTxt.text = "Wave: " + waveNumber;
        StartCoroutine(FadeWaveNumberText());
    }

    IEnumerator FadeWaveNumberText()
    {
        _waveTxt.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        _waveTxt.gameObject.SetActive(false);;
    }
    
    public void SetLaserMode()
    {
        _ammoText.color = Color.green;
        _missileCountText.color = Color.white;
    }

    public void SetMissileMode()
    {
        _ammoText.color = Color.white;
        _missileCountText.color = Color.green;
    }

    public void GameWonDisplay()
    {
        _winText.gameObject.SetActive(true);
    }

    

}
