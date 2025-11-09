using UnityEngine;
using UnityEngine.SceneManagement;

public class deathManager : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject deathScreen;
    bool isPlayerDead;

    
    void Update()
    {
        IsPlayerDead();
    }

    private void PlayerInput()
    {
        if (!isPlayerDead) return;

        if (Input.GetKeyDown(KeyCode.R))
            Retry();
        else if (Input.GetKeyDown(KeyCode.Return))
            GoToMainMenu();
        else if (Input.GetKeyDown(KeyCode.Delete))
            QuitGame();
    }
    private void IsPlayerDead()
    {
        isPlayerDead = !player.GetComponent<playerController>().isPlayerAlive;

        if(isPlayerDead)
        {
            PlayerInput();
            deathScreen.SetActive(true);
        }
        else if (!isPlayerDead)
        {
            deathScreen.SetActive(false);
        }
    }
    public void Retry()
    {
        Time.timeScale = 1f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Retrying");
    }
    public void GoToMainMenu()
    {
        //SceneManager.LoadScene("MainMenu");
        Debug.Log("Going to Main Menu");
    }
    public void QuitGame()
    {
        //Application.Quit();
        Debug.Log("Quiting game");
    }
}
