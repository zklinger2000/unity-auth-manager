using UnityEngine;

public class InputManager : Manager<InputManager>
{
    private void Update()
    {
        // PREGAME
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.PREGAME)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.Instance.GoToMenu("auth");
            }
        }

        // RUNNING
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.TogglePause();
            }
        }

        // PAUSED
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.PAUSED)
        {

        }

        // LOGIN MENU
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.LOGIN_MENU)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.GoToMenu("auth");
            }
        }
    }
}
