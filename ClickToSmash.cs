using UnityEngine;

public class ClickToSmash : MonoBehaviour
{
    public Sprite destroyedSprite;         // Sprite que representa o estado destruído
    private SpriteRenderer spriteRenderer; // Componente de renderização de sprite
    private bool isDestroyed = false;      // Verifica se o objeto já foi destruído
    public int pointValue = 1;             // Valor de pontos ao clicar (definido no prefab individualmente)
    public float autoDestructTime = 10f;   // Tempo de auto-destruição (pode ser ajustado no Inspector)
    public SmashObjectSpawner spawner;     // Referência ao spawner para atualizar a pontuação
    public float minSpeed = 1f;            // Velocidade mínima de movimento
    public float maxSpeed = 5f;            // Velocidade máxima de movimento
    public bool damagesLife = false;       // Indica se este objeto deve subtrair vida
    public int lifeDamage = 5;             // Quantidade de vida subtraída (se for um objeto que subtrai vida)

    private Rigidbody2D rb;                // Componente de física para o movimento

    // Modificação: evento aceita dois parâmetros a mais para vida e se afeta a vida
    public delegate void ObjectDestroyed(int pointValue, int lifeDamage, bool damagesLife);
    public event ObjectDestroyed OnDestroyed;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer não encontrado! Adicione um SpriteRenderer ao objeto.");
        }

        if (destroyedSprite == null)
        {
            Debug.LogError("Sprite destruído não atribuído! Arraste o sprite no campo destroyedSprite no Inspector.");
        }

        // Inicia a auto-destruição após o tempo especificado
        Invoke("AutoDestroy", autoDestructTime);

        // Adiciona movimento aleatório ao objeto
        AddRandomMovement();
    }

    void OnMouseDown()
    {
        // Adicionando verificação para pausar o clique se o jogo estiver pausado
        if (Time.timeScale == 0)
        {
            return; // Não permite cliques enquanto o jogo está pausado
        }

        // Verifica se o objeto já foi destruído e se ainda tem o sprite
        if (!isDestroyed && spriteRenderer != null && destroyedSprite != null)
        {
            spriteRenderer.sprite = destroyedSprite;
            isDestroyed = true;

            // Notifica o spawner se o objeto foi destruído manualmente, passando os valores
            if (OnDestroyed != null)
            {
                OnDestroyed.Invoke(pointValue, lifeDamage, damagesLife);
            }

            // Destrói o objeto após um pequeno atraso
            Destroy(gameObject, 0.5f);
        }
    }

    void AutoDestroy()
    {
        if (!isDestroyed)
        {
            isDestroyed = true;

            // Notifica o spawner que o objeto foi destruído sem ganhar ou perder pontos
            if (OnDestroyed != null)
            {
                OnDestroyed.Invoke(0, 0, false); // Nenhum ponto e sem dano à vida
            }

            // Destrói o objeto após um pequeno atraso
            Destroy(gameObject);
        }
    }

    void AddRandomMovement()
    {
        if (rb != null)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float randomSpeed = Random.Range(minSpeed, maxSpeed);
            rb.velocity = randomDirection * randomSpeed;
        }
    }
}
