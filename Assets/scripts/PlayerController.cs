using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Velocidad de movimiento horizontal del jugador.
    public float speed = 7f;  
    // Prefab del proyectil que se disparará.
    public GameObject projectilePrefab;  
    // Punto desde donde se instanciará el proyectil.
    public Transform projectileSpawnPoint;  

    // Límite izquierdo del movimiento del jugador.
    public float leftLimit = -15f;  
    // Límite derecho del movimiento del jugador.
    public float rightLimit = 15f;  

    void Update()
    {
        // Detecta la entrada del usuario para el movimiento horizontal (A/D o flechas izquierda/derecha).
        float horizontalInput = Input.GetAxis("Horizontal");  

        // Mueve al jugador en la dirección del input.
        transform.Translate(Vector2.right * horizontalInput * speed * Time.deltaTime);

        // Limita la posición del jugador dentro de los límites especificados.
        float clampedX = Mathf.Clamp(transform.position.x, leftLimit, rightLimit);
        transform.position = new Vector2(clampedX, transform.position.y);

        // Dispara el proyectil cuando el jugador presiona la tecla "Espacio".
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Crea una instancia del proyectil en el punto de disparo.
            Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        }
    }
}
