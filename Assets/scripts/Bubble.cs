using UnityEngine;

public class Bubble : MonoBehaviour
{
    // Velocidad base de la burbuja.
    public float speed = 5f;  
    // Fuerza del rebote cuando la burbuja toca una superficie.
    public float bounceStrength = 8f;  

    // Referencia al componente Rigidbody2D para controlar el movimiento.
    private Rigidbody2D rb;  
    // Dirección inicial de movimiento de la burbuja.
    private Vector2 initialDirection;  

    // Número de veces que la burbuja se ha dividido.
    public int divisionCount = 0;  
    // Máximo número de divisiones permitidas antes de destruir la burbuja.
    public int maxDivisions = 2;  

    // Referencia al LevelManager para notificar cuando una burbuja es destruida.
    public LevelManager levelManager;  

    // Límite horizontal en el que la burbuja puede moverse antes de rebotar.
    private float horizontalLimit = 13f;  

    // Sonido que se reproduce cuando la burbuja explota.
    public AudioClip bubblePopSound;  

    // Previene la destrucción múltiple de la misma burbuja.
    public bool isDestroyed = false;  

    void Start()
    {
        // Obtiene el componente Rigidbody2D de la burbuja.
        rb = GetComponent<Rigidbody2D>();

        // Establece una dirección inicial aleatoria (hacia la izquierda o derecha).
        initialDirection = new Vector2(Random.Range(0, 2) == 0 ? 1 : -1, 1).normalized;
        rb.linearVelocity = initialDirection * speed;

        // Ignora las colisiones entre burbujas para evitar comportamientos extraños.
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bubble"), LayerMask.NameToLayer("Bubble"));
    }

    void Update()
    {
        // Verifica si la burbuja ha alcanzado los límites horizontales.
        if (transform.position.x <= -horizontalLimit)
        {
            // Corrige la posición y rebota hacia el lado contrario.
            transform.position = new Vector3(-horizontalLimit, transform.position.y, transform.position.z);
            rb.linearVelocity = new Vector2(Mathf.Abs(rb.linearVelocity.x), rb.linearVelocity.y);
        }
        else if (transform.position.x >= horizontalLimit)
        {
            // Corrige la posición y rebota hacia el lado contrario.
            transform.position = new Vector3(horizontalLimit, transform.position.y, transform.position.z);
            rb.linearVelocity = new Vector2(-Mathf.Abs(rb.linearVelocity.x), rb.linearVelocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Si la burbuja choca con el proyectil y no ha sido destruida previamente.
        if (collision.gameObject.CompareTag("proyectil") && !isDestroyed)
        {
            isDestroyed = true;  // Marca la burbuja como destruida.
            HandleDivisionOrDestruction();  // Maneja la división o destrucción.
        }
        // Si choca con el suelo, rebota hacia arriba.
        else if (collision.gameObject.CompareTag("suelo"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceStrength);
            if (divisionCount < maxDivisions)
            {
                HandleDivisionOrDestruction();  // Maneja la división si no ha alcanzado el límite.
            }
        }
        // Si choca con el jugador, el jugador pierde una vida y la burbuja rebota.
        else if (collision.gameObject.CompareTag("jugador"))
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.LoseLife();  // Notifica la pérdida de vida.
            }

            // Rebota en dirección opuesta al jugador.
            Vector2 direccionRebote = (transform.position - collision.transform.position).normalized;
            rb.linearVelocity = direccionRebote * bounceStrength;
        }
        // Si choca con el techo, invierte su dirección vertical.
        else if (collision.gameObject.CompareTag("techo"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -bounceStrength);
        }
    }

    // Decide si la burbuja se divide o es destruida.
    public void HandleDivisionOrDestruction()
    {
        if (divisionCount < maxDivisions)
        {
            DivideBubble();  // Se divide si no ha alcanzado el límite.
        }
        else
        {
            DestroyOnProjectile();  // Se destruye si ha alcanzado el límite de divisiones.
        }
    }

    // Destruye la burbuja al ser golpeada por el proyectil.
    public void DestroyOnProjectile()
    {
        PlayBubblePopSound();  // Reproduce el sonido de explosión.

        if (levelManager != null)
        {
            levelManager.BubbleDestroyed();  // Notifica al LevelManager.
        }

        Destroy(gameObject);  // Destruye el objeto.
    }

    // Reproduce el sonido de estallido de la burbuja.
    private void PlayBubblePopSound()
    {
        if (bubblePopSound != null)
        {
            AudioSource.PlayClipAtPoint(bubblePopSound, transform.position, 1f);
        }
    }

    // Divide la burbuja en dos burbujas más pequeñas.
    public void DivideBubble()
    {
        PlayBubblePopSound();  // Reproduce el sonido de explosión.
        float separationOffset = 3f;  // Distancia entre las burbujas resultantes.

        // Crea dos burbujas nuevas.
        GameObject bubble1 = Instantiate(gameObject, transform.position + new Vector3(-separationOffset, 0, 0), Quaternion.identity);
        GameObject bubble2 = Instantiate(gameObject, transform.position + new Vector3(separationOffset, 0, 0), Quaternion.identity);

        // Reduce el tamaño de las burbujas nuevas.
        bubble1.transform.localScale *= 0.5f;
        bubble2.transform.localScale *= 0.5f;

        // Actualiza el conteo de divisiones de las nuevas burbujas.
        bubble1.GetComponent<Bubble>().divisionCount = divisionCount + 1;
        bubble2.GetComponent<Bubble>().divisionCount = divisionCount + 1;

        // Asocia las burbujas al mismo LevelManager.
        bubble1.GetComponent<Bubble>().levelManager = levelManager;
        bubble2.GetComponent<Bubble>().levelManager = levelManager;

        // Establece velocidades iniciales para las nuevas burbujas.
        bubble1.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-speed + Random.Range(-1f, 1f), bounceStrength);
        bubble2.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(speed + Random.Range(-1f, 1f), bounceStrength);

        InformLevelManager();  // Notifica al LevelManager sobre la división.

        Destroy(gameObject);  // Destruye la burbuja original.
    }

    // Informa al LevelManager cuando la burbuja es destruida o dividida.
    void InformLevelManager()
    {
        if (levelManager != null)
        {
            levelManager.BubbleDestroyed();
        }
    }
}
