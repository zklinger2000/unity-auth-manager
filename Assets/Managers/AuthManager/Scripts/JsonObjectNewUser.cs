using System;

public struct JsonObjectNewUser
{
    public string _id;
    public string username;
    public string password;
    public int __v;

    public JsonObjectNewUser(string username, string password) {
        this.username = username;
        this.password = password;
        _id = String.Empty;
        __v = 0;
    }

    public JsonObjectNewUser(string id, string username, string password, int v)
    {
        _id = id;
        this.username = username;
        this.password = password;
        __v = v;
    }
}
