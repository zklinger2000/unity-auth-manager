using UnityEngine;

[CreateAssetMenu(fileName = "New User Save Data", menuName = "User/Data", order = 1)]
public class UserSaveData_SO : ScriptableObject
{

    [SerializeField] private string _username;
    [SerializeField] private string _token;

    [Header("Save Data")] [SerializeField] private string key;

    public string Username
    {
        get { return _username; }
        set { _username = value; }
    }

    public string Token
    {
        get { return _token; }
        set { _token = value; }
    }

    public void Save()
    {
        if (key == "")
        {
            key = name;
        }
        string jsonData = JsonUtility.ToJson(this, true);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), this);
    }
}
