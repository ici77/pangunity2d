using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // Para manejar el texto de vidas

public class GameManager : MonoBehaviour
{
    // Instancia única del GameManager (patrón Singleton).
    public static GameManager instance;  
    // Cantidad de vidas iniciales.
    public int lives = 5;  
    // Referencia al panel que se muestra cuando se acaba el juego.
    public GameObject gameOverPanel;  
    // Referencia al texto en pantalla que muestra las vidas.
    public TextMeshProUGUI livesText;  

    void Awake()
    {
        // Implementación del patrón Singleton para evitar duplicados.
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);  // Destruye el GameManager duplicado.
        }
    }

    void Start()
    {
        UpdateLivesText();  // Actualiza el texto de las vidas al inicio.

        // Asegúrate de que el panel de Game Over esté oculto al inicio del juego.
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    // Método para reducir la vida del jugador.
    public void LoseLife()
    {
        if (lives > 0)
        {
            lives--;  // Reduce una vida.
            UpdateLivesText();  // Actualiza el texto en pantalla.

            // Si las vidas llegan a 0 o menos, muestra la pantalla de Game Over.
            if (lives <= 0)
            {
                ShowGameOver();
            }
        }
    }

    // Actualiza el texto de las vidas en pantalla.
    public void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = "Vidas: " + Mathf.Max(0, lives);  // Asegura que no muestre valores negativos.
        }
    }

    // Muestra la pantalla de Game Over y pausa el juego.
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);  // Muestra el panel de Game Over.
            Time.timeScale = 0f;  // Detiene el tiempo en el juego.
        }
    }

    // Reinicia el juego y el nivel actual.
    public void RestartGame()
    {
        Time.timeScale = 1f;  // Reanuda el tiempo del juego.
        lives = 5;  // Restablece las vidas a 5.
        UpdateLivesText();  // Muestra las vidas restablecidas.

        // Oculta el panel de Game Over si está activo.
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Busca el LevelManager para reiniciar el nivel.
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null)
        {
            // Oculta el panel de nivel completado, si está activo.
            if (levelManager.levelCompletePanel != null)
            {
                levelManager.levelCompletePanel.SetActive(false);
            }

            levelManager.ResetAndStartLevel();  // Reinicia y comienza el nivel.
        }
        else
        {
            // Si no se encuentra el LevelManager, recarga la escena actual.
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
