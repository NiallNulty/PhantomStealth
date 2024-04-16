using BrainCloud.LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using static Network;

public class Network : MonoBehaviour
{
    public delegate void AuthenticationRequestCompleted();
    public delegate void AuthenticationRequestFailed();
    public delegate void BrainCloudLogOutCompleted();
    public delegate void BrainCloudLogOutFailed();
    public delegate void UpdateUsernameRequestCompleted();
    public delegate void UpdateUsernameRequestFailed();
    public delegate void LeaderboardRequestCompleted(LeaderboardController leaderboard);
    public delegate void LeaderboardRequestFailed();
    public delegate void RequestUserEntityDataCompleted(UserData userData);
    public delegate void RequestUserEntityDataFailed();
    public delegate void CreateUserEntityDataCompleted();
    public delegate void CreateUserEntityDataFailed();
    public delegate void UpdateUserEntityDataCompleted();
    public delegate void UpdateUserEntityDataFailed();

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
        Globals.isAnonymous = true;
    }

    public void RequestAuthenticationUniversal(string userID, string password, bool forceCreate, AuthenticationRequestCompleted authenticationRequestCompleted = null, AuthenticationRequestFailed authenticationRequestFailed = null)
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
        brainCloudWrapper.AuthenticateUniversal(userID, password, forceCreate, successCallback, failureCallback);
        Globals.isAnonymous = false;
    }

    public void RequestUpdateUsername(string newUsername, UpdateUsernameRequestCompleted updateUsernameRequestCompleted = null, UpdateUsernameRequestFailed updateUsernameRequestFailed = null)
    {
        if (IsAuthenticated())
        {
            // Success callback lambda
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                LogManager.Log("RequestUpdateUsername success: " + responseData);

                JsonData jsonData = JsonMapper.ToObject(responseData);
                username = jsonData["data"]["playerName"].ToString();

                if (updateUsernameRequestCompleted != null)
                    updateUsernameRequestCompleted();
            };

            // Failure callback lambda
            BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
            {
                LogManager.Log("RequestUpdateUsername failed: " + statusMessage);

                if (updateUsernameRequestFailed != null)
                    updateUsernameRequestFailed();
            };

            // Make the BrainCloud request
            brainCloudWrapper.PlayerStateService.UpdateName(newUsername, successCallback, failureCallback);
        }
        else
        {
            LogManager.Log("RequestUpdateUsername failed: user is not authenticated");

            if (updateUsernameRequestFailed != null)
                updateUsernameRequestFailed();
        }
    }

    public void RequestLeaderboard(string leaderboardId, LeaderboardRequestCompleted leaderboardRequestCompleted = null, LeaderboardRequestFailed leaderboardRequestFailed = null)
    {
        RequestLeaderboard(leaderboardId, 0, 9, leaderboardRequestCompleted, leaderboardRequestFailed);
    }

    public void RequestLeaderboard(string leaderboardId, int startIndex, int endIndex, LeaderboardRequestCompleted leaderboardRequestCompleted = null, LeaderboardRequestFailed leaderboardRequestFailed = null)
    {
        if (IsAuthenticated())
        {
            // Success callback lambda
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                LogManager.Log("RequestMainHighScores success: " + responseData);

                // Read the json and update our values
                JsonData jsonData = JsonMapper.ToObject(responseData);
                JsonData leaderboard = jsonData["data"]["leaderboard"];

                List<LeaderboardEntry> leaderboardEntriesList = new List<LeaderboardEntry>();
                int rank = 0;
                string nickname;
                long ms = 0;
                float time = 0.0f;
                long score;

                if (leaderboard.IsArray)
                {
                    for (int i = 0; i < leaderboard.Count; i++)
                    {
                        rank = int.Parse(leaderboard[i]["rank"].ToString());
                        nickname = leaderboard[i]["data"]["nickname"].ToString();
                        ms = long.Parse(leaderboard[i]["score"].ToString());
                        time = (float)ms / 1000.0f;
                        score = long.Parse(leaderboard[i]["score"].ToString());

                        leaderboardEntriesList.Add(new LeaderboardEntry(nickname, rank, time, score));
                    }
                }

                LeaderboardController lb = new LeaderboardController(leaderboardId, leaderboardEntriesList);

                if (leaderboardRequestCompleted != null)
                    leaderboardRequestCompleted(lb);
            };

            // Failure callback lambda
            BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
            {
                LogManager.Log("RequestMainHighScores failed: " + statusMessage);

                if (leaderboardRequestFailed != null)
                    leaderboardRequestFailed();
            };

            // Make the BrainCloud request
            brainCloudWrapper.LeaderboardService.GetGlobalLeaderboardPage(leaderboardId, BrainCloud.BrainCloudSocialLeaderboard.SortOrder.HIGH_TO_LOW, startIndex, endIndex, successCallback, failureCallback);
        }
        else
        {
            LogManager.Log("RequestMainHighScores failed: user is not authenticated");

            if (leaderboardRequestFailed != null)
                leaderboardRequestFailed();
        }
    }

    public void RequestUserEntityData(RequestUserEntityDataCompleted requestUserEntityDataCompleted = null, RequestUserEntityDataFailed requestUserEntityDataFailed = null)
    {
        if (IsAuthenticated())
        {
            // Success callback lambda
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                LogManager.Log("RequestUserEntityData success: " + responseData);

                JsonData jsonData = JsonMapper.ToObject(responseData);
                JsonData entities = jsonData["data"]["entities"];

                UserData userData = null;
                List<Vector3> playerPathList = new List<Vector3>();


                if (entities.IsArray && entities.Count > 0)
                {
                    string entityID = entities[0]["entityId"].ToString();
                    string entityType = entities[0]["entityType"].ToString();

                    userData = new UserData(entityID, entityType);

                    if (!Globals.isNewUser)
                    {
                        JsonData playerPathArray = JsonMapper.ToObject(entities[0]["data"]["playerPath"].ToJson());


                        for (int i = 0; i < playerPathArray.Count; i++)
                        {
                            float x = float.Parse(playerPathArray[i][0].ToString());
                            float y = float.Parse(playerPathArray[i][1].ToString());
                            float z = float.Parse(playerPathArray[i][2].ToString());

                            playerPathList.Add(new Vector3(x, y, z));
                        }
                    }

                    Globals.isNewUser = false;

                    if (userData.EntityID != null)
                    {
                        Globals.EntityID = userData.EntityID;
                    }

                    if (playerPathList != null || playerPathList.Count > 0)
                    {
                        Globals.GhostPath = playerPathList;
                    }

                }

                if (requestUserEntityDataCompleted != null) 
                requestUserEntityDataCompleted(userData);
            };

            // Failure callback lambda
            BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
            {
                LogManager.Log("RequestUserEntityData failed: " + statusMessage);

                if (requestUserEntityDataFailed != null)
                    requestUserEntityDataFailed();
            };

            // Make the BrainCloud request
            brainCloudWrapper.EntityService.GetEntitiesByType("userProgress", successCallback, failureCallback);
        }
        else
        {
            LogManager.Log("RequestUserEntityData failed: user is not authenticated");

            if (requestUserEntityDataFailed != null)
                requestUserEntityDataFailed();
        }
    }

    public void CreateUserEntityData(CreateUserEntityDataCompleted createUserEntityDataCompleted = null, CreateUserEntityDataFailed createUserEntityDataFailed = null)
    {
        if (IsAuthenticated())
        {
            // Success callback lambda
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                LogManager.Log("CreateUserEntityData success: " + responseData);

                if (createUserEntityDataCompleted != null)
                    createUserEntityDataCompleted();
            };

            // Failure callback lambda
            BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
            {
                LogManager.Log("CreateUserEntityData failed: " + statusMessage);

                if (createUserEntityDataFailed != null)
                    createUserEntityDataFailed();
            };

            // Make the BrainCloud request
            brainCloudWrapper.EntityService.CreateEntity("userProgress",
                                                    "{\"playerPath\" : \"test\"}",
                                                    "{\"other\":0}",
                                                    successCallback, failureCallback);
        }
        else
        {
            LogManager.Log("CreateUserEntityData failed: user is not authenticated");

            if (createUserEntityDataFailed != null)
                createUserEntityDataFailed();
        }
    }


    public void UpdateUserEntityData(string entityID, string entityType, string jsonData, UpdateUserEntityDataCompleted updateUserEntityDataCompleted = null, UpdateUserEntityDataFailed updateUserEntityDataFailed = null)
    {
        if (IsAuthenticated())
        {
            // Success callback lambda
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                LogManager.Log("UpdateUserEntityData success: " + responseData);

                if (updateUserEntityDataCompleted != null)
                    updateUserEntityDataCompleted();
            };

            // Failure callback lambda
            BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
            {
                LogManager.Log("EntityID: " + entityID);
                LogManager.Log("EntityType: " + entityType);
                LogManager.Log("JsonDAta: " + jsonData);
                LogManager.Log("UpdateUserEntityData failed: " + statusMessage);

                if (updateUserEntityDataFailed != null)
                    updateUserEntityDataFailed();
            };

            // Make the BrainCloud request
            brainCloudWrapper.EntityService.UpdateEntity(entityID, entityType, jsonData, "{\"other\":0}", -1, successCallback, failureCallback);
        }
        else
        {
            LogManager.Log("UpdateUserEntityData failed: user is not authenticated");

            if (updateUserEntityDataFailed != null)
                updateUserEntityDataFailed();
        }
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
