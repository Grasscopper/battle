using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyControllerBattle : MonoBehaviour
{
    public static int startX = 1;
    public static int startY = 4;
    public static int move = 2;
    public static int attack = 4;
    public static int damageHealth = 20;
    public static int stunHealth = 1;
    public static int stunHealthReset = 1; //stun reset
    public static int fgRange = 2;
    public static int hgRange = 2;
    public static int wakeUp = 2;
    public static int wakeUpReset = 2; //wake up reset
    public static int tactic = 0;
    public static bool marked = false;
    public static int bounty = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
