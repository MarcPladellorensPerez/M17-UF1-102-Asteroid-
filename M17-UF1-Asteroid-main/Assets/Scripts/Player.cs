using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject prefabBullet;
    public float speedThrusting = 1.0f;
    public float speedTurn = 1.0f;
    public float turnDirection = 0.0f;
    private bool thrusting = false;
    public Rigidbody2D rb;

    public AudioClip movementSound; // Asigna el clip de sonido desde el editor de Unity
    public AudioClip shotSound; // Asigna el clip de sonido desde el editor de Unity

    public float movementSoundVolume = 0.5f; // Volumen del sonido de movimiento
    public float shotSoundVolume = 1.0f; // Volumen del sonido de disparo

    private AudioSource audioSource;
    private Renderer playerRenderer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerRenderer = GetComponent<Renderer>();

        // Ocultar al jugador al inicio
        ShowPlayer(false);
    }

    void Update()
    {
        thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            turnDirection = 1.0f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            turnDirection = -1.0f;
        }
        else
        {
            turnDirection = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shot();
        }
    }

    private void FixedUpdate()
    {
        if (thrusting)
        {
            rb.AddForce(transform.up * speedThrusting);

            // Reproducir el sonido de movimiento al estar en movimiento
            PlaySound(movementSound, movementSoundVolume);
        }

        if (turnDirection != 0)
        {
            rb.AddTorque(turnDirection * speedTurn);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si el objeto impactado tiene un componente AsteroidController
        AsteroidController asteroidController = collision.gameObject.GetComponent<AsteroidController>();

        // Si el componente está presente, llamar a OnPlayerHit
        if (asteroidController != null)
        {
            asteroidController.OnPlayerHit();
        }
    }

    private void Shot()
    {
        GameObject o = Instantiate(prefabBullet, transform.position, transform.rotation, transform);
        Bullet b = o.GetComponent<Bullet>();
        b.Shot(transform.up);

        // Reproducir el sonido de disparo al disparar
        PlaySound(shotSound, shotSoundVolume);
    }

    private void PlaySound(AudioClip clip, float volume = 1.0f)
    {
        // Verificar si se proporcionó un clip de sonido
        if (clip != null)
        {
            // Crear un objeto de audio temporal para reproducir el sonido
            GameObject soundObject = new GameObject("PlayerSound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();

            // Asignar el clip de sonido y el volumen
            audioSource.clip = clip;
            audioSource.volume = volume;

            // Reproducir el sonido
            audioSource.Play();

            // Destruir el objeto de audio después de que termine el sonido
            Destroy(soundObject, clip.length);
        }
    }

    public void ShowPlayer(bool show)
    {
        playerRenderer.enabled = show;
    }
}
