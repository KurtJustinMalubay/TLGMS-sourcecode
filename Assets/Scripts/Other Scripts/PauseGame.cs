using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject GamePause;
    public GameObject HowTo;
    public CursorScript cursor;
    private void Start()
    {
        HowTo.SetActive(true);
        Time.timeScale = 0f;
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Pause();
        }
    }
    public void startGame()
    {
        HowTo.SetActive(false);
        cursor.setShootingCursor();
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        cursor.setDefaultCursor();
        GamePause.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        cursor.setShootingCursor();
        GamePause.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
