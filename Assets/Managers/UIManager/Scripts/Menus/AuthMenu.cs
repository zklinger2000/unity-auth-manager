using UnityEngine;
using UnityEngine.UI;

public class AuthMenu : MonoBehaviour
{
    [SerializeField] private Button LoginButton;
    [SerializeField] private Button SignUpButton;

    private void Start()
    {
        LoginButton.onClick.AddListener(HandleLoginClicked);
        SignUpButton.onClick.AddListener(HandleSignUpClicked);
        if (AuthManager.Instance.IsAuthenticated())
        {
            GameManager.Instance.GoToMenu("welcome");
        }
    }

    private void HandleLoginClicked()
    {
        GameManager.Instance.GoToMenu("login");
    }

    private void HandleSignUpClicked()
    {
        GameManager.Instance.GoToMenu("signup");
    }
}
