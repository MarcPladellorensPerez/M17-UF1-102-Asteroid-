using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public float speed = 5f;
    public float timeToDestroy = 10f; // Ajusta este tiempo según tus necesidades
    private int size;

    private MenuController menuController;

    private void Start()
    {
        // Buscar el MenuController al inicio
        menuController = FindObjectOfType<MenuController>();

        // Iniciar el temporizador para destruir automáticamente después de un tiempo
        Invoke("DestroyAsteroid", timeToDestroy);

        // Iniciar el comportamiento del asteroide
        StartAsteroid();
    }

    public void Initialize(Vector2 direction, int asteroidSize)
    {
        size = asteroidSize;
        transform.localScale = new Vector3(size, size, 1);

        // Asegurarse de que el Rigidbody2D esté presente
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            // Si no está presente, intenta agregarlo
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // Ignorar colisiones con objetos de la capa "Boundary"
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Asteroid"), LayerMask.NameToLayer("Boundary"));

        rb.velocity = direction * speed;
    }

    public void OnBulletHit()
    {
        // Cancelar la destrucción automática al ser impactado por una bala
        CancelInvoke("DestroyAsteroid");

        // Verificar si se encontró el MenuController
        if (menuController != null)
        {
            // Notificar al MenuController para agregar puntos al score
            menuController.AddScore(1);
        }

        // Destruir el asteroide al ser impactado por la bala
        Destroy(gameObject);
    }

    public void OnPlayerHit()
    {
        // Verificar si se encontró el MenuController
        if (menuController != null)
        {
            // Notificar al MenuController para reducir las vidas del jugador
            menuController.ReduceLives();
        }

        // Destruir el asteroide al colisionar con el jugador
        Destroy(gameObject);
    }

    private void DestroyAsteroid()
    {
        // Destruir automáticamente el asteroide después del tiempo especificado
        Destroy(gameObject);
    }

    // Método que inicia el comportamiento del asteroide
    public void StartAsteroid()
    {
        // Implementa aquí la lógica específica de inicio del asteroide
        // Puedes añadir comportamientos, efectos, etc.
    }
}
