using UnityEngine;
using UnityEngine.UI;  // Para trabajar con imágenes de fondo

public class LevelManager : MonoBehaviour
{
    // Prefab de la burbuja que se instanciará al iniciar el nivel.
    public GameObject bubblePrefab;  
    // Panel que se muestra cuando se completa un nivel.
    public GameObject levelCompletePanel;  
    // Arreglo de sprites para cambiar el fondo en cada nivel.
    public Sprite[] backgrounds;  
    // Componente SpriteRenderer que muestra el fondo.
    public SpriteRenderer backgroundImage;  

    // Nivel actual del juego.
    private int currentLevel = 1;  
    // Nivel máximo definido para este proyecto.
    private int maxLevel = 3;  
    // Indica si el nivel está activo.
    private bool levelActive = true;  

    void Start()
    {
        // Verifica que todas las referencias necesarias estén asignadas.
        if (levelCompletePanel == null || backgroundImage == null || backgrounds.Length == 0)
        {
            Debug.LogError("Faltan referencias en el LevelManager. Verifica que todo esté correctamente asignado.");
            return;
        }

        // Oculta el panel de nivel completado al inicio.
        levelCompletePanel.SetActive(false);  
        StartLevel();  // Inicia el primer nivel.
    }

    // Inicia el nivel actual y configura su entorno.
    public void StartLevel()
    {
        Debug.Log("Iniciando nivel " + currentLevel);

        // Cambia el fondo de acuerdo al nivel actual.
        if (backgrounds.Length >= currentLevel)
        {
            backgroundImage.sprite = backgrounds[currentLevel - 1];
            Debug.Log("Fondo cambiado para el nivel " + currentLevel);
        }
        else
        {
            Debug.LogWarning("No hay fondo asignado para el nivel " + currentLevel);
        }

        // Determina cuántas burbujas generar (1 burbuja en el nivel 1, 2 en el nivel 2, etc.).
        int bubbleCount = 1 + currentLevel;  

        // Genera las burbujas en posiciones aleatorias dentro de un rango.
        for (int i = 0; i < bubbleCount; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), 5f, 0);
            GameObject bubble = Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);

            // Asocia el LevelManager a cada burbuja para que pueda notificar cuando se destruye.
            if (bubble.TryGetComponent(out Bubble bubbleScript))
            {
                bubbleScript.levelManager = this;
            }
            else
            {
                Debug.LogError("El prefab de la burbuja no tiene el componente Bubble.");
            }
        }

        levelActive = true;  // Marca el nivel como activo.
    }

    // Llamado cada vez que una burbuja es destruida.
    public void BubbleDestroyed()
    {
        if (!levelActive) return;  // Si el nivel no está activo, no hace nada.

        // Verifica si quedan burbujas después de un pequeño retraso.
        Invoke("CheckBubblesRemaining", 0.1f);
    }

    // Verifica si quedan burbujas en la escena.
    public void CheckBubblesRemaining()
    {
        // Busca todas las burbujas activas.
        Bubble[] bubbles = Object.FindObjectsByType<Bubble>(FindObjectsSortMode.None);

        // Si no quedan burbujas y el jugador aún tiene vidas, se completa el nivel.
        if (bubbles.Length == 0 && levelActive && GameManager.instance.lives > 0)
        {
            CompleteLevel();
        }
    }

    // Maneja la finalización del nivel.
    private void CompleteLevel()
    {
        Debug.Log("¡Nivel " + currentLevel + " completado!");
        levelActive = false;  // Desactiva el nivel.
        levelCompletePanel.SetActive(true);  // Muestra el panel de nivel completado.
        Time.timeScale = 0;  // Detiene el tiempo del juego.
    }

    // Pasa al siguiente nivel o termina el juego si se ha completado el último nivel.
    public void NextLevel()
    {
        Time.timeScale = 1;  // Reanuda el tiempo.

        if (currentLevel < maxLevel)
        {
            currentLevel++;  // Avanza al siguiente nivel.
            levelCompletePanel.SetActive(false);  // Oculta el panel de nivel completado.
            StartLevel();  // Inicia el nuevo nivel.
        }
        else
        {
            Debug.Log("¡Juego completado!");  // Mensaje de finalización del juego.
            // Aquí podrías implementar una pantalla de victoria o final.
        }
    }

    /// <summary>
    /// Este método reinicia el juego al nivel inicial.
    /// Se llama cuando el jugador selecciona "Jugar de nuevo" después de un Game Over.
    /// </summary>
    public void ResetAndStartLevel()
    {
        // Elimina todas las burbujas existentes en la escena.
        Bubble[] bubbles = Object.FindObjectsByType<Bubble>(FindObjectsSortMode.None);
        foreach (Bubble bubble in bubbles)
        {
            Destroy(bubble.gameObject);
        }

        currentLevel = 1;  // Reinicia al primer nivel.
        levelCompletePanel.SetActive(false);  // Oculta el panel de nivel completado.
        Time.timeScale = 1;  // Asegúrate de que el tiempo esté activo.
        StartLevel();  // Inicia el primer nivel.
    }

    private void OnDestroy()
    {
        Debug.Log("Limpiando referencias del LevelManager.");  // Mensaje de depuración al destruir el objeto.
    }
}
