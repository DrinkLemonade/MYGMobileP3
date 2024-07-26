
using Newtonsoft.Json.Linq;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
/*
The provided code is not for use with mobile Titles. This is only an example, and shows how to log in with a CustomID. To implement login for a mobile Title, use either LoginWithAndroidDeviceID, LoginWithIOSDeviceID, or some form of social login such as LoginWithFacebook.
*/

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField]
    MainMenu mainMenu;
    [SerializeField]
    string titleId;
    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            /*
            Please change the titleId below to your own titleId from PlayFab Game Manager.
            If you have already set the value in the Editor Extensions, this can be skipped.
            */
            PlayFabSettings.staticSettings.TitleId = titleId;
        }
        var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        ClientGetTitleData();
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    public void ClientGetTitleData()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result => {
                if (result.Data == null || !result.Data.ContainsKey("LevelContent")) Debug.Log("No LevelContent");
                else
                {
                    Debug.Log("LevelContent: " + result.Data["LevelContent"]);
                    mainMenu.ParseLevelContent(result.Data["LevelContent"]);

                }
            },
            error => {
                Debug.Log("Got error getting titleData:");
                Debug.LogError(error.GenerateErrorReport());
            }
        );
    }
}