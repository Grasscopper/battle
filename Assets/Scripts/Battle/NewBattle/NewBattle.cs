using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

//handles button presses, attack sound effects and animations, and battle turn logic
public class NewBattle : MonoBehaviour
{
    bool gameOver = false;

    public GameObject playerCharacter;
    public GameObject enemyCharacter;
    Player player;
    Animator playerAnim;

    int maxPlayerHealth;
    int maxPlayerPsyche;
    int maxAbilityPoints;
    int maxStealthCoins;
    bool stealthOn = false;

    //public GameObject foe;
    public string attackAnimation;
    public string deathAnimation;
    Enemy enemy;
    Animator enemyAnim;

    int maxEnemyHealth;
    int maxEnemyPsyche;

    bool playerAction = false;
    bool enemyAction = false;

    bool turn = true;

    public Text log;
    public Text currentTurn;

    public Text playerHealth;
    public Text playerPsyche;

    public Text abilityPoints;

    public Text enemyHealth;
    public Text enemyPsyche;

    public Text stealthCoins;
    public Text stealthStatus;

    bool swingOnce = false;
    bool enemySoundOnce = false;
    public AudioSource swordSwingSound;
    public AudioSource energyBlastSound;
    public AudioSource healSound;
    public AudioSource stealthSound;
    public AudioSource stealthOnSound;
    public AudioSource stealthOffSound;
    public AudioSource noPointsSound;
    public AudioSource enemyDrawSound;
    public AudioSource enemySwingSound;
    public AudioSource enemyDeathSound;

    float enemyTurnTime = 0;

    bool animating = false;

    bool sword = false;
    float swordTimer = 0;

    bool enemyAttack = false;
    float enemyAttackTimer = 0;

    bool enemySpecialAttack = false;

    float horizontal;
    float vertical;

    bool horizontalNetural = true;
    bool leftIsPressed = false;
    bool rightIsPressed = false;

    bool verticalNeutral = true;
    bool upIsPressed = false;
    bool downIsPressed = false;

    int turnCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = playerCharacter.GetComponent<Player>();
        playerAnim = playerCharacter.GetComponent<Animator>();
        player.SetAnim(playerAnim);
        //Let's try to get animations only on the Player and Enemy scripts
        //No animations or sound effects on NewBattle.cs

        enemy = enemyCharacter.GetComponent<Enemy>();
        enemyAnim = enemyCharacter.GetComponent<Animator>();

        maxPlayerHealth = player.GetHealth();
        maxPlayerPsyche = player.GetPsyche();
        maxAbilityPoints = player.GetAbilityPoints();
        maxStealthCoins = player.GetStealthCoins();

        playerHealth.text = $"{maxPlayerHealth}/{maxPlayerHealth}";
        playerPsyche.text = $"{maxPlayerPsyche}/{maxPlayerPsyche}";
        abilityPoints.text = $"{maxAbilityPoints}/{maxAbilityPoints}";
        stealthCoins.text = $"{maxStealthCoins}/{maxStealthCoins}";

        maxEnemyHealth = enemy.GetHealth();
        maxEnemyPsyche = enemy.GetPsyche();

