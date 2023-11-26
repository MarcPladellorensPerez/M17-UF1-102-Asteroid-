using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public int initialWaveCount = 1;
    public float timeBetweenSpawns = 5f;
    public float spawnSpeedMultiplier = 0.9f;
    public int spawnCountIncrement = 1;

    private int currentWave = 0;
    private float timer;

    // Referencia al objeto vacío que contendrá los asteroides
    public Transform asteroidsContainer;

    private bool isGameStarted = false;

    private MenuController menuController;

    private void Start()
    {
        // Inicializa el temporizador y comienza la primera ola
        timer = 0f;

        // Obtener la referencia al MenuController al inicio
        menuController = FindObjectOfType<MenuController>();

        StartNextWave();
    }

    private void Update()
    {
        // Contador de tiempo para spawns
        timer += Time.deltaTime;

        // Comprobar si es hora de hacer spawn
        if (timer >= timeBetweenSpawns && isGameStarted)
        {
            // Reiniciar el temporizador
            timer = 0f;

            // Hacer spawn de asteroides
            SpawnAsteroids(currentWave * spawnCountIncrement);

            // Ajustar el tiempo entre spawns y la cantidad para la próxima vez
            timeBetweenSpawns *= spawnSpeedMultiplier;
        }
    }

    private void StartNextWave()
    {
        // Incrementa el número de la wave
        currentWave++;

        // Restablecer el temporizador al comenzar una nueva wave
        timer = 0f;

        // Actualizar el texto de la ola en el menú
        if (menuController != null)
        {
            menuController.UpdateWaveText(GetCurrentWave());
        }
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }

    public void StartGame()
    {
        isGameStarted = true;
        ResetAsteroidGeneration(); // Agregar esta línea para reiniciar el spawn de asteroides
    }

    public void ResetAsteroidGeneration()
    {
        // Restablecer la generación de asteroides al llamar a StartGame
        currentWave = 0;
        timeBetweenSpawns = 5f;
    }

    public void EnableAsteroidSpawn(bool enable)
    {
        isGameStarted = enable;
    }

    public void AddScore(int points)
    {
        // Suma puntos al puntaje del juego
        if (menuController != null)
        {
            menuController.AddScore(points);
        }
    }

    public void AsteroidDestroyed(int asteroidSize)
    {
        // Si el asteroide destruido es grande (size == 3), se generan más asteroides
        if (asteroidSize == 3)
        {
            SpawnAsteroids(2);
        }

        // Comprueba si todos los asteroides de la wave actual han sido destruidos
        if (isGameStarted && GameObject.FindGameObjectsWithTag("Asteroid").Length == 0)
        {
            StartNextWave();
        }
    }

    public void SpawnAsteroids(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Al spawnear nuevos asteroides, se les asigna un tamaño aleatorio entre 1 y 3
            SpawnAsteroid(Random.Range(1, 4)); // 1: pequeño, 2: mediano, 3: grande
        }
    }

    private void SpawnAsteroid(int size)
    {
        // Asegúrate de que el objeto vacío contenedor de asteroides esté asignado
        if (isGameStarted && asteroidsContainer != null)
        {
            float spawnRadius = 10f; // Ajusta este valor según tus necesidades

            // Posición de spawn dentro de la pantalla
            Vector2 spawnPosition = new Vector2(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius));
            Vector2 spawnDirection = -spawnPosition.normalized;

            // Crea el asteroide dentro del objeto vacío ("Asteroids")
            GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity, asteroidsContainer);
            AsteroidController asteroidController = asteroid.GetComponent<AsteroidController>();

            // Inicializa el asteroide y comienza su comportamiento
            asteroidController.Initialize(spawnDirection, size);
            asteroidController.StartAsteroid();
        }
    }
}
