using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;  // Velocidad de desplazamiento del proyectil
    public static float lastShotTime = 0f;  // Tiempo del último disparo
    public float shotDelay = 0.003f;  // Retraso mínimo entre disparos (3 milisegundo)

    void Start()
    {
        // Verifica si ha pasado el retraso necesario desde el último disparo
        if (Time.time < lastShotTime + shotDelay)
        {
            Destroy(gameObject);  // Si no ha pasado el tiempo, destruye el proyectil inmediatamente
            return;
        }

        lastShotTime = Time.time;  // Actualiza el tiempo del último disparo

        // Destruye el proyectil después de 3 segundos para evitar objetos persistentes en la escena
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        // Mueve el proyectil hacia arriba en cada frame
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("burbuja"))
        {
            Bubble bubble = collision.GetComponent<Bubble>();
            if (bubble != null && !bubble.isDestroyed)  // Verifica que la burbuja no haya sido destruida
            {
                bubble.DestroyOnProjectile();  // Llama al método de destrucción directa
            }

            Destroy(gameObject);  // Destruir el proyectil
        }
        else if (collision.CompareTag("techo"))
        {
            // Destruye el proyectil al tocar el techo
            Destroy(gameObject);
        }
    }
}
