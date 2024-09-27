using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameover : MonoBehaviour
{
    public Image gameOverImage; // Referência para a imagem de Game Over
    public float fadeDuration = 2f; // Duração do fade-out
    public float delayBeforeFade = 3f; // Tempo antes do fade-out começar
    public string nextScene = "main_menu"; // Nome da cena para carregar após o fade

    void Start()
    {
        StartCoroutine(HandleGameOverScreen());
    }

    IEnumerator HandleGameOverScreen()
    {
        // Espera por alguns segundos antes de começar o fade
        yield return new WaitForSeconds(delayBeforeFade);

        // Inicia o fade-out
        yield return StartCoroutine(FadeOutImage());

        // Carrega a próxima cena após o fade-out
        SceneManager.LoadScene(nextScene);
    }

    IEnumerator FadeOutImage()
    {
        float elapsedTime = 0f;
        Color imageColor = gameOverImage.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            // Calcula a nova transparência (alfa) ao longo do tempo
            imageColor.a = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            gameOverImage.color = imageColor;
            yield return null;
        }
    }
}
