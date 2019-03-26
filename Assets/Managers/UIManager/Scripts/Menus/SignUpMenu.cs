using UnityEngine;
using UnityEngine.UI;

public class SignUpMenu : MonoBehaviour
{
    [SerializeField] private Button SubmitButton;
    [SerializeField] private InputField _username;
    [SerializeField] private InputField _password;
    private void Start()
    {
        SubmitButton.onClick.AddListener(HandleSubmitClicked);
    }

    private void HandleSubmitClicked()
    {
        string username = _username.GetComponent<InputText>().nameValue;
        string password = _password.GetComponent<InputText>().nameValue;

        AuthManager.Instance.PostNewUser(username, password);
    }
}
