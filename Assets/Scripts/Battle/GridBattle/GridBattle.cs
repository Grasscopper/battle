using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GridBattle : MonoBehaviour
{
    public GameObject morgan;
    public GameObject enemy;
    public GameObject moveButton;
    Button moveAction;
    public GameObject attackLethalButton;
    Button attackLethalAction;
    public GameObject attackNonLethalButton;
    Button attackNonLethalAction;
    public GameObject guardButton;
    Button guardAction;
    public GameObject markButton;
    Button markAction;
    public GameObject fultonButton;
    Button fultonAction;
    public Vector2 target;
    BattleJudge judge;
    Point start;
    Point tile;
    public int tileX;
    public int tileY;
    public static bool playerTurn = true;
    bool canMove = false;
    bool canLethalAttack = false;
    bool canNonLethalAttack = false;
    bool canGuard = false;
    bool canMark = false;
    bool canFulton = false;
    int enemyX;
    int enemyY;
    public Text playerDamageHealthText;
    public Text playerStunHealthText;
    public Text enemyDamageHealthText;
    public Text enemyStunHealthText;
    public Text outcomeText;
    int playerDamageHealth;
    int playerMaxDamageHealth;
    int playerStunHealth;
    int playerMaxStunHealth;
    int enemyDamageHealth;
    int enemyMaxDamageHealth;
    int enemyStunHealth;
    int enemyMaxStunHealth;
    int distance;
    Vector2 enemyTileMove;
    Vector2 newStartingTile;
    float playerTurnTime;
    Color darkBlue;
    Color darkRed;
    // Start is called before the first frame update
    void Start()
    {
        //our user interface
        GameObject.Find("Tactic").GetComponent<Text>().text = UpcomingTactic();
        GameObject.Find("BlueTicketCount").GetComponent<Text>().text = BountyHunter.blue.ToString();
        GameObject.Find("RedTicketCount").GetComponent<Text>().text = BountyHunter.red.ToString();

        moveAction = moveButton.GetComponent<Button>(); //Move button
        moveAction.onClick.AddListener(CanMove);

        attackLethalAction = attackLethalButton.GetComponent<Button>(); //Lethal button
        attackLethalAction.onClick.AddListener(CanLethalAttack);//CanLethalAttack

        attackNonLethalAction = attackNonLethalButton.GetComponent<Button>(); //Non lethal button
        attackNonLethalAction.onClick.AddListener(CanNonLethalAttack);

        guardAction = guardButton.GetComponent<Button>(); //Guard button
        guardAction.onClick.AddListener(CanGuard);

        markAction = markButton.GetComponent<Button>();
        markAction.onClick.AddListener(CanMark);

        fultonAction = fultonButton.GetComponent<Button>();
        fultonAction.onClick.AddListener(CanFulton);

        playerDamageHealth = PlayerControllerBattle.damageHealth; //Player Health
        playerMaxDamageHealth = playerDamageHealth;
        playerDamageHealth.ToString();
        playerDamageHealthText.text = playerDamageHealth + "/" + playerMaxDamageHealth;

        playerStunHealth = PlayerControllerBattle.stunHealth; //Player Psyche
        playerMaxStunHealth = playerStunHealth;
        playerStunHealth.ToString();
        playerStunHealthText.text = playerStunHealth + "/" + playerMaxStunHealth;

        enemyDamageHealth = EnemyControllerBattle.damageHealth; //Enemy Health
        enemyMaxDamageHealth = enemyDamageHealth;
        enemyDamageHealth.ToString();
        enemyDamageHealthText.text = enemyDamageHealth + "/" + enemyMaxDamageHealth;

        enemyStunHealth = EnemyControllerBattle.stunHealth; //Enemy Psyche
        enemyMaxStunHealth = enemyStunHealth;
        enemyStunHealth.ToString();
        enemyStunHealthText.text = enemyStunHealth + "/" + enemyMaxStunHealth;

        darkBlue = new Color(0, 0, 139);
        darkRed = new Color(139, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(enemy.transform.position);
        if (playerTurn)
        {
            PlayerTurn();
            playerTurnTime = Time.time;
        }

        if (playerTurn == false) //enemyTurn
        {
            EnemyTurn();
            if (Time.time - playerTurnTime > 2) //increase for longer turn change intervals
            {
                if (EnemyStatus.stun == false)
                {
                    ResetEnemyPsycheText();
                    EnemyAction();
                    ChangeTurn();
                }
                else if (EnemyStatus.stun)
                {
                    EnemyAsleepText();
                    EnemyWakingUp();
                    ChangeTurn();
                }
            }
        }
    }

    //displaying respective turns and changing turns
    void PlayerTurn()
    {
        GameObject.Find("Turn").GetComponent<Text>().text = "PLAYER TURN";
        GameObject.Find("Turn").GetComponent<Text>().color = darkBlue;
    }
    void EnemyTurn()
    {
        GameObject.Find("Turn").GetComponent<Text>().text = "ENEMY TURN";
        GameObject.Find("Turn").GetComponent<Text>().color = darkRed;
        if (EnemyControllerBattle.marked && EnemyStatus.stun)
        {
            GameObject.Find("Tactic").GetComponent<Text>().text = "Enemy knocked out";
        }
    }
    void ChangeTurn()
    {
        playerTurn = !playerTurn;
        if (playerTurn)
        {
            PlayerControllerBattle.guarding = false;
            EnemyControllerBattle.tactic = Random.Range(1, 10);
            if (EnemyControllerBattle.marked)
            {
                GameObject.Find("Tactic").GetComponent<Text>().text = UpcomingTactic();
            }
        }
    }

    string UpcomingTactic()
    {
        if (EnemyControllerBattle.tactic == 0)
        {
            return "Mark enemy to learn their next attack";
        }
        else if (EnemyControllerBattle.tactic <= 6 && EnemyStatus.stun == false)
        {
            return $"Upcoming attack: Handgun   {EnemyControllerBattle.attack} damage, {EnemyControllerBattle.fgRange} range";
        }
        else if (EnemyControllerBattle.tactic > 6 && EnemyStatus.stun == false)
        {
            return $"Upcoming attack: Grenade   {EnemyControllerBattle.attack + 2} damage, {EnemyControllerBattle.hgRange} range"; ;
        }
        else
        {
            return "Enemy knocked out";
        }
    }

    //handling enemy being stunned
    void EnemyWakingUp()
    {
        EnemyControllerBattle.wakeUp -= 1;
        if (EnemyControllerBattle.wakeUp == -1)
        {
            RestoreEnemyPsyche();
        }
    }
    void EnemyAsleepText()
    {
        if (EnemyControllerBattle.wakeUp == 1)
        {
            outcomeText.text = $"Enemy is asleep for {EnemyControllerBattle.wakeUp} more turn";
        }
        else if (EnemyControllerBattle.wakeUp > 1)
        {
            outcomeText.text = $"Enemy is asleep for {EnemyControllerBattle.wakeUp} more turns";
        }
        else if (EnemyControllerBattle.wakeUp <= 0)
        {
            outcomeText.text = $"Enemy is awake";
        }
    }
    void ResetEnemyPsycheText()
    {
        enemyStunHealth = EnemyControllerBattle.stunHealth; //set variable to stun health
        enemyStunHealth.ToString(); //stun health to string
        enemyStunHealthText.text = enemyStunHealth + "/" + enemyMaxStunHealth; //reset text display of stun health
        enemyStunHealthText.color = new Color(0,0,0);
    }
    void RestoreEnemyPsyche()
    {
        EnemyStatus.stun = false; //the enemy is no longer stunned
        EnemyControllerBattle.wakeUp = EnemyControllerBattle.wakeUpReset; //reset waking up strength
        EnemyControllerBattle.stunHealth = EnemyControllerBattle.stunHealthReset; //reset stun health
    }

    //These "Can" methods allow only one action to be performed by the player at once
    public void CanMove()
    {
        outcomeText.text = "Morgan is moving";
        //CanMove();
        canMove = true;

        canLethalAttack = false;
        canNonLethalAttack = false;
        canGuard = false;
        canMark = false;
        canFulton = false;
    }
    public void CanLethalAttack()
    {
        outcomeText.text = "Morgan is attacking (lethal)";
        //CanLethalAttack();
        canLethalAttack = true;

        canMove = false;
        canNonLethalAttack = false;
        canGuard = false;
        canMark = false;
        canFulton = false;
    }
    public void CanNonLethalAttack()
    {
        outcomeText.text = "Morgan is attacking (non-lethal)";
        //CanNonLethalAttack();
        canNonLethalAttack = true;

        canMove = false;
        canLethalAttack = false;
        canGuard = false;
        canMark = false;
        canFulton = false;
    }
    public void CanGuard()
    {
        outcomeText.text = "Morgan is taking cover";
        //CanGuard();
        canGuard = true;

        canMove = false;
        canLethalAttack = false;
        canNonLethalAttack = false;
        canMark = false;
        canFulton = false;
    }
    public void CanMark()
    {
        outcomeText.text = "Morgan is using his binoculars";
        //CanMark();
        canMark = true;

        canMove = false;
        canLethalAttack = false;
        canNonLethalAttack = false;
        canGuard = false;
        canFulton = false;
    }
    public void CanFulton()
    {
        outcomeText.text = "Morgan is deploying a fulton";
        //CanFulton();
        canFulton = true;

        canMove = false;
        canLethalAttack = false;
        canNonLethalAttack = false;
        canGuard = false;
        canMark = false;
    }

    //click the action buttons
    //this calls the "Can" methods above
    //perform respective action by clicking on tile
    public void Action() 
    {
        if (playerTurn)
        {
            if (canMove) //canMove comes from pressing the Move button
            {
                outcomeText.text = "Morgan is moving";
                judge = new BattleJudge();
                start = new Point(PlayerControllerBattle.startX, PlayerControllerBattle.startY); //player
                tile = new Point(tileX, tileY); //tile clicked
                if (judge.CheckMovement(start, tile)) //within movement range and no enemy is on tile
                {
                    Move();
                    ChangeTurn();
                }
                else
                {
                    outcomeText.text = "Morgan cannot move there";
                }
            }

            if (EnemyClicked(canLethalAttack) && LethalAttackRange()) //canLethalAttack comes from pressing the Lethal attack button
            {
                LethalAttack("AM MRS-4 Rifle"); //maybe later, the paramater could be string equippedWeapon
                ChangeTurn();
            }
                else if (EnemyClicked(canLethalAttack))
                {
                    outcomeText.text = "Enemy out of range (lethal)";
                }
                else if (canLethalAttack)
                {
                    outcomeText.text = "No target found (lethal)";
                }

            if (EnemyClicked(canNonLethalAttack) && NonLethalAttackRange()) //canNonLethalAttack comes from pressing the Non Lethal attack button
            {
                NonLethalAttack("Wu Pistol");
                ChangeTurn();
            }
                else if (EnemyClicked(canNonLethalAttack))
                {
                    outcomeText.text = "Enemy out of range (non-lethal)";
                }
                else if (canNonLethalAttack)
                {
                    outcomeText.text = "No target found (non-lethal)";
                }

            if (canGuard && PlayerClicked())
            {
                Guard();
                ChangeTurn();
            }
                else if (canGuard && PlayerClicked() == false)
                {
                    outcomeText.text = "No player found to guard";
                }

            if (EnemySpotted(canMark))
            {
                Mark();
            }
                else if (canMark)
                {
                    outcomeText.text = "No target found (marking)";
                }

            if (EnemyClicked(canFulton) && EnemyStatus.stun && FultonRange())
            {
                Fulton();
            }
                else if (EnemyClicked(canFulton) && EnemyStatus.stun == false && FultonRange())
                {
                outcomeText.text = "Enemy not stunned, cannot be fultoned";
                }
                else if (canFulton)
                {
                    outcomeText.text = "No target found (fulton)";
                }
        }
    }

    void Move()
    {
        morgan.transform.position = target;
        outcomeText.text = "Morgan moved";
        PlayerControllerBattle.startX = tileX;
        PlayerControllerBattle.startY = tileY; 
    }

    public bool PlayerClicked()
    {
        int startX = PlayerControllerBattle.startX;
        int startY = PlayerControllerBattle.startY;
        if (startX == tileX && startY == tileY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Guard()
    {
        PlayerControllerBattle.guarding = true;
        outcomeText.text = "Morgan took cover";
    }

    void Mark()
    {
        outcomeText.text = "Enemy marked";
        GameObject.Find("Tactic").GetComponent<Text>().text = "Intel acquired";
        EnemyControllerBattle.marked = true;
        ChangeTurn();
    }

    void Fulton()
    {
        BountyHunter.Fulton("red", EnemyControllerBattle.bounty);
        GameObject.Find("BlueTicketCount").GetComponent<Text>().text = BountyHunter.blue.ToString();
        outcomeText.text = "Enemy fultoned";
        EnemyRespawn();
        ChangeTurn();
    }

    public bool FultonRange() //amRange
    {
        judge = new BattleJudge();
        start = new Point(PlayerControllerBattle.startX, PlayerControllerBattle.startY); //player
        tile = new Point(tileX, tileY); //tile clicked
        distance = judge.GetDistance(start, tile);
        if (distance <= PlayerControllerBattle.fultonRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool EnemyClicked(bool canAct)
    {
        enemyX = EnemyControllerBattle.startX;
        enemyY = EnemyControllerBattle.startY;
        if (canAct && enemyX == tileX && enemyY == tileY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool EnemySpotted(bool canMark)
    {
        enemyX = EnemyControllerBattle.startX;
        enemyY = EnemyControllerBattle.startY;
        if (canMark && enemyX == tileX && enemyY == tileY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool LethalAttackRange() //amRange
    {
        judge = new BattleJudge();
        start = new Point(PlayerControllerBattle.startX, PlayerControllerBattle.startY); //player
        tile = new Point(tileX, tileY); //tile clicked
        distance = judge.GetDistance(start, tile);
        if (distance <= PlayerControllerBattle.amRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool NonLethalAttackRange() //wuRange
    {
        judge = new BattleJudge();
        start = new Point(PlayerControllerBattle.startX, PlayerControllerBattle.startY); //player
        tile = new Point(tileX, tileY); //tile clicked
        distance = judge.GetDistance(start, tile);
        if (distance <= PlayerControllerBattle.wuRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LethalAttack(string weapon)
    {
        judge = new BattleJudge();
        start = new Point(PlayerControllerBattle.startX, PlayerControllerBattle.startY); //from where I am
        tile = new Point(tileX, tileY); //to the tile I clicked
        if (judge.CheckAttackRange(start, tile, weapon)) //returns true if target is hit
        {
            if (enemyDamageHealth != 0)
            {
                EnemyControllerBattle.damageHealth -= 4;
                outcomeText.text = "Enemy hit: 4 Dmg";
            }
            EnemyDamageText();
            if (enemyDamageHealth <= 0)
            {
                BountyHunter.ReduceTicket("red");
                PlayerControllerBattle.bounty++;
                EnemyRespawn();
                if (BountyHunter.red <= 0)
                {
                    enemyDamageHealthText.text = "XXX";
                    enemyDamageHealthText.color = Color.red;
                }
            }
        }
        else
        {
            outcomeText.text = "Morgan Miss (lethal)";
        }
    }

    void EnemyDamageText()
    {
        enemyDamageHealth = EnemyControllerBattle.damageHealth;
        enemyDamageHealth.ToString();
        enemyDamageHealthText.text = enemyDamageHealth + "/" + enemyMaxDamageHealth;
    }

    void Respawn()
    {
        if (enemy.transform.position.x == -228.0 && enemy.transform.position.y == -30.0)
        {
            morgan.transform.position = new Vector2(-357, -30); //respawn elsewhere
            PlayerControllerBattle.startX = 1;
            PlayerControllerBattle.startY = 0;
        }
        else
        {
            morgan.transform.position = new Vector2(-228, -30); //normal respawn
            PlayerControllerBattle.startX = 1;
            PlayerControllerBattle.startY = 1;
        }

        PlayerControllerBattle.damageHealth = playerMaxDamageHealth; //reset health
        playerDamageHealth = PlayerControllerBattle.damageHealth;
        playerDamageHealth.ToString();
        playerDamageHealthText.text = playerDamageHealth + "/" + playerMaxDamageHealth;
    }

    void EnemyRespawn()
    {
        EnemyStatus.stun = false;
        EnemyControllerBattle.marked = false;
        GameObject.Find("Tactic").GetComponent<Text>().text = "Mark enemy to learn their next attack";
        if (morgan.transform.position.x == 169.0 && morgan.transform.position.y == -30.0)
        {
            enemy.transform.position = new Vector2(290, -30); //respawn elsewhere
            EnemyControllerBattle.startX = 1;
            EnemyControllerBattle.startY = 5;
        }
        else
        {
            enemy.transform.position = new Vector2(169, -30); //normal respawn
            EnemyControllerBattle.startX = 1;
            EnemyControllerBattle.startY = 4;
        }
        RestoreEnemyPsyche();
        EnemyControllerBattle.damageHealth = enemyMaxDamageHealth; //reset health
        enemyDamageHealth = EnemyControllerBattle.damageHealth;
        enemyDamageHealth.ToString();
        enemyDamageHealthText.text = enemyDamageHealth + "/" + enemyMaxDamageHealth;

        EnemyControllerBattle.stunHealth = enemyMaxStunHealth; //reset psyche
        enemyStunHealth = EnemyControllerBattle.stunHealth;
        enemyStunHealth.ToString();
        enemyStunHealthText.text = enemyStunHealth + "/" + enemyMaxStunHealth;
        EnemyStatus.stun = false;
    }

    public void NonLethalAttack(string weapon)
    {
        judge = new BattleJudge();
        start = new Point(PlayerControllerBattle.startX, PlayerControllerBattle.startY);
        tile = new Point(tileX, tileY);
        if (judge.CheckAttackRange(start, tile, weapon))
        {
            if (enemyStunHealth != 0)
            {
                EnemyControllerBattle.stunHealth--;
                outcomeText.text = "Enemy hit: 1 Stun";
            }
            enemyStunHealth = EnemyControllerBattle.stunHealth;
            enemyStunHealth.ToString();
            enemyStunHealthText.text = enemyStunHealth + "/" + enemyMaxStunHealth;
            if (enemyStunHealth <= 0)
            {
                enemyStunHealthText.text = "ZZZ";
                enemyStunHealthText.color = Color.blue;
                EnemyStatus.stun = true;
            }
        }
        else
        {
            outcomeText.text = "Morgan Miss (non-lethal)";
        }
    }

    public void EnemyAction()
    {
        judge = new BattleJudge();
        start = new Point(EnemyControllerBattle.startX, EnemyControllerBattle.startY); //tile that the enemy is standing on
        tile = new Point(PlayerControllerBattle.startX, PlayerControllerBattle.startY); //tile that the player is standing on
        distance = judge.GetDistance(start, tile); //number of tiles from enemy to player

        if (distance > 2) //move towards player to get in range
        {
            int maxMovement = distance - 2; //to keep two spaces away (sometimes we will be 1 space away, but that is OK!)

            if (PlayerControllerBattle.startX == EnemyControllerBattle.startX) //moving left and right
            {
                EnemyHorizontal(maxMovement, EnemyControllerBattle.move);
            }
            if (PlayerControllerBattle.startX != EnemyControllerBattle.startX) //moving up and down
            {
                EnemyVertical(VerticalMovement());
            }
            outcomeText.text = "Enemy moved";
        }
        else if (distance <= 2) //in range, attack player
        {
            if (EnemyControllerBattle.tactic > 6)
            {
                EnemyGrenade();
            }
            else
            {
                EnemyAttack();
            }
        }
    }

    void EnemyMove()
    {
        enemy.transform.position = enemyTileMove;
    }

    public void EnemyHorizontal(int maxMovement, int remainingMovement)
    {
        if ((PlayerControllerBattle.startY - EnemyControllerBattle.startY) > 0) //GO RIGHT
        {
            if (remainingMovement > maxMovement)
            {
                enemyTileMove = GetTarget(EnemyControllerBattle.startX, (EnemyControllerBattle.startY + maxMovement));
            }
            else
            {
                enemyTileMove = GetTarget(EnemyControllerBattle.startX, (EnemyControllerBattle.startY + remainingMovement));
            }
        }
        else if ((PlayerControllerBattle.startY - EnemyControllerBattle.startY) < 0) //GO LEFT
        {
            if (remainingMovement > maxMovement)
            {
                enemyTileMove = GetTarget(EnemyControllerBattle.startX, (EnemyControllerBattle.startY - maxMovement));
            }
            else
            {
                enemyTileMove = GetTarget(EnemyControllerBattle.startX, (EnemyControllerBattle.startY - remainingMovement));
            }
        }

        EnemyMove();
        newStartingTile = GetTile(enemyTileMove);
        EnemyControllerBattle.startX = (int)newStartingTile.x; //I didn't look up the syntax, but I already knew that to downcast I should type '(int)'
        EnemyControllerBattle.startY = (int)newStartingTile.y;
    }

    public int VerticalMovement()
    {
        int verticalMovement; //always either a 1 or 2
        if (EnemyControllerBattle.move > 2) //if 3 or more
        {
            verticalMovement = 2;
        }
        else //if 1 or 2
        {
            verticalMovement = EnemyControllerBattle.move;
        }
        return verticalMovement;
    }

    public void EnemyVertical(int verticalMovement)
    {
        int remainingMovement = EnemyControllerBattle.move;
        if ((PlayerControllerBattle.startX - EnemyControllerBattle.startX) > 0) //GO DOWN
        {
            if (PlayerControllerBattle.startX - EnemyControllerBattle.startX == 2)
            {
                enemyTileMove = GetTarget((EnemyControllerBattle.startX + verticalMovement), EnemyControllerBattle.startY);
                if (verticalMovement == 2)
                {
                    remainingMovement -= 2;
                } else if (verticalMovement == 1)
                {
                    remainingMovement = 0;
                }
            }
            else if (PlayerControllerBattle.startX - EnemyControllerBattle.startX == 1)
            {
                enemyTileMove = GetTarget((EnemyControllerBattle.startX + 1), EnemyControllerBattle.startY);
                remainingMovement -= 1;
            }
        }
        else if ((PlayerControllerBattle.startX - EnemyControllerBattle.startX) < 0) //GO UP
        {
            if (PlayerControllerBattle.startX - EnemyControllerBattle.startX == -2)
            {
                enemyTileMove = GetTarget((EnemyControllerBattle.startX - verticalMovement), EnemyControllerBattle.startY);
                if (verticalMovement == 2)
                {
                    remainingMovement -= 2;
                }
                else if (verticalMovement == 1)
                {
                    remainingMovement = 0;
                }
            }
            else if (PlayerControllerBattle.startX - EnemyControllerBattle.startX == -1)
            {
                enemyTileMove = GetTarget((EnemyControllerBattle.startX - 1), EnemyControllerBattle.startY);
                remainingMovement -= 1;
            }
        }

        EnemyMove();
        newStartingTile = GetTile(enemyTileMove);
        EnemyControllerBattle.startX = (int)newStartingTile.x; //I didn't look up the syntax, but I already knew that to downcast I should type '(int)'
        EnemyControllerBattle.startY = (int)newStartingTile.y;

        judge = new BattleJudge();
        start = new Point(EnemyControllerBattle.startX, EnemyControllerBattle.startY); //tile that the enemy is standing on
        tile = new Point(PlayerControllerBattle.startX, PlayerControllerBattle.startY); //tile that the player is standing on
        distance = judge.GetDistance(start, tile); //number of tiles from enemy to player
        int maxMovement = distance - 2; //to keep two spaces away

        if (remainingMovement != 0 && distance > 2) //moved vertically, but can still go horizontally
        {
            EnemyHorizontal(maxMovement, remainingMovement);
        }
    }

    public void EnemyAttack()
    {
        judge = new BattleJudge();
        start = new Point(EnemyControllerBattle.startX, EnemyControllerBattle.startY);
        tile = new Point(PlayerControllerBattle.startX, PlayerControllerBattle.startY);
        if (judge.CheckAttackRange(start, tile, "Frog Gun M2020"))
        {
            if (playerDamageHealth != 0) //the player has health
            {
                int attack = EnemyControllerBattle.attack;
                if (PlayerControllerBattle.guarding)
                {
                    attack -= PlayerControllerBattle.defense;
                    if (attack < 0)
                    {
                        attack = 0;
                    }
                }
                PlayerControllerBattle.damageHealth -= attack;
                PlayerControllerBattle.guarding = false;
                if (PlayerControllerBattle.damageHealth <= 0)
                {
                    outcomeText.text = $"Morgan DEFEATED by handgun";
                }
                else
                {
                    outcomeText.text = $"Morgan attacked by handgun: {attack} dmg";
                }
            }
            playerDamageHealth = PlayerControllerBattle.damageHealth;
            playerDamageHealth.ToString();
            playerDamageHealthText.text = playerDamageHealth + "/" + playerMaxDamageHealth;
            if (playerDamageHealth <= 0)
            {
                BountyHunter.ReduceTicket("blue");
                EnemyControllerBattle.bounty++;
                Respawn();
                if (BountyHunter.blue <= 0)
                {
                    playerDamageHealthText.text = "XXX";
                    playerDamageHealthText.color = Color.red;
                }
            }
        }
        else
        {
            outcomeText.text = "Enemy Miss (lethal)";
        }
    }

    public void EnemyGrenade()
    {
        judge = new BattleJudge();
        start = new Point(EnemyControllerBattle.startX, EnemyControllerBattle.startY);
        tile = new Point(PlayerControllerBattle.startX, PlayerControllerBattle.startY);
        if (judge.CheckAttackRange(start, tile, "Hand Grenade"))
        {
            if (playerDamageHealth != 0)
            {
                int attack = EnemyControllerBattle.attack;
                attack += 2;
                if (PlayerControllerBattle.guarding)
                {
                    attack -= PlayerControllerBattle.defense;
                    if (attack < 0)
                    {
                        attack = 0;
                    }
                }
                PlayerControllerBattle.damageHealth -= attack;
                PlayerControllerBattle.guarding = false;
                if (PlayerControllerBattle.damageHealth <= 0)
                {
                    outcomeText.text = $"Morgan DEFEATED by grenade";
                }
                else
                {
                    outcomeText.text = $"Morgan attacked by grenade: {attack} dmg";
                }
            }
            playerDamageHealth = PlayerControllerBattle.damageHealth;
            playerDamageHealth.ToString();
            playerDamageHealthText.text = playerDamageHealth + "/" + playerMaxDamageHealth;
            if (playerDamageHealth <= 0)
            {
                BountyHunter.ReduceTicket("blue");
                EnemyControllerBattle.bounty++;
                Respawn();
                if (BountyHunter.blue <= 0)
                {
                    playerDamageHealthText.text = "XXX";
                    playerDamageHealthText.color = Color.red;
                }
            }
        }
        else
        {
            outcomeText.text = "Enemy Miss (grenade)";
        }
    }

    public Vector2 GetTarget(int x, int y)
    {
        Vector2 target = new Vector2(0, 0);

        //Row 0
        if (x == 0 && y == 0)
        {
            target = new Vector2(-357, 68);
        }
        else if (x == 0 && y == 1)
        {
            target = new Vector2(-228, 69);
        }
        else if (x == 0 && y == 2)
        {
            target = new Vector2(-81, 69);
        }
        else if (x == 0 && y == 3)
        {
            target = new Vector2(30, 69);
        }
        else if (x == 0 && y == 4)
        {
            target = new Vector2(169, 69);
        }
        else if (x == 0 && y == 5)
        {
            target = new Vector2(290, 69);
        }

        //Row 1
        else if (x == 1 && y == 0)
        {
            target = new Vector2(-357, -30);
        }
        else if (x == 1 && y == 1)
        {
            target = new Vector2(-228, -30);
        }
        else if (x == 1 && y == 2)
        {
            target = new Vector2(-81, -30);
        }
        else if (x == 1 && y == 3)
        {
            target = new Vector2(30, -30);
        }
        else if (x == 1 && y == 4)
        {
            target = new Vector2(169, -30);
        }
        else if (x == 1 && y == 5)
        {
            target = new Vector2(290, -30);
        }

        //Row 2
        else if (x == 2 && y == 0)
        {
            target = new Vector2(-357, -120);
        }
        else if (x == 2 && y == 1)
        {
            target = new Vector2(-228, -120);
        }
        else if (x == 2 && y == 2)
        {
            target = new Vector2(-81, -120);
        }
        else if (x == 2 && y == 3)
        {
            target = new Vector2(30, -120);
        }
        else if (x == 2 && y == 4)
        {
            target = new Vector2(169, -120);
        }
        else if (x == 2 && y == 5)
        {
            target = new Vector2(290, -120);
        }

        return target;
    }

    public Vector2 GetTile(Vector2 target)
    {
        Vector2 tile = new Vector2(0, 0);

        //Row 0
        if (target.x == -357 && target.y == 68)
        {
            tile = new Vector2(0, 0);
        }
        else if (target.x == -228 && target.y == 69)
        {
            tile = new Vector2(0, 1);
        }
        else if (target.x == -81 && target.y == 69)
        {
            tile = new Vector2(0, 2);
        }
        else if (target.x == 30 && target.y == 69)
        {
            tile = new Vector2(0, 3);
        }
        else if (target.x == 169 && target.y == 69)
        {
            tile = new Vector2(0, 4);
        }
        else if (target.x == 290 && target.y == 69)
        {
            tile = new Vector2(0, 5);
        }

        //Row 1
        if (target.x == -357 && target.y == -30)
        {
            tile = new Vector2(1, 0);
        }
        else if (target.x == -228 && target.y == -30)
        {
            tile = new Vector2(1, 1);
        }
        else if (target.x == -81 && target.y == -30)
        {
            tile = new Vector2(1, 2);
        }
        else if (target.x == 30 && target.y == -30)
        {
            tile = new Vector2(1, 3);
        }
        else if (target.x == 169 && target.y == -30)
        {
            tile = new Vector2(1, 4);
        }
        else if (target.x == 290 && target.y == -30)
        {
            tile = new Vector2(1, 5);
        }

        //Row 2
        if (target.x == -357 && target.y == -120)
        {
            tile = new Vector2(2, 0);
        }
        else if (target.x == -228 && target.y == -120)
        {
            tile = new Vector2(2, 1);
        }
        else if (target.x == -81 && target.y == -120)
        {
            tile = new Vector2(2, 2);
        }
        else if (target.x == 30 && target.y == -120)
        {
            tile = new Vector2(2, 3);
        }
        else if (target.x == 169 && target.y == -120)
        {
            tile = new Vector2(2, 4);
        }
        else if (target.x == 290 && target.y == -120)
        {
            tile = new Vector2(2, 5);
        }

        return tile;
    }   
}
