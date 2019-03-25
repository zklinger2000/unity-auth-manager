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
    }

    private void HandleLoginClicked()
    {
        GameManager.Instance.GoToMenu("login");
        Debug.Log("CLICKED Login");
    }

    private void HandleSignUpClicked()
    {
        Debug.Log("CLICKED SignUp");
    }
}
