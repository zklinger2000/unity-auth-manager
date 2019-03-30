# Unity AuthManager
#### Unity Version: `2018.3.6f1`

### Prerequisites

- Unity 2018 - [https://unity3d.com/get-unity/download](https://unity3d.com/get-unity/download)
- Nodejs - [https://nodejs.org/en/download/](https://nodejs.org/en/download/)
- MongoDB - [https://docs.mongodb.com/manual/installation/](https://docs.mongodb.com/manual/installation/)

> For running the backend REST API and connecting to MongoDB, see:
> [https://github.com/zklinger2000/express-unity-auth](https://github.com/zklinger2000/express-unity-auth)  
> In my example, I am using a Digital Ocean droplet with MongoDB and running the Express REST API locally.
> Having a local MongoDB on Linux or OSX is fairly simple to setup, but I would suggest Docker on Windows, if you
> have a copy of Windows Pro Edition.

[![YouTube Video](https://i.ytimg.com/vi/LMwqkT5ZZzQ/hqdefault.jpg)](https://www.youtube.com/watch?v=LMwqkT5ZZzQ)
# Overview

This project demos a token-based Authentication system built using ExpressJS and MongoDB on the server side, and a
Unity client on the frontend. It is similar to the process of saving cookies into your web browser, but uses Unity's
PlayerPrefs utility to save the token locally instead of saving it into the browser's Application cache.

## Getting Started

Simply clone the repo and open the project folder in Unity.  This will give you the _client_ or _front-end_ piece of
this **full-stack** project.  **NOTE**: You need the REST API, or _backend_ server portion to see this work.  See the
repo link above.

### Installing
```
git clone git@github.com:zklinger2000/unity-auth-manager.git
```

Go into `Assets/Scenes/` and load the scene `Boot`.

## Manager List

The architecture for this Unity project is based on the principle _separation of concerns_.  Each manager's job is to own
a specific set of methods and properties dealing with specific aspects of the application.

### GameManager

When you load up the `Boot` scene, the only object in it is the `GamaManager` transform object with a script attached.
The `GamaManager` is in charge of loading scenes, instantiating prefabs and updating the **state** of the application.
It is also where all the other managers get initialized on startup, or enabled later at runtime.

The thing to notice about the `GameManager` class is that it inherits from the `Manager` class:
```c#
public class GameManager : Manager<GameManager>
```
In the `/Scripts/Utils` folder you'll find two classes, `Singleton.cs` and `Manager.cs`.  **Both** of these classes are
singleton implementations in C#, but the `Manager` class has the major distinction of not _replacing_ an object when a
new instantiation is attempted, thereby only ever having one instance of an object, but instead, does not allow any new
instantiations at all. The video courses in the Programming Path on Pluralsight go into depth on how these two classes
are created and used. I highly suggest watching those if you want more information on the specifics.

### UIManager
```c#
public class UIManager : Manager<UIManager>
```

The `UIManager` has a prefab with all the different canvases and buttons that make up the starting screen and menus. It
also holds all the scripts involved with handling button clicks and updating which menu should be seen for each `state`.
Depending on which `state` the application is in, each canvas has its `Active` property turned on or off, so they are
always sitting above every loaded scene, but not necessarily visible.

### AuthManager
```c#
public class AuthManager : Manager<AuthManager>
```
The `AuthManager` holds our `User` object, saves and loads our `User` data between sessions and makes HTTP calls to our
backend for logging in and making authorized requests for secure resources. 

#### HTTP
[RestClient for Unity](https://github.com/proyecto26/RestClient) is a very **Nodejs** friendly package you can get off the
Asset store. It is friendly in that it looks _very_ similiar to how one writes requests in ExpressJS. They deserve a lot
of thanks for writing that and making it open-sourced.

```c#
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
```

#### Persistent Login
A fancy way to say you don't have to login every time you open the app, or a web page.  The token we receive upon
login gets saved into a file on your local machine using Unity's `PlayerPrefs` utility.   

```c#
[SerializeField] private UserSaveData_SO userSaveData;
```

```c#
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
        if (key == "")
        {
            key = name;
        }
        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), this);
    }
```

Whenever the app opens, it checks the registry (on Windows) and if it finds a token, it updates the current `User` and 
can now be considered "logged in" as long as any check for `AuthManager.Instance.isAuthorized()` returns `true`. 

```c#
    // AuthManager.cs
    
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
```

## Authentication Flows

### SignUp
1. `username` and `password` are entered into the `InputField`'s.
1. The payload is written as a JSON string.
1. An ```HTTP POST``` request is sent to the `/create` route of the backend API with the payload attached to the body.
1. If that `username` does not already exist, a new `User` is created and saved in MongoDB.
1. The response from the server sends back a JSON object with the `User` and `token`.
1. The `User` object is updated on the client with whatever info we want to send back.
1. The `username` and `token` are saved to the local machine.

### Login
1. `username` and `password` are entered into the `InputField`'s.
1. The payload is written as a JSON string.
1. An `HTTP POST` request is sent to the `/login` route of the backend API with the payload attached to the body.
1. If that `username` and `password` are verified, we get the `User` from MongoDB.
1. The response from the server sends back a JSON object with the `User` and `token`.
1. The `User` object is updated on the client with whatever info we want to send back.
1. The `username` and `token` are saved to the local machine.

### Logout
1. The `User` object in the Unity client has its data deleted.
1. The file with the token on the local machine is deleted.
1. Any requests for secure data to the backend or `state` change in the Unity client will no longer pass a check to
`AuthManager.Instance.isAuthorized();`.

### Authorized HTTP Requests
1. A new request object is created with the JWT (token) inserted as the value of the `Authorization` Header.
1. Request is sent to the REST API.
1. On the backend, the token is pulled out of the Header and decrypted, which gives the timestamp and `User._id` value
used to do a lookup in the database.
1. If the `User` exists, and has authorization for that particular resource (not implemented for this demo), a response
with JSON data is sent back to Unity.

## Conclusion

While this is a very simple demo, it does create the framework on which a much more robust auth system could be built.
Input validation, encrypting the password in Unity before sending it in the request, adding a timestamp check, etc, can
all be added onto this to make it more secure.

On the backend, we're using a simple **username/password** ourselves, but it could be changed to use Google, AWS,
Facebook, or any other ID system, without changing the Unity code at all.  This token system does not rely on _how_
the User is authenticated, it simply relies on whether or not you have the Bearer token itself and the ID of the
document in the database matching the ID in the encrypted token.

From what I've read, while these tokens can be decrypted by a dedicated hacker, they can never be successfully
**created** without prior access to the database and the encryption key sitting on the server. So, don't put anything
in your token that is hyper critical.  The `user._id` in your database isn't that important without access to that
encryption key, thus making _it_ the thing that needs to be guarded the most.

Let's say the encryption key changes once a day on the server. Every time that encryption key is changed, no tokens
anywhere in the world would be valid and the next time someone tried to launch the app, or request a secured resource,
the old token would be invalid, the user is considered 'logged out' and the REST API would refuse any requests made
until the user is logged back in and given a new token.

After spending the last few months focused on nothing but animation in Unity, this was a great time for going on a deep
dive into the "software" aspects of building a Unity game or VR app. I love the Manager system setup I learned from the
Unity courses on Pluralsight. Marrying that to prior work built in React made it for a really fun project.

## Authors

Zachary Klinger

## License

UNLICENSED

## Acknowledgments

All of this was built upon the techniques learned from the **Swords and Shovels** course on Pluralsight blended with the
authentication process from Stephen Grider's course on Udemy
[Advanced React and Redux](https://www.udemy.com/react-redux-tutorial).  
**Swords and Shovels** is actually three separate Learning Paths on Pluralsight:
* [Unity Game Dev Courses: Fundamentals](https://www.pluralsight.com/paths/unity-game-development-core-skills)
* [Unity Game Dev Courses: Programming](https://www.pluralsight.com/paths/unity-game-dev-courses-programming)
* [Unity Game Dev Courses: Design](https://www.pluralsight.com/paths/unity-game-dev-courses-design)
