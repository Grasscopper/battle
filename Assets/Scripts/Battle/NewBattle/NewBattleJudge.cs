using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NewBattleJudge : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool ChanceSuccess(int accuracy)
    {
        int hit = Random.Range(1, 100);
        if (hit <= accuracy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //public int StealthBoost(int currentAttack)
    //{
    //    currentAttack = (int) Math.Floor(currentAttack * 1.40);
    //    return currentAttack;
    //}
}
