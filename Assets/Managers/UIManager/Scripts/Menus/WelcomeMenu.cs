using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeMenu : MonoBehaviour
{
    [SerializeField] private Button LoginButton;
    [SerializeField] private TextMeshProUGUI username;

    private void Start()
    {
        LoginButton.onClick.AddListener(HandleLoginClicked);
        username.text = AuthManager.Instance.Username;
    }

    private void HandleLoginClicked()
    {
        AuthManager.Instance.Logout();
    }
}
