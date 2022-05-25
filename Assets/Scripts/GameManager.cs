using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour
{
    public bool _isCoop = false;
    private bool _gameOver = false;
    [SerializeField]
    private GameObject _pauseMenu;
    private Animator _pauseAnim;

    void Start() {
        _pauseAnim = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        if (!_pauseAnim)
        {
            Debug.LogError("Failed to find Animation Controller on Pause_Menu_Panel");
        }
    }

    void Update() {
        // Could probably shorten this logic, if this check fails, then its obviously co-op
        if (_gameOver)
        {
            if ((Input.GetKeyDown(KeyCode.R) || CrossPlatformInputManager.GetButtonDown("Fire")) && _isCoop == false)
            {
                SceneManager.LoadScene("Single_Player");
            }
            else if (Input.GetKeyDown(KeyCode.R) && _isCoop == true)
            {
                SceneManager.LoadScene("Co_Op");
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) || CrossPlatformInputManager.GetButtonDown("Pause"))
        {
            _pauseAnim.SetBool("isPaused", true);
            _pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void GameOver()
    {
        _gameOver = true;
    }

    public void Resume()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
