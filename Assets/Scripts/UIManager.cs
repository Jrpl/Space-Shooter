using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _bestText;
    private int _highScore;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Image _healthImg;
    [SerializeField]
    private Sprite[] _healthDisplaySprites;
    private GameManager _gameManager;

    void Start()
    {
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
        _bestText.text = "Best: " + _highScore;
        _scoreText.text = "Current: " + 0;
        _gameOverText.gameObject.SetActive(false);
#if MOBILE_INPUT
        _restartText.text = "Press 'FIRE' to restart";
#endif
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (!_gameManager)
        {
            Debug.LogError("Failed to find Game Object: Game_Manager");
        }
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Current: " + score;
    }

    public void UpdateHighScore(int highScore)
    {
        _bestText.text = "Best: " + highScore;
    }

    public void UpdateHealthDisplay(int currentHealth)
    {
        _healthImg.sprite = _healthDisplaySprites[currentHealth];

        if (currentHealth == 0) {
            GameOverSequence();   
        }
    }

    public void Resume()
    {
        _gameManager.Resume();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
        Time.timeScale = 1;
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
