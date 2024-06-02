using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Global {
    private static string worldName = "world0";

    public static string WorldName { get => worldName; set => worldName = value; }
    public static string StartSquare { get => startSquare; set => startSquare = value; }

    private static string startSquare = "Level0";
    public static readonly float STRENGTH_DAMAGE_MODIFIER = 2f;
    public static readonly float WEAKNESS_DAMAGE_MODIFIER = 0.5f;
    public static readonly string COMBAT_SCENE_NAME = "CombatLevelScene";
    public static readonly int MAX_COMBAT_CREATURES = 4;
}