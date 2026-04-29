using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Stats")]
    public int eggsCollected = 0;
    public int targetEggs = 10;
    public int score = 0;

    [Header("Timer")]
    public float levelTime = 60f;
    private float currentTime;
    private bool gameEnded = false;
    private bool gamePaused = false;

    [Header("UI")]
    public TextMeshProUGUI eggText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pausePanel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentTime = levelTime;
        UpdateUI();

        winPanel.SetActive(false);
        losePanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    void Update()
    {
        if (gameEnded || gamePaused) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            LoseGame();
        }

        timerText.text = "Time: " + Mathf.CeilToInt(currentTime);
    }

    // Called when player collects egg
    public void CollectEgg(int amount = 1)
    {
        eggsCollected += amount;
        score += amount * 100;

        UpdateUI();

        if (eggsCollected >= targetEggs)
        {
            WinGame();
        }
    }

    void UpdateUI()
    {
        eggText.text = "Eggs: " + eggsCollected + " / " + targetEggs;
        scoreText.text = "Score: " + score;
    }

    public void WinGame()
    {
        gameEnded = true;
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void LoseGame()
    {
        gameEnded = true;
        losePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void PauseGame()
    {
        gamePaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        gamePaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}