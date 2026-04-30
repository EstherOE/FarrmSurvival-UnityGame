
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Stats")]
    public int eggsCollected = 0;
    public int targetEggs = 10;
    public int score = 0;
    public int currentLevel = 1;

   [Header("Chicken System")]
   public GameObject chickenPrefab;
   public Transform [] spawnPoints;

   private List<GameObject> activeChicken= new List<GameObject>();
    [Header("Timer")]
    public float currentTime;
    private bool gameEnded = false;
    private bool gamePaused = false;

    [Header("UI")]
    public TextMeshProUGUI eggText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI winScoreText;

    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pausePanel;
    public GameObject EggCounterPanel;
    public GameObject ObjectivePanel;

    [Header("Stars")]
    public GameObject Heart1;
    public GameObject Heart2;
    public GameObject Heart3;

    [Header("Effects")]
    public ParticleSystem winParticles;

    [Header("Obstacles")]
    public GameObject[] level2Objects;
    public GameObject[] level3Objects;
    public GameObject[] level4Objects;
    public GameObject[] level5Objects;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        pausePanel.SetActive(false);

        LoadLevelData();
    }

   void SpawnChicken()
    {
        ClearChickens();
        int chickenCount= Mathf.Min(2+ currentLevel,6);
        for(int i=0; i<chickenCount; i++)
        {
          int rand= Random.Range(0, spawnPoints.Length);
          GameObject chicken= Instantiate(chickenPrefab, spawnPoints[rand].position, Quaternion.identity);    
          activeChicken.Add(chicken);
        }
    }

    void ClearChickens()
    {
        foreach(GameObject obj in activeChicken)
        {
            Destroy(obj);
        }
        activeChicken.Clear();
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

    public void CollectEgg(int amount = 1)
    {
        eggsCollected += amount;
        score += amount * 100;

        UpdateUI();

        if (eggsCollected >= targetEggs)
            WinGame();
    }

   public void RelocateRandomChicken()
    {
         if(activeChicken.Count==0) return;
         int c= Random.Range(0,activeChicken.Count);
         int p= Random.Range(0, spawnPoints.Length);
         activeChicken[c].transform.position= spawnPoints[p].position;
    }

    void UpdateUI()
    {
        eggText.text = "Eggs: " + eggsCollected + " / " + targetEggs;
        scoreText.text = "Score: " + score;
    }

    void LoadLevelData()
    {
        eggsCollected = 0;
        score = 0;
        gameEnded = false;

        winPanel.SetActive(false);
        losePanel.SetActive(false);
        EggCounterPanel.SetActive(true);

        targetEggs = 10 + (currentLevel * 2);
        currentTime = 60 + currentLevel;

        ActivateLevelObstacles();

        ShowObjective();
        UpdateUI();
        SpawnChicken();
    }

    void ActivateLevelObstacles()
    {
        DisableAllObstacles();

        if (currentLevel >= 2)
            foreach (GameObject obj in level2Objects) obj.SetActive(true);

        if (currentLevel >= 3)
            foreach (GameObject obj in level3Objects) obj.SetActive(true);

        if (currentLevel >= 4)
            foreach (GameObject obj in level4Objects) obj.SetActive(true);

        if (currentLevel >= 5)
            foreach (GameObject obj in level5Objects) obj.SetActive(true);
    }

    void DisableAllObstacles()
    {
        foreach (GameObject obj in level2Objects) obj.SetActive(false);
        foreach (GameObject obj in level3Objects) obj.SetActive(false);
        foreach (GameObject obj in level4Objects) obj.SetActive(false);
        foreach (GameObject obj in level5Objects) obj.SetActive(false);
    }

    void ShowObjective()
    {
        ObjectivePanel.SetActive(true);
        objectiveText.text = "Level " + currentLevel + "\nCollect " + targetEggs + " Eggs";
        Invoke(nameof(HideObjective), 3f);
    }

    void HideObjective()
    {
        ObjectivePanel.SetActive(false);
    }

    public void WinGame()
    {
        gameEnded = true;
        EggCounterPanel.SetActive(false);

        Heart1.SetActive(false);
        Heart2.SetActive(false);
        Heart3.SetActive(false);

        if (currentTime > 30)
        {
            Heart1.SetActive(true);
            Heart2.SetActive(true);
            Heart3.SetActive(true);
        }
        else if (currentTime > 15)
        {
            Heart1.SetActive(true);
            Heart2.SetActive(true);
        }
        else
        {
            Heart1.SetActive(true);
        }

        winScoreText.text = "Score: " + score;
        winPanel.SetActive(true);

        if (winParticles != null)
            winParticles.Play();

        Invoke(nameof(FreezeGame), 1f);
    }

    void FreezeGame()
    {
        Time.timeScale = 0f;
    }

    public void LoseGame()
    {
        gameEnded = true;
        EggCounterPanel.SetActive(false);
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
        LoadLevelData();
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        currentLevel++;
        LoadLevelData();
    }

    public void MainMenu(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
