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
    private GameManager _gameManager;
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoText.text = "Ammo: " + 15 + "/15";
        _gameOverTxt.gameObject.SetActive(false);
        _waveTxt.gameObject.SetActive(false);
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

    public void UpdateAmmoCount(int ammoCount)
    {
        _ammoText.text = "Ammo: " + ammoCount.ToString() + "/15";
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
        _waveTxt.CrossFadeAlpha(0.0f, 1.0f, false);
    }
}