        enemyHealth.text = $"{maxEnemyHealth}/{maxEnemyHealth}";
        enemyPsyche.text = $"{maxEnemyPsyche}/{maxEnemyPsyche}";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Options"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (gameOver == false)
        {
            //ANIMATIONS START
            if (animating)
            {
                playerAction = true;
                if (sword) //player sword attack animation and RPG math
                {
                    swordTimer += Time.deltaTime;
                    if (swordTimer >= 0.10f && swingOnce == false)
                    {
                        swingOnce = true;
                        swordSwingSound.Play();
                    }
                    if (swordTimer >= 0.75f) //we are done with the sword attack animation 
                    {
                        //RESET ANIMATION CODE
                        animating = false; //no longer animating let's end this player's turn
                        sword = false; //sword is false so we can trigger this animation again
                        swordTimer = 0; //reset the time
                        swingOnce = false; //we want to be able to swing once again
                        //END RESET

                        player.Sword(); //this is when I want the hit fx and RPG math
                    }
                }

                if (enemyAttack) //enemy attack animation and RPG math
                {
                    enemyAttackTimer += Time.deltaTime;
                    if (enemyAttackTimer >= 0.30f && enemySoundOnce == false) //sound once because we don't have an animation yet
                    {
                        enemySoundOnce = true;
                        enemyDrawSound.Play();
                        enemySwingSound.Play();
                    }
                    if (enemyAttackTimer >= 0.75f) //theoretical enemy attack animation is over
                    {
                        //RESET ANIMATION CODE
                        animating = false;
                        enemyAttack = false;
                        enemyAttackTimer = 0;
                        enemySoundOnce = false;
                        //END RESET

                        enemy.Attack(); //this is when I want the hit fx and RPG math
                    }
                }

                if (enemySpecialAttack) //different
                {
                    enemyAttackTimer += Time.deltaTime;
                    if (enemyAttackTimer >= 0.30f && enemySoundOnce == false)
                    {
                        enemySoundOnce = true;
                        enemyDrawSound.Play();
                        enemySwingSound.Play();
                    }
                    if (enemyAttackTimer >= 0.75f) //theoretical enemy attack animation is over
                    {
                        animating = false;
                        enemySpecialAttack = false; //different
                        enemyAttackTimer = 0;
                        enemySoundOnce = false;

                        enemy.SpecialAttack(); //different and this is when I want the hit fx
                    }
                }
            }
            //ANIMATIONS END

            //PLAYER TURN
            if (turn && animating == false) //player turn and no animations are in progress
            {
                PlayerTurn();

                //SWORD ATTACK
                if (!playerAction && Input.GetKeyDown("a") || !playerAction && Input.GetButtonDown("Square") && Input.GetAxis("R2") <= 0 && Input.GetAxis("L2") <= 0)
                {
                    if (player.GetStealth() == false || player.GetStealth() && player.GetStealthCoins() > 0)
                    {
                        playerAction = true;
                        playerAnim.Play("lethalAttack");
                        animating = true;
                        sword = true;
                    }
                    else
                    {
                        noPointsSound.Play();
                        log.text = "Not enough coins to attack in stealth";
                    }
                }

                //STUN GRENADE
                if (!playerAction && Input.GetKeyDown("u"))
                {
                    player.StunGrenade();
                }

                horizontal = Input.GetAxis("DPadX");
                if (horizontal < 0 && !leftIsPressed) //LEFT dpad
                {
                    horizontalNetural = false;
                    leftIsPressed = true;
                }
                else if (horizontal > 0 && !rightIsPressed) //RIGHT dpad
                {
                    horizontalNetural = false;
                    rightIsPressed = true;
                    if (player.GetAbilityPoints() >= 3)
                    {
                        player.SetAbilityPoints(player.GetAbilityPoints() - 3);
                        abilityPoints.text = $"{player.GetAbilityPoints()}/{maxAbilityPoints}";

                        playerAction = true;
                        player.StunGrenade();
                    }
                    else
                    {
                        noPointsSound.Play();
                        log.text = "Not enough ability points";
                    }
                }
                else if (horizontal == 0 && !horizontalNetural) //NEUTRAL dpad
                {
                    horizontalNetural = true;
                    leftIsPressed = false;
                    rightIsPressed = false;
                }

                //vertical = Input.GetAxis("DPadY");
                //if (vertical > 0 && !upIsPressed)
                //{
                //    verticalNeutral = false;
                //    upIsPressed = true;
                //    Debug.Log("Up");
                //}
                //else if (vertical < 0 && !downIsPressed)
                //{
                //    verticalNeutral = false;
                //    downIsPressed = true;
                //    Debug.Log("Down");
                //}
                //else if (vertical == 0 && !verticalNeutral)
                //{
                //    verticalNeutral = true;
                //    downIsPressed = false;
                //    upIsPressed = false;
                //}

                //ENERGY BLAST
                if (!playerAction && Input.GetKeyDown("k") || !playerAction && Input.GetButtonDown("L1"))
                {
                    if (player.GetAbilityPoints() >= 2)
                    {
                        player.SetAbilityPoints(player.GetAbilityPoints() - 2);
                        abilityPoints.text = $"{player.GetAbilityPoints()}/{maxAbilityPoints}";

                        playerAction = true;
                        energyBlastSound.Play();
                        player.EnergyBlast();
                    }
                    else
                    {
                        noPointsSound.Play();
                        log.text = "Not enough ability points";
                    }
                }

                //HEAL
                if (!playerAction && Input.GetKeyDown("l") || !playerAction && Input.GetButtonDown("R1"))
                {
                    if (player.GetAbilityPoints() >= 1 && player.GetHealth() != maxPlayerHealth)
                    {
                        player.SetAbilityPoints(player.GetAbilityPoints() - 1);
                        abilityPoints.text = $"{player.GetAbilityPoints()}/{maxAbilityPoints}";

                        playerAction = true;
                        healSound.Play();
                        player.Heal();
                    }
                    else if (player.GetAbilityPoints() >= 1 && player.GetHealth() == maxPlayerHealth)
                    {
                        noPointsSound.Play();
                        log.text = "Already at full health";
                    }
                    else
                    {
                        noPointsSound.Play();
                        log.text = "Not enough ability points";
                    }
                }

                //TRANQUILIZER GUN ATTACK
                if (!playerAction && Input.GetKeyDown("s") || !playerAction && Input.GetButtonDown("Triangle") && Input.GetAxis("R2") <= 0 && Input.GetAxis("L2") <= 0)
                {
                    if (enemy.GetAsleep())
                    {
                        noPointsSound.Play();
                        log.text = "Enemy already asleep";
                    }
                    else
                    {
                        playerAction = true;
                        player.Tranq();
                    }
                }

                //WAIT THIS TURN
                if (!playerAction && Input.GetKeyDown("f") || !playerAction && Input.GetButtonDown("Circle") && Input.GetAxis("R2") <= 0 && Input.GetAxis("L2") <= 0)
                {
                    playerAction = true;
                    log.text = "Player waiting";
                    ChangeTurn();
                }

                //TOGGLE STEALTH
                if (!playerAction && Input.GetKeyDown("z") || !playerAction && Input.GetButtonDown("Cross"))
                {
                    if (player.GetStealthCoins() > 0 && stealthOn == false)
                    {
                        stealthOn = true; //toggle
                        player.PayStealthCost(); //with stealth coins, you can enter stealth
                        player.EnterStealth(); //enter stealth and avoid enemy attacks

                        stealthCoins.text = $"{player.GetStealthCoins()}/{maxStealthCoins}";
                        stealthOnSound.Play(); //on sound
                        log.text = "Player entered stealth";
                    }
                    else if (stealthOn)
                    {
                        stealthOn = false; //toggle
                        player.Refund(); //+1 stealth coins
                        player.LeaveStealth(); //leave stealth

                        stealthCoins.text = $"{player.GetStealthCoins()}/{maxStealthCoins}";
                        stealthOffSound.Play(); //off sound
                        log.text = "Player has left stealth";
                    }
                    else if (player.GetStealthCoins() <= 0)
                    {
                        noPointsSound.Play();
                        log.text = "Cannot enter stealth. 0 coins.";
                    }
                }
            }
            //PLAYER TURN END

            //ENEMY TURN START
            if (turn == false && animating == false) //it's the enemy's turn and animations are done
            {
                EnemyTurn(); //turn display before performing an action
                enemyTurnTime += Time.deltaTime; //time delay until enemy performs an action

                if (!enemyAction && enemyTurnTime > 1) //action not performed and turn delay is over
                {
                    if (player.GetStealth() == false) //player in BATTLE so we can attack
                    {
                        if (enemy.GetAsleep() == false) //if enemy is awake
                        {
                            int randomAction = Random.Range(1, 10);
                            if (enemy.GetHealth() <= maxEnemyHealth * 0.40 && randomAction >= 9)
                                //low on health, then we have a 20% chance to heal
                            {
                                enemy.Heal();
                            }
                            else if (randomAction >= 7)
                                //not low on health, then we have a 40% chance to special attack
                            {
                                //SPECIAL ATTACK
                                enemyAnim.Play(attackAnimation);
                                animating = true;
                                enemySpecialAttack = true;
                            }
                            else
                            {
                                //ATTACK
                                enemyAnim.Play(attackAnimation); //originally redKingAttack
                                animating = true;
                                enemyAttack = true;
                            }
                        }
                        else if (enemy.GetAsleep()) //if enemy is asleep
                        {
                            //WAKE UP
                            enemy.Asleep(); //starting to wake up each turn 
                        }
                    }
                    else if (player.GetStealth()) //player in STEALTH so we cannot attack
                    {
                        if (enemy.GetAsleep() == false) //if enemy is awake
                        {
                            //DON'T ATTACK
                            int randomAction = Random.Range(1, 10);
                            if (enemy.GetHealth() <= maxEnemyHealth * 0.60 && randomAction >= 8)
                            {
                                enemy.Heal(); //heal
                            }
                            else
                            {
                                enemy.NoAttack(); //animate STEALTH display
                            }
                        }
                        else if (enemy.GetAsleep()) //if enemy is asleep
                        {
                            //WAKE UP
                            enemy.Asleep(); //starting to wake up each turn                   
                        }
                    }
                    enemyAction = true; //don't perform multiple actions
                    player.LeaveStealth(); //back to player, leave stealth
                    stealthOn = false; //you can toggle stealth again next player turn
                }
            }
            //ENEMY TURN END

        } //CLOSING GAME OVER BRACE
    } //CLOSING UPDATE METHOD BRACE

    public GameObject GetPlayerCharacter()
    {
        return playerCharacter;
    }

    public GameObject GetEnemyCharacter()
    {
        return enemyCharacter;
    }

    public int GetTurnCount()
    {
        return turnCount;
    }

    public void GameOver()
    {
        gameOver = true;
        if (enemy.GetHealth() <= 0)
        {
            enemyDeathSound.Play();
            //enemyAnim.Play(deathAnimation); //originally redKingDeath
        }
    }

    void PlayerTurn()
    {
        currentTurn.text = "Player Turn";
        currentTurn.color = Color.cyan;
        if (player.GetStealth())
        {
            stealthStatus.text = "Stealth";
        }
        else
        {
            stealthStatus.text = "Battle";
        }
    }

    void EnemyTurn()
    {
        currentTurn.text = "Enemy turn";
        currentTurn.color = Color.red;
    }

    public bool GetPlayerAction()
    {
        return playerAction;
    }

    public void SetPlayerAction(bool playerAction)
    {
        this.playerAction = playerAction;
    }

    public void ChangeTurn()
    {
        if (turn == false)
        {
            turnCount++;
        }

        turn = !turn; //change turns

        playerAction = false; //was true when the player performs their single action
        enemyAction = false; //was true when the enemy performs their single action

        //swingOnce = false; //was true to only play player swing animation once
        //enemySoundOnce = false; //was true to only play enemy sound once
        enemyTurnTime = 0; //on the enemy's turn, we can start the turn delay again
        //change turns, the player and enemy have not performed actions, enemy turn delay reset
    }

    public bool GetTurn() //used in Power.cs
    {
        return turn;
    }

    public bool GetAnimating() //used in Power.cs
    {
        return animating;
    }

    public void Animating()
    {
        animating = true;
        playerAction = true;
    }

    public void StopAnimating()
    {
        animating = false;
        playerAction = false;
    }
}