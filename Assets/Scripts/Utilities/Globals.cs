using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals 
{
    public static string EntityID { get; set; }
    public static bool isAnonymous { get; set; }
    public static bool isNewUser { get; set; }
    public static bool GhostPathEnabled { get; set; }
    public static List<Vector3> GhostPath { get; set; }
    public static string hint { get; set; }
}
