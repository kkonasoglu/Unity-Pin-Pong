using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    public GameObject difficultyPanel;

    [Header("References")]
    public AIPaddle aIPaddle;
    public ball ballScript;
    public GameObject rightPaddle;

    [Header("UI Text")]
    public TextMeshProUGUI p1ScoreText;
    public TextMeshProUGUI p2ScoreText;
    public TextMeshProUGUI winText;

    [Header("Game Settings")]
    public int maxScore = 5;
    public bool isGameOver = false;

    private int p1Score = 0;
    private int p2Score = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
{
    isGameOver = false;

    if (winText != null)
    {
        winText.gameObject.SetActive(false);
    }

    if (MainMenuManager.IsAI)
    {
        // PvE Modu: Paneli aç ve ZAMANI DURDUR
        if (difficultyPanel != null) difficultyPanel.SetActive(true);
        Time.timeScale = 0f; // Oyun ve top tamamen donsun

        if (rightPaddle != null)
        {
            var moveScript = rightPaddle.GetComponent<MoveScript>();
            if (moveScript != null) moveScript.enabled = false;

            aIPaddle = rightPaddle.GetComponent<AIPaddle>();
            if (aIPaddle == null) aIPaddle = rightPaddle.AddComponent<AIPaddle>();
            aIPaddle.enabled = false;
        }
    }
    else
    {
        // PvP Modu: Zamanı başlat ve oyuna gir
        Time.timeScale = 1f;
        if (difficultyPanel != null) difficultyPanel.SetActive(false);

        if (rightPaddle != null)
        {
            var moveScript = rightPaddle.GetComponent<MoveScript>();
            if (moveScript != null) moveScript.enabled = true;

            var aiComp = rightPaddle.GetComponent<AIPaddle>();
            if (aiComp != null) aiComp.enabled = false;
        }

        StartGame();
    }
}

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    // --- ZORLUK BUTONLARININ TETİKLEYECEĞİ FONKSİYONLAR ---

    public void SelectEasy()
    {
        SetDifficultyAndStart(AIDifficulty.Easy);
    }

    public void SelectMedium()
    {
        SetDifficultyAndStart(AIDifficulty.Medium);
    }

    public void SelectHard()
    {
        SetDifficultyAndStart(AIDifficulty.Hard);
    }

    void SetDifficultyAndStart(AIDifficulty difficulty)
{
    if (aIPaddle != null)
    {
        aIPaddle.enabled = true;
        aIPaddle.SetDifficulty(difficulty);
    }

    // Paneli gizle
    if (difficultyPanel != null)
    {
        difficultyPanel.SetActive(false);
    }

    // ZAMANI BAŞLAT
    Time.timeScale = 1f;
    StartGame();
}

    public void StartGame()
    {
        Time.timeScale = 1f;
        
        // Eğer topun başlangıçta fırlatılmasını GameManager yönetiyorsa:
        if (ballScript != null)
        {
            // ballScript.LaunchBall(); // ball.cs içinde public ise doğrudan çağırabilirsin
        }
    }

    // --- SKOR VE OYUN DÖNGÜSÜ ---

    public void Player1Scored()
    {
        SoundManager.Instance.PlayScore();
        
        if (isGameOver) return;
        p1Score++;
        if (p1ScoreText != null) p1ScoreText.text = p1Score.ToString();
        CheckWinCondition();
    }

    public void Player2Scored()
    {
        SoundManager.Instance.PlayScore();
        if (isGameOver) return;
        p2Score++;
        if (p2ScoreText != null) p2ScoreText.text = p2Score.ToString();
        CheckWinCondition();
    }

    public void CheckWinCondition()
    {
        if (p1Score >= maxScore)
        {
            EndGame("PLAYER 1 WINS!\nPress 'R' to Restart");
        }
        else if (p2Score >= maxScore)
        {
            string winner = MainMenuManager.IsAI ? "AI BOT WINS!" : "PLAYER 2 WINS!";
            EndGame(winner + "\nPress 'R' to Restart");
        }
    }

    void EndGame(string message)
    {
        SoundManager.Instance.PlayWin();
        isGameOver = true;
        if (winText != null)
        {
            winText.text = message;
            winText.gameObject.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}