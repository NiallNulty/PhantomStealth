using BrainCloud.LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Network;

public class Network : MonoBehaviour
{
    public delegate void AuthenticationRequestCompleted();
    public delegate void AuthenticationRequestFailed();
    public delegate void BrainCloudLogOutCompleted();
    public delegate void BrainCloudLogOutFailed();

    public static Network sharedInstance;

    private BrainCloudWrapper brainCloudWrapper;

    private string username;

    void Awake()
    {
        if (sharedInstance == null)
        { 
            sharedInstance = this;

            DontDestroyOnLoad(this.gameObject);

            brainCloudWrapper = gameObject.AddComponent<BrainCloudWrapper>();
            brainCloudWrapper.Init();

            LogManager.Log("BrainCloud client version: " + brainCloudWrapper.Client.BrainCloudClientVersion);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        brainCloudWrapper.RunCallbacks();
    }

    void OnApplicationQuit()
    {
        brainCloudWrapper.Logout(true);
    }

    public bool HasAuthenticatedPreviously()
    {
        return brainCloudWrapper.GetStoredProfileId() != string.Empty && brainCloudWrapper.GetStoredAnonymousId() != string.Empty;
    }

    public bool IsAuthenticated()
    {
        LogManager.Log("Checking if user is authenticated.....");
        bool isAuthenticated = brainCloudWrapper.Client.Authenticated;
        LogManager.Log("Is Authenticated :" + isAuthenticated.ToString());
        return isAuthenticated;
    }

    public string GetUsername()
    {
        return username;
    }

    public void LogOut(BrainCloudLogOutCompleted brainCloudLogOutCompleted = null, BrainCloudLogOutFailed brainCloudLogOutFailed = null)
    {
        if (IsAuthenticated())
        {
            // Success 
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                LogManager.Log("LogOut success: " + responseData);

                username = string.Empty;

                if (brainCloudLogOutCompleted != null)
                    brainCloudLogOutCompleted();
            };

            // Failure
            BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
            {
                LogManager.Log("BrainCloud Logout failed: " + statusMessage);

                if (brainCloudLogOutFailed != null)
                    brainCloudLogOutFailed();
            };

            // Make the BrainCloud request
            brainCloudWrapper.Logout(true, successCallback, failureCallback);
        }
        else
        {
            Debug.Log("BrainCloud Logout failed: user is not authenticated");

            if (brainCloudLogOutFailed != null)
                brainCloudLogOutFailed();
        }
    }


    public void Reconnect(AuthenticationRequestCompleted authenticationRequestCompleted = null, AuthenticationRequestFailed authenticationRequestFailed = null)
    {
        // Success
        BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
        {
            LogManager.Log("Reconnect authentication success: " + responseData);
            HandleAuthenticationSuccess(responseData, cbObject, authenticationRequestCompleted);
        };

        // Failure 
        BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
        {
            LogManager.Log("Reconnect authentication failed. " + statusMessage);

            if (authenticationRequestFailed != null)
                authenticationRequestFailed();
        };

        // Make the BrainCloud request
        brainCloudWrapper.Reconnect(successCallback, failureCallback);
    }

    public void RequestAnonymousAuthentication(AuthenticationRequestCompleted authenticationRequestCompleted = null, AuthenticationRequestFailed authenticationRequestFailed = null)
    {
        //success
        BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
        {
            LogManager.Log("RequestAnonymousAuthentication success: " + responseData);
            HandleAuthenticationSuccess(responseData, cbObject, authenticationRequestCompleted);
        };

        //failure
        BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
        {
            LogManager.Log("RequestAnonymousAuthentication failed: " + statusMessage);

            if (authenticationRequestCompleted != null)
            {
                authenticationRequestCompleted();
            }
        };

        //Make BranCloud Request
        brainCloudWrapper.AuthenticateAnonymous(successCallback, failureCallback);
    }

    public void RequestAuthenticationUniversal(string userID, string password, AuthenticationRequestCompleted authenticationRequestCompleted = null, AuthenticationRequestFailed authenticationRequestFailed = null)
    {
        //success
        BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
        {
            LogManager.Log("Universal authentication success: " + responseData);
            HandleAuthenticationSuccess(responseData, cbObject, authenticationRequestCompleted);
            username = userID;
        };

        //failure
        BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
        {
            LogManager.Log("Universal authentication failed. " + statusMessage);

            if (authenticationRequestFailed != null)
                authenticationRequestFailed();
        };

        //Make the BrainCloud request
        brainCloudWrapper.AuthenticateUniversal(userID, password, true, successCallback, failureCallback);
    }

    private void HandleAuthenticationSuccess(string responseData, object cbObject, AuthenticationRequestCompleted authenticationRequestCompleted)
    {

        JsonData jsonData = JsonMapper.ToObject(responseData);
        username = jsonData["data"]["playerName"].ToString();

        if (authenticationRequestCompleted != null)
        {
            authenticationRequestCompleted();
        }
    }
}
