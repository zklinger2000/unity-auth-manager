using System;
using UnityEngine;
using UnityEditor;
using Proyecto26;    // JSON Utility

public class AuthManager : Manager<AuthManager>
{
    [SerializeField] private string basePath = "http://localhost:8050/api";
	private RequestHelper _currentRequest;
    private User _user;
    [SerializeField] private UserSaveData_SO userSaveData;

    public string Username
    {
        get { return _user.Username; }
    }

    private void Start()
    {
        _user = new User();
        // Load User data from PlayerPrefs
        userSaveData.Load();
        if (userSaveData.Token != String.Empty)
        {
            _user.Username = userSaveData.Username;
            _user.Token = userSaveData.Token;
        }
    }

    public bool IsAuthenticated()
    {
        // TODO: Could add a timestamp check here if we added the 'issued at time' value to the response from REST API
        // so our token would expire offline, too
        return _user.Token != String.Empty;
    }

    public void Logout()
    {
        userSaveData.Delete();
        GameManager.Instance.GoToMenu("auth");
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
                _user.Username = TrimQuotationMarks(json["username"].ToString());
                _user.Token = TrimQuotationMarks(json["token"].ToString());
                // Save token and username with PlayerPrefs
                userSaveData.Username = _user.Username;
                userSaveData.Token = _user.Token;
                userSaveData.Save();
                GameManager.Instance.GoToMenu("welcome");
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
                _user.Username = TrimQuotationMarks(json["username"].ToString());
                _user.Token = TrimQuotationMarks(json["token"].ToString());
                // Save token and username with PlayerPrefs
                userSaveData.Username = _user.Username;
                userSaveData.Token = _user.Token;
                userSaveData.Save();
                GameManager.Instance.GoToMenu("welcome");
            })
            .Catch(err => EditorUtility.DisplayDialog ("Error", err.Message, "Ok"));
    }

    private string TrimQuotationMarks(string str)
    {
        int length = str.Length;
        str = str.Remove(length - 1, 1);
        return str.Remove(0, 1);
    }
}
