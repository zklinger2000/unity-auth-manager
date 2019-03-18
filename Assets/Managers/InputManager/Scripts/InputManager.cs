using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private void Update()
    {
        // PREGAME
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.PREGAME)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.Instance.StartGame();
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
    }
}
