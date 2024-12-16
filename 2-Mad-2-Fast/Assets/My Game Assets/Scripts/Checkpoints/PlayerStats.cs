using System;
using UnityEngine;

/// <summary>
/// Storage class that contains two player stats object for each player
/// </summary>
public class PlayerStatsStorage : MonoBehaviour
{
    #region variables
    [Header("Player Stats")]
    [SerializeField]
    public static PlayerStats ps1;
    [SerializeField]
    public static PlayerStats ps2;
    #endregion
}
