using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseManager : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject pauseScreen;
    private bool isGamePaused, isPlayerAlive;



    private void Update()
    {
        IsPlayerDead();
    }

    private void IsPlayerDead()
    {
        isPlayerAlive = player.GetComponent<playerController>().isPlayerAlive;

        if (isPlayerAlive)
            PlayerInput();
          
    }
    private void PlayerInput()
    {
        if (!isPlayerAlive) return;

        if (!isGamePaused && Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
        else if (isGamePaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Resume();
            else if (Input.GetKeyDown(KeyCode.R))
                Retry();
            else if (Input.GetKeyDown(KeyCode.Return))
                GoToMainMenu();
            else if (Input.GetKeyDown(KeyCode.Delete))
                QuitGame();
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
        pauseScreen.SetActive(true);
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        pauseScreen.SetActive(false);
    }
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
