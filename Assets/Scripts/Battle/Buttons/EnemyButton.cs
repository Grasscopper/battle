using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyButton : MonoBehaviour
{
    public GameObject morgan;
    public GameObject enemy;
    public Vector2 target;
    BattleJudge judge;
    bool playerMoving = false;
    bool enemyMoving = false;
    float speed;
    float step;
    Point start;
    Point tile;
    public int tileX;
    public int tileY;
    public static bool playerTurn = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
