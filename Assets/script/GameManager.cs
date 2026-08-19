using UnityEngine;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI text")]
    public TextMeshProUGUI p1ScoreText;
    public TextMeshProUGUI p2ScoreText;
    public TextMeshProUGUI winText;

    [Header("Game Settings")]
    public int maxscore = 5;
    public bool isGameOver = false;

    private int p1Score = 0;
    private int p2Score = 0;

    public GameObject rigthPaddle;
    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {

        if (MainMenuManager.IsAI)
        {
            rigthPaddle.GetComponent<MoveScript>().enabled = false;

            if(rigthPaddle.GetComponent<AIPaddle>() == null)
            rigthPaddle.AddComponent<AIPaddle>();
            else rigthPaddle.GetComponent<AIPaddle>().enabled = true;
        }
        else
        {
            rigthPaddle.GetComponent<MoveScript>().enabled = true;
            if(rigthPaddle.GetComponent<AIPaddle>() != null)
            rigthPaddle.GetComponent<AIPaddle>().enabled = false;
        }

        
        if(winText != null)
        {
            winText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void Player1Scored()
    {
        if(isGameOver) return;
        p1Score++;
        p1ScoreText.text= p1Score.ToString();
        CheckWinCondition();
    }

      public void Player2Scored()
    {
        if(isGameOver) return;
        p2Score++;
        p2ScoreText.text= p2Score.ToString();

        CheckWinCondition();
    }

    public void CheckWinCondition()
    {
        if(p1Score >= maxscore)
        {
            EndGame("PLAYER 1 WİN!!!  press 'R' for restart..");
        }
        else if(p2Score >= maxscore)
        {
            EndGame("PLAYER 2 WİN!!! 'R' for restart..");
        }
    }

    void EndGame(string massage)
    {
        isGameOver = true;
        if(winText != null)
        {
            winText.text = massage;
            winText.gameObject.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    

}
