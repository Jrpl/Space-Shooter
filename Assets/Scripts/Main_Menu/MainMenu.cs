using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
#if MOBILE_INPUT
    private GameObject _coopButton;
    void Start() {
        _coopButton = GameObject.Find("Co_Op_Button");
        _coopButton.SetActive(false);
    }
#endif

    public void NewSinglePlayer()
    {
        SceneManager.LoadScene("Single_Player");
    }

    public void NewCo_Op()
    {
        SceneManager.LoadScene("Co_Op");
    }
}
