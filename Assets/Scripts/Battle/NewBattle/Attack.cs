using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//place on game object with Player script
public class Attack : MonoBehaviour
{
    //Battle START
    protected GameObject battle;

    protected NewBattle newBattle;
    protected NewBattleJudge judge;

    protected Text log;
    protected Text coins;
    //Battle END

    //Universal Attack Sounds START
    //public AudioSource attackSound;
    //public AudioSource attackHitSound;
    //public AudioSource attackMissSound;
    //Sounds END

    int damageStore;

    //Animations START
    float timer;
    bool animating;
    bool attackOnce;
    bool attacking;
    //Animations END

    Color lethalColor;

    bool playerAction;

    // Start is called before the first frame update
    void Start()
    {
        battle = GameObject.Find("Battle");

        newBattle = battle.GetComponent<NewBattle>(); //not working sometimes
        judge = battle.GetComponent<NewBattleJudge>(); //not working sometimes

        log = GameObject.Find("Log").GetComponent<Text>();
        coins = GameObject.Find("StealthCoins").GetComponent<Text>();

        lethalColor = new Color(0.89803921568f, 0.25882352941f, 0.25882352941f);

        animating = false;
        attacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (animating)
        //{
        //    timer += Time.deltaTime;
        //    if (timer >= 0.10f && attackOnce == false)
        //    {
        //        attackOnce = true;
        //        attackSound.Play();
        //    }
        //    if (timer >= 0.75f) //we are done with the sword attack animation
        //    {
        //        animating = false; //no longer animating let's end this player's turn
        //        timer = 0; //reset the time
        //        attackOnce = false; //we want to be able to swing once again

        //        //Begin LethalAttack(); the RPG math, the precise moment of attack, when the hit effect and display starts
        //        attacking = true;
        //        attackHitSound.Play();
        //    }
        //}
    }

    protected int GetDamageStore()
    {
        return damageStore;
    }

    void SetDamageStore(int damageStore)
    {
        this.damageStore = damageStore;
    }

    //can we attack in stealth or otherwise
    public bool StealthCheck(Character character)
    {
        if (character.GetStealth() == false || character.GetStealth() && character.GetStealthCoins() > 0)
        {
            if (character.GetStealth())
            {
                character.PayStealthCost(); //if you are in stealth and have enough coins
                coins.text = $"{character.GetStealthCoins()}/{character.GetMaxStealthCoins()}";
            }
            return true;
        }
        else //in stealth, but not enough coins to attack
        {
            GameObject.Find("NoPointsSound").GetComponent<AudioSource>().Play();
            log.text = "Not enough coins to attack in stealth";
            return false;
        }
    }

    //did the attack hit?
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

    public bool GetPlayerAction()
    {
        return playerAction;
    }

    public void SetPlayerAction(bool playerAction)
    {
        this.playerAction = playerAction;
    }

    //generic lethal attack method to lower health; the moment of attack
    public bool LethalAttack(int damage, Character attacker, Character defender, AudioSource hitSound, AudioSource missSound)
    {
        log = GameObject.Find("Log").GetComponent<Text>();
        newBattle = GameObject.Find("Battle").GetComponent<NewBattle>();
        //if (attacker.GetStealth() == false || attacker.GetStealth() && attacker.GetStealthCoins() > 0)
        //{
        //    animating = true;
        //    attacking = false;
        //    newBattle.SetPlayerAction(true); //prevents multiple button presses in Player.cs
        //    attacker.GetAnim().Play("lethalAttack");
        //}
        //else
        //{
        //    animating = false;
        //    attacking = false;
        //    noPointsSound.Play();
        //    log.text = "Not enough coins to attack in stealth";
        //}

        //PERFORM GENERIC HIT ANIMATION
        //Second, perform the RPG math when animations are done; the moment of attack
        //if (animating == false && attacking)
        //{
        if (ChanceSuccess(attacker.GetHit()) && ChanceSuccess(defender.GetDodge()) == false || defender.GetAsleep())
            {
            //if (defender.GetAsleep())
            //{
            //    damage = (int)Math.Floor(damage * 1.5);
            //}

            float damagePercentage = 100.0f / (100.0f + defender.GetDefense());
                int attackResult = (int)Math.Floor(damage * damagePercentage);
                if (attackResult <= 0)
                {
                    attackResult = 0;
                }

            SetDamageStore(attackResult);

                string newHealthText = defender.LowerHealth(attackResult).ToString();
                newHealthText += $"/{defender.GetMaxHealth()}";
                defender.SetHealthText(newHealthText);

            //attacker.GetAttackResultNumber().text = attackResult.ToString();
            //attacker.GetAttackResultNumber().color = lethalColor;
            //attacker.GetAttackResultNumber().transform.position = new Vector2(-204, -304);

            log.text = $"Player hits {attackResult} damage";

            //defender.Awakening();

            //defender.Awakening() is...
            //if (defender.GetAsleep())
            //{
            //    if (defender.GetHealth() != 0)
            //    {
            //        defender.WakeUp(); //enemy asleep property is false
            //        sleepCounter = 1; //so we can fall asleep again
            //        defender.SetPsyche(maxEnemyPsyche); //restore psyche
            //        enemyPsyche.text = $"{maxEnemyPsyche}/{maxEnemyPsyche}"; //display restored psyche
            //    }

            //    newBattle.ChangeTurn();
            //}

            //newBattle.ChangeTurn();

            if (defender.GetAsleep())
            {
                GameObject.Find("Sleep").GetComponent<Sleep>().Awaken(defender);
            }

                if (defender.GetHealth() <= 0)
                {
                log.text = "Enemy defeated";
                newBattle.GameOver();
                }

                if (defender.GetAsleep())
            {
                Sleep sleep = GameObject.Find("Sleep").GetComponent<Sleep>();
                sleep.Awaken(defender);
            }
            return true;
            }
            else
            {
            //missSound.Play();

            log.text = "Player attack misses";
            //newBattle.ChangeTurn();
            return false;
            }

        //}
    }
}