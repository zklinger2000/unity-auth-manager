public struct JsonObjectLogin
{
    public string username;
    public string password;

    public JsonObjectLogin(string username, string password) {
        this.username = username;
        this.password = password;
    }
}
