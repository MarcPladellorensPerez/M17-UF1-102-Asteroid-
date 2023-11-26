using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject gamePanel;
    public GameObject victoryMenu;
    public GameObject gameOverMenu;

    public TextMeshProUGUI startMenuBestScoreText;

    public TextMeshProUGUI gamePanelBestScoreText;
    public TextMeshProUGUI gamePanelScoreText;
    public TextMeshProUGUI gamePanelWaveText;
    public GameObject life1;
    public GameObject life2;
    public GameObject life3;

    public TextMeshProUGUI victoryMenuBestScoreText;

    public AudioClip startMenuSound;
    public AudioClip gameOverMenuSound;
    public AudioClip victoryMenuSound;
    public AudioClip bulletSound;
    public AudioClip playerMoveSound;

    private int startMenuBestScore = 0;
    private int gamePanelBestScore = 0;
    private int victoryMenuBestScore = 0;

    private int gamePanelScore;
    private int gamePanelLives;
    private int gamePanelWave;

    private GameManager gameManager;
    private Player player;
    private Bullet[] bullets;
    private AudioSource audioSource;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<Player>();
        bullets = FindObjectsOfType<Bullet>();
        audioSource = GetComponent<AudioSource>();

        ShowStartMenu();
    }

    public void AddScore(int points)
    {
        gamePanelScore += points;
        UpdateGameUI();
    }

    public void StartGame()
    {
        gamePanelScore = 0;
        gamePanelLives = 3;
        gamePanelWave = 1;

        startMenuBestScore = 0;
        startMenuBestScoreText.text = "Best Score: " + startMenuBestScore;

        EnableAsteroidSpawn(true); // Activar el spawn de asteroides

        UpdateGameUI();
        ShowGamePanel();
        ShowPlayerAndBullets(true);
    }

    public void GameOver()
    {
        if (gamePanelScore > gamePanelBestScore)
        {
            gamePanelBestScore = gamePanelScore;
            gamePanelBestScoreText.text = "Best Score: " + gamePanelBestScore;
        }

        if (gamePanelBestScore > startMenuBestScore)
        {
            startMenuBestScore = gamePanelBestScore;
            startMenuBestScoreText.text = "Best Score: " + startMenuBestScore;
        }

        if (gamePanelBestScore > victoryMenuBestScore)
        {
            victoryMenuBestScore = gamePanelBestScore;
            victoryMenuBestScoreText.text = "Best Score: " + victoryMenuBestScore;
        }

        EnableAsteroidSpawn(false); // Desactivar el spawn de asteroides

        if (gamePanelScore >= startMenuBestScore)
        {
            ShowVictoryMenu();
            PlaySound(victoryMenuSound);
        }
        else
        {
            ShowGameOverMenu();
            PlaySound(gameOverMenuSound);
        }

        ShowPlayerAndBullets(false);
    }

    public void PlayAgain()
    {
        StartGame();
    }

    public void ReturnToMainMenu()
    {
        ShowStartMenu();
    }

    public void ReduceLives()
    {
        gamePanelLives--;

        UpdateGameUI();

        if (gamePanelLives <= 0)
        {
            GameOver();
        }
    }

    public void UpdateWaveText(int wave)
    {
        gamePanelWave = wave;
        UpdateGameUI();
    }

    private void ShowStartMenu()
    {
        startMenu.SetActive(true);
        gamePanel.SetActive(false);
        victoryMenu.SetActive(false);
        gameOverMenu.SetActive(false);

        startMenuBestScoreText.text = "Best Score: " + startMenuBestScore;

        PlaySound(startMenuSound);
    }

    private void ShowGamePanel()
    {
        startMenu.SetActive(false);
        gamePanel.SetActive(true);
        victoryMenu.SetActive(false);
        gameOverMenu.SetActive(false);

        UpdateGameUI();
    }

    private void ShowVictoryMenu()
    {
        startMenu.SetActive(false);
        gamePanel.SetActive(false);
        victoryMenu.SetActive(true);
        gameOverMenu.SetActive(false);

        victoryMenuBestScoreText.text = "Best Score: " + victoryMenuBestScore;
    }

    private void ShowGameOverMenu()
    {
        startMenu.SetActive(false);
        gamePanel.SetActive(false);
        victoryMenu.SetActive(false);
        gameOverMenu.SetActive(true);
    }

    private void UpdateGameUI()
    {
        gamePanelBestScoreText.text = "Best Score: " + gamePanelBestScore;
        gamePanelScoreText.text = "Score: " + gamePanelScore;
        gamePanelWaveText.text = "Wave: " + gamePanelWave;

        life1.SetActive(gamePanelLives >= 1);
        life2.SetActive(gamePanelLives >= 2);
        life3.SetActive(gamePanelLives >= 3);
    }

    private void EnableAsteroidSpawn(bool enable)
    {
        if (gameManager != null)
        {
            gameManager.EnableAsteroidSpawn(enable);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayBulletSound()
    {
        PlaySound(bulletSound);
    }

    public void PlayPlayerMoveSound()
    {
        PlaySound(playerMoveSound);
    }

    private void ShowPlayerAndBullets(bool show)
    {
        if (player != null)
        {
            player.ShowPlayer(show);
        }

        foreach (var bullet in bullets)
        {
            bullet.ShowBullet(show);
        }
    }
}
