using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class BattleJudge : MonoBehaviour
{
    int move;
    int amRange;
    int wuRange;
    int fgRange;
    int hgRange;
    int differenceX;
    int differenceY;
    int distance;
    int enemyTileX;
    int enemyTileY;
    int playerTileX;
    int playerTileY;
    public GameObject morgan;
    int weaponRange;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckMovement(Point start, Point tile)
    {
        differenceX = tile.getX() - start.getX();
        differenceY = tile.getY() - start.getY();
        distance = Math.Abs(differenceX) + Math.Abs(differenceY);
        enemyTileX = EnemyControllerBattle.startX;
        enemyTileY = EnemyControllerBattle.startY;
        move = PlayerControllerBattle.move;
        if (tile.getX() == enemyTileX && tile.getY() == enemyTileY)
        {
            return false;
        }
        if (distance <= move)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetDistance(Point start, Point tile)
    {
        differenceX = tile.getX() - start.getX();
        differenceY = tile.getY() - start.getY();
        distance = Math.Abs(differenceX) + Math.Abs(differenceY);
        return distance;
    }

    public bool TargetHit(int distance, string weapon) //public bool TargetHit(Point start, Point tile, int distance, string weapon)
    {
        distance *= 10;
        int accuracy = 100 - distance;
        int accuracyMutator = GetWeaponAccuracy(weapon);
        accuracy += accuracyMutator;
        int hit = Random.Range(0, 110); //distance 1 is 90, 2 is 80
        if (hit <= accuracy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CheckAttackRange(Point start, Point target, string weapon)
        //lots of methods, but this one determines if you hit or miss
    {
        differenceX = target.getX() - start.getX();
        differenceY = target.getY() - start.getY();
        distance = Math.Abs(differenceX) + Math.Abs(differenceY);
        weaponRange = GetWeaponRange(weapon);
        bool targetHit = TargetHit(distance, weapon); //bool targetHit = TargetHit(start, target, distance, weapon);
        if (distance <= weaponRange && targetHit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetWeaponRange(string weapon)
    {
        switch (weapon)
        {
            case "Wu Pistol":
                wuRange = PlayerControllerBattle.wuRange;
                return wuRange;
            case "AM MRS-4 Rifle":
                amRange = PlayerControllerBattle.amRange;
                return amRange;
            case "Frog Gun M2020":
                fgRange = EnemyControllerBattle.fgRange;
                return fgRange;
            case "Hand Grenade":
                hgRange = EnemyControllerBattle.hgRange;
                return hgRange;
            default:
                Debug.Log("Not a weapon");
                return 0;
        }
    }

    public int GetWeaponAccuracy(string weapon)
    {
        switch (weapon)
        {
            case "Wu Pistol":
                return 2;
            case "AM MRS-4 Rifle":
                return 10;
            case "Frog Gun M2020":
                return 2;
            case "Hand Grenade":
                return 5;
            default:
                Debug.Log("Not a weapon");
                return 0;
        }
    }

    public bool CheckMovementEnemy(Point start, Point tile)
    {
        differenceX = tile.getX() - start.getX();
        differenceY = tile.getY() - start.getY();
        distance = Math.Abs(differenceX) + Math.Abs(differenceY);
        playerTileX = PlayerControllerBattle.startX;
        playerTileY = PlayerControllerBattle.startY;
        move = EnemyControllerBattle.move;
        if (tile.getX() == playerTileX && tile.getY() == playerTileY)
        {
            return false;
        }
        if (distance <= move)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
