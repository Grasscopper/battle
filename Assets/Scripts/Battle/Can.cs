using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Can : MonoBehaviour
{
    public static bool canMove = false;
    public static bool canLethalAttack = false;
    public static bool canNonLethalAttack = false;
    public static bool canGuard = false;
    public static bool canMark = false;
    public static bool canFulton = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //These "Can" methods allow only one action to be performed by the player at once
    public static void CanMove()
    {
        canMove = true;

        canLethalAttack = false;
        canNonLethalAttack = false;
        canGuard = false;
        canMark = false;
        canFulton = false;
    }
    public static void CanLethalAttack()
    {
        canLethalAttack = true;

        canMove = false;
        canNonLethalAttack = false;
        canGuard = false;
        canMark = false;
        canFulton = false;
    }
    public static void CanNonLethalAttack()
    {
        canNonLethalAttack = true;

        canMove = false;
        canLethalAttack = false;
        canGuard = false;
        canMark = false;
        canFulton = false;
    }
    public static void CanGuard()
    {
        canGuard = true;

        canMove = false;
        canLethalAttack = false;
        canNonLethalAttack = false;
        canMark = false;
        canFulton = false;
    }
    public static void CanMark()
    {
        canMark = true;

        canMove = false;
        canLethalAttack = false;
        canNonLethalAttack = false;
        canGuard = false;
        canFulton = false;
    }
    public static void CanFulton()
    {
        canFulton = true;

        canMove = false;
        canLethalAttack = false;
        canNonLethalAttack = false;
        canGuard = false;
        canMark = false;
    }
}