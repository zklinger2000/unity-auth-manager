using UnityEngine;
using UnityEngine.UI;

public class LoginMenu : MonoBehaviour
{
    [SerializeField] private Button SubmitButton;

    private void Start()
    {
        SubmitButton.onClick.AddListener(HandleSubmitClicked);
    }

    private void HandleSubmitClicked()
    {
        Debug.Log("CLICKED Submit");
    }
}
