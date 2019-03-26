using UnityEngine;

public class UIManager : Manager<UIManager>
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private AuthMenu _authMenu;
    [SerializeField] private LoginMenu _loginMenu;
    [SerializeField] private SignUpMenu _signUpMenu;
    [SerializeField] private Camera _dummyCamera;

    public Events.EventFadeComplete OnMainMenuFadeComplete;
    private void Start()
    {
        _mainMenu.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    public void SetDummyCameraActive(bool active)
    {
        _dummyCamera.gameObject.SetActive(active);
    }

    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        _pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);
        _authMenu.gameObject.SetActive(currentState == GameManager.GameState.AUTH_MENU);
        _loginMenu.gameObject.SetActive(currentState == GameManager.GameState.LOGIN_MENU);
        _signUpMenu.gameObject.SetActive(currentState == GameManager.GameState.SIGNUP_MENU);
    }

    private void HandleMainMenuFadeComplete(bool fadeOut)
    {
        OnMainMenuFadeComplete.Invoke(fadeOut);
    }
}
