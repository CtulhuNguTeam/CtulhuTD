using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerStats : MonoBehaviour
{
    public static int lives;
    public int startLives = 1;
    public int startElements = 4;
    public static int rounds;
    public static Dictionary<TurretType, int> elements;
    private static float value = 0;

    void Start()
    {
        lives = startLives;
        rounds = 0;
        elements = new Dictionary<TurretType, int>();
        foreach (TurretType turret in Enum.GetValues(typeof(TurretType)))
        {
            elements.Add(turret, startElements);
        }
    }

    public static void IncreaseElement(float part)
    {
        value += part;
        if (value < 1) return;
        value--;
        IncreaseElement((TurretType)Enum.GetValues(typeof(TurretType)).GetValue(Mathf.FloorToInt(Random.value * Enum.GetValues(typeof(TurretType)).Length)));
    }

    public static void IncreaseElement(TurretType turret)
    {
        elements[turret]++;
    }
    
    public static bool HasElement(TurretType turret)
    {
        return elements[turret] > 0;
    }

    public static void DecreaseElement(TurretType turret)
    {
        elements[turret]--;
    }

}
