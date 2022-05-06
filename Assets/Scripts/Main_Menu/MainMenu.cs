using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewSinglePlayer()
    {
        SceneManager.LoadScene("Single_Player");
    }

    public void NewCo_Op()
    {
        SceneManager.LoadScene("Co_Op");
    }
}
