using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class User
{
    public string Username;
    public string Token;

    public User()
    {
        Username = String.Empty;
        Token = String.Empty;
    }

    public User(string username, string token)
    {
        Username = username;
        Token = token;
    }

    public void Print()
    {
        Debug.LogFormat("username: {0}\ntoken: {1}", Username, Token);
    }
}
