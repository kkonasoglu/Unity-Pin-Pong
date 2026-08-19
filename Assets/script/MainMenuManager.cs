using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static bool IsAI = false;  
    public void PlayPVP()
    {
        IsAI = false;
        SceneManager.LoadScene("GameScene");
    }  

    public void PlayPVE()
    {
        IsAI = true;
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
