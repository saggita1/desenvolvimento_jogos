using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class SmashObjectSpawner : MonoBehaviour
{
    public GameObject[] smashablePrefabs;  // Array de prefabs (vários tipos de objetos clicáveis)
    public float spawnInterval = 5f;       // Intervalo de tempo entre os spawns (pode ser ajustado no Inspector)
    public int maxObjects = 10;            // Número máximo de objetos ativos na cena
    public Vector2 spawnArea = new Vector2(10, 5); // Área de spawn (X, Y)
    public int targetScore = 100;          // Pontuação necessária para passar de fase
    public GameObject victoryPopup;        // Referência ao painel do pop-up de vitória
    public GameObject pausePopup;          // Referência ao painel do pop-up de pausa
    public string nextSceneName;           // Nome da próxima cena
    public string gameOverSceneName;       // Nome da cena de Game Over
    public TextMeshProUGUI scoreText;      // Referência ao TextMeshProUGUI que exibirá a pontuação
    public int timePenalty = 1;            // Valor a ser subtraído por segundo
    public float penaltyInterval = 1f;     // Intervalo em segundos para subtração do score
    public int scoreLimit = -5;            // Limite de score para Game Over

    private int currentSpawnCount = 0;     // Número atual de objetos ativos na cena
    private int score = 0;                 // Sistema de pontuação
    private bool isPaused = false;         // Indica se o jogo está pausado ou não

    void Start()
    {
        // Inicia a coroutine para spawnar objetos repetidamente
        StartCoroutine(SpawnObjects());

        // Inicializa o texto da pontuação
        UpdateScoreText();

        // Inicia a chamada repetida da função para reduzir a pontuação a cada segundo
        InvokeRepeating("ReduceScoreOverTime", penaltyInterval, penaltyInterval);
    }

    void Update()
    {
        // Detecta se a tecla de pausa (P ou Esc) foi pressionada
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();  // Alterna entre pausar e retomar o jogo
        }
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (currentSpawnCount < maxObjects)
            {
                // Gera uma posição aleatória dentro da área especificada
                Vector2 spawnPosition = new Vector2(Random.Range(-spawnArea.x, spawnArea.x), Random.Range(-spawnArea.y, spawnArea.y));

                // Escolhe aleatoriamente um prefab para spawnar
                int randomPrefabIndex = Random.Range(0, smashablePrefabs.Length);
                GameObject newObject = Instantiate(smashablePrefabs[randomPrefabIndex], spawnPosition, Quaternion.identity);

                // Aumenta a contagem de objetos ativos
                currentSpawnCount++;

                // Subscrição para evento de destruição e atribuição de pontos
                ClickToSmash clickToSmash = newObject.GetComponent<ClickToSmash>();
                clickToSmash.OnDestroyed += OnObjectDestroyed;

                // Passa a referência do spawner para o script do objeto para atualizar a pontuação
                clickToSmash.spawner = this;
            }

            // Aguarda o intervalo de tempo antes de tentar criar mais objetos
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Função chamada quando um objeto é destruído
    void OnObjectDestroyed(int pointValue)
    {
        // Atualiza a pontuação
        score += pointValue;
        Debug.Log(score);

        // Atualiza o texto da pontuação no UI
        UpdateScoreText();

        // Decrementa a contagem de objetos ativos
        currentSpawnCount--;

        // Verifica se o jogador atingiu a pontuação necessária para passar de fase
        CheckGameProgress();
    }

    void UpdateScoreText()
    {
        // Atualiza o texto do TextMeshProUGUI com a pontuação atual
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    void CheckGameProgress()
    {
        // Se a pontuação atingir ou ultrapassar a pontuação necessária, mostra o pop-up de vitória
        if (score >= targetScore)
        {
            Debug.Log("Passou de fase!");
            ShowVictoryPopup();  // Chama o método para exibir o pop-up de vitória
        }
        else if (score < scoreLimit)
        {
            // Se a pontuação for menor que o limite, vai para o Game Over
            Debug.Log("Game Over!");
            SceneManager.LoadScene(gameOverSceneName);  // Carrega a cena de Game Over
        }
    }

    // Função para mostrar o pop-up de vitória e pausar o jogo
    void ShowVictoryPopup()
    {
        victoryPopup.SetActive(true);  // Ativa o painel do pop-up de vitória
        Time.timeScale = 0;           // Pausa o jogo
    }

    // Função para carregar a próxima fase, chamada pelo botão no pop-up
    public void LoadNextLevel()
    {
        Time.timeScale = 1;  // Restaura o tempo antes de carregar a próxima fase
        SceneManager.LoadScene(nextSceneName);  // Carrega a próxima cena
    }

    // Função chamada repetidamente para diminuir a pontuação ao longo do tempo
    void ReduceScoreOverTime()
    {
        score -= timePenalty;
        UpdateScoreText();
    }

    // Alterna entre pausar e retomar o jogo
    void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();  // Se já está pausado, retoma o jogo
        }
        else
        {
            PauseGame();   // Se não está pausado, pausa o jogo
        }
    }

    // Função para pausar o jogo e exibir o pop-up de pausa
    void PauseGame()
    {
        Time.timeScale = 0;  // Pausa o jogo
        pausePopup.SetActive(true);  // Exibe o pop-up de pausa
        isPaused = true;  // Marca que o jogo está pausado
    }

    // Função para retomar o jogo a partir do pop-up de pausa
    public void ResumeGame()
    {
        Time.timeScale = 1;  // Retoma o jogo
        pausePopup.SetActive(false);  // Esconde o pop-up de pausa
        isPaused = false;  // Marca que o jogo não está mais pausado
    }
}
