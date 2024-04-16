using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    private string entityID { get; set; }
    private string entityType { get; set; }
    public string playerPath { get; set; }

    public UserData()
    {
        entityID = string.Empty;
        entityType = string.Empty;
        playerPath = string.Empty;
    }

    public UserData(string EntityID, string EntityType)
    {
        entityID = EntityID;
        entityType = EntityType;
        playerPath = string.Empty;
    }

    public string EntityID
    {
        get { return entityID; }
    }

    public string EntityType
    {
        get { return entityType; }
    }

    public string JsonData
    {
        get
        {
            string jsonData = playerPath;
            return jsonData;
        }
    }
}
