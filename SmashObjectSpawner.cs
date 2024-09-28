using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class SmashObjectSpawner : MonoBehaviour
{
    public GameObject[] smashablePrefabs;
    public float spawnInterval = 5f;
    public int maxObjects = 10;
    public Vector2 spawnArea = new Vector2(10, 5);
    public int targetScore = 100;
    public GameObject victoryPopup;
    public GameObject pausePopup;
    public string nextSceneName;
    public string gameOverSceneName;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifeText;
    public int timePenalty = 1;            // Valor a ser subtraído da vida por segundo
    public float penaltyInterval = 1f;     // Intervalo em segundos para subtração da vida
    public int scoreLimit = -5;
    public int maxLife = 100;              // Quantidade máxima de vida
    private int life;                      // Vida atual do jogador
    private int currentSpawnCount = 0;
    private int score = 0;
    private bool isPaused = false;

    void Start()
    {
        life = maxLife;

        // Inicia o spawn de objetos e o sistema de penalidade de tempo
        StartCoroutine(SpawnObjects());
        UpdateScoreText();
        UpdateLifeText();
        InvokeRepeating("ReduceLifeOverTime", penaltyInterval, penaltyInterval);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (currentSpawnCount < maxObjects)
            {
                Vector2 spawnPosition = new Vector2(Random.Range(-spawnArea.x, spawnArea.x), Random.Range(-spawnArea.y, spawnArea.y));

                int randomPrefabIndex = Random.Range(0, smashablePrefabs.Length);
                GameObject newObject = Instantiate(smashablePrefabs[randomPrefabIndex], spawnPosition, Quaternion.identity);

                currentSpawnCount++;

                ClickToSmash clickToSmash = newObject.GetComponent<ClickToSmash>();
                clickToSmash.OnDestroyed += OnObjectDestroyed;
                clickToSmash.spawner = this;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void OnObjectDestroyed(int pointValue, int lifeDamage, bool damagesLife)
    {
        if (damagesLife)
        {
            life -= lifeDamage;
            Debug.Log("Vida: " + life);
            UpdateLifeText();
        }
        else
        {
            score += pointValue;
            Debug.Log("Pontuação: " + score);
            UpdateScoreText();
        }

        currentSpawnCount--;
        CheckGameProgress();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    void UpdateLifeText()
    {
        if (lifeText != null)
        {
            lifeText.text = "Vida: " + life.ToString();
        }
    }

    void CheckGameProgress()
    {
        if (score >= targetScore)
        {
            ShowVictoryPopup();
        }
        else if (life <= 0)
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
    }

    // Função chamada repetidamente para diminuir a vida ao longo do tempo
    void ReduceLifeOverTime()
    {
        life -= timePenalty;
        UpdateLifeText();

        // Verifica se a vida acabou
        if (life <= 0)
        {
            CheckGameProgress();
        }
    }

    void ShowVictoryPopup()
    {
        victoryPopup.SetActive(true);
        Time.timeScale = 0;
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(nextSceneName);
    }

    void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        pausePopup.SetActive(true);
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pausePopup.SetActive(false);
        isPaused = false;
    }
}
