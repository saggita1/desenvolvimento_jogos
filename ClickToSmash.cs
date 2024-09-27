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

    private Rigidbody2D rb;                // Componente de física para o movimento

    // Evento para notificar quando o objeto é destruído
    public delegate void ObjectDestroyed(int pointValue);
    public event ObjectDestroyed OnDestroyed;

    void Start()
    {
        // Tenta pegar o SpriteRenderer do objeto atual
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>(); // Obtém o Rigidbody2D para aplicar o movimento

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
        if (!isDestroyed && spriteRenderer != null && destroyedSprite != null)
        {
            // Trocar para o sprite de destruído
            spriteRenderer.sprite = destroyedSprite;

            // Define que o objeto foi destruído para evitar múltiplos cliques
            isDestroyed = true;

            // Notifica o spawner que o objeto foi destruído, passando o valor de pontos
            if (OnDestroyed != null)
            {
                OnDestroyed.Invoke(pointValue);
            }

            // Destrói o objeto após um pequeno delay
            Destroy(gameObject, 0.5f);
        }
    }

    // Função para auto-destruição
    void AutoDestroy()
    {
        if (!isDestroyed)
        {
            // Se o objeto não foi destruído manualmente, apenas destrua sem mudar o sprite
            isDestroyed = true;

            // Notifica o spawner que o objeto foi destruído sem ganhar ou perder pontos
            if (OnDestroyed != null)
            {
                OnDestroyed.Invoke(0);  // Nenhum ponto é adicionado ou subtraído
            }

            // Destrói o objeto e remove da cena
            Destroy(gameObject);
        }
    }

    // Função para adicionar movimento aleatório
    void AddRandomMovement()
    {
        if (rb != null)
        {
            // Gera uma direção aleatória
            Vector2 randomDirection = Random.insideUnitCircle.normalized;

            // Gera uma velocidade aleatória entre minSpeed e maxSpeed
            float randomSpeed = Random.Range(minSpeed, maxSpeed);

            // Aplica a velocidade ao Rigidbody2D
            rb.velocity = randomDirection * randomSpeed;
        }
    }
}
