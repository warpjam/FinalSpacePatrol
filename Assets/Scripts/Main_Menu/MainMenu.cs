using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene(1); //game scene
    }

    public void LoadManual()
    {
        SceneManager.LoadScene(2);//Manual scene
    }
}
