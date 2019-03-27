using UnityEngine;
using UnityEditor;
using Proyecto26;    // JSON Utility

public class AuthManager : Manager<AuthManager>
{
    [SerializeField] private string basePath = "http://localhost:8050/api";
	private RequestHelper _currentRequest;
    private User _user;
    [SerializeField] private UserSaveData_SO userSaveData;

    private void Start()
    {
        _user = new User();
        // TODO: Add token check
    }

    public void PostNewUser(string username, string password)
    {
        _currentRequest = new RequestHelper {
            Uri = basePath + "/users/create",
            Body = new JsonObjectNewUser(username, password)
        };
        RestClient.Post(_currentRequest)
            .Then(res =>
            {
                JSONObject json = new JSONObject(res.Text);
                _user.Username = json["username"].ToString();
                _user.Token = json["token"].ToString();
                // Save token and username with PlayerPrefs
                userSaveData.Username = _user.Username;
                userSaveData.Token = _user.Token;
                userSaveData.Save();
            })
            .Catch(err => EditorUtility.DisplayDialog ("Error", err.Message, "Ok"));
    }

    public void PostLogin(string username, string password)
    {
        _currentRequest = new RequestHelper {
            Uri = basePath + "/users/login",
            Body = new JsonObjectLogin(username, password)
        };
        RestClient.Post(_currentRequest)
            .Then(res =>
            {
                JSONObject json = new JSONObject(res.Text);
                _user.Username = json["username"].ToString();
                _user.Token = json["token"].ToString();
                // Save token and username with PlayerPrefs
                userSaveData.Username = _user.Username;
                userSaveData.Token = _user.Token;
                userSaveData.Save();
            })
            .Catch(err => EditorUtility.DisplayDialog ("Error", err.Message, "Ok"));
    }
}
