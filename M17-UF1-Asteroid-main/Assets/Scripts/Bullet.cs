using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 500.0f;
    public AudioClip bulletSound; 
    public AudioClip asteroidCollisionSound;

    private AudioSource audioSource;
    private Renderer bulletRenderer;

    private void Awake()
    {
        // Desactivar la gravedad y la interacción con colisiones
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.drag = 0f;
        rb.angularDrag = 0f;
        rb.isKinematic = true;

        audioSource = GetComponent<AudioSource>();
        bulletRenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        // Reproducir el sonido de la bala al inicializarla
        PlaySound(bulletSound);
    }

    public void Shot(Vector2 direction)
    {
        // Activar la gravedad y la interacción con colisiones cuando se dispara la bala
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<Rigidbody2D>().AddForce(direction * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si el objeto impactado tiene un componente AsteroidController
        AsteroidController asteroidController = collision.gameObject.GetComponent<AsteroidController>();

        // Si el componente está presente, llamar a OnBulletHit
        if (asteroidController != null)
        {
            asteroidController.OnBulletHit();

            // Reproducir el sonido de colisión con el asteroide
            PlaySound(asteroidCollisionSound);
        }

        // Destruir la bala
        DestroyBullet();
    }

    private void PlaySound(AudioClip clip)
    {
        // Verificar si se proporcionó un clip de sonido
        if (clip != null)
        {
            // Crear un objeto de audio temporal para reproducir el sonido
            GameObject soundObject = new GameObject("BulletSound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();

            // Asignar el clip de sonido
            audioSource.clip = clip;

            // Reproducir el sonido
            audioSource.Play();

            // Destruir el objeto de audio después de que termine el sonido
            Destroy(soundObject, clip.length);
        }
    }

    public void ShowBullet(bool show)
    {
        bulletRenderer.enabled = show;
    }

    private void DestroyBullet()
    {
        // Destruir la bala
        Destroy(gameObject);
    }
}
