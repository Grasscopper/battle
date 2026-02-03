using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//enemy actions that can performed and sound effects and animations
public class Enemy : Character
{
    protected Text log;
    public Text playerHealth;
    public Text enemyHealth;
    public Text enemyPsyche;

    public Text playerHit; //damage to player, number display
    public new GameObject hitEffect; //literal hit effect
    protected Animator hitEffectAnim; //the animator for the hit effect
    protected Vector2 resetPosition; //reset hit effect and display positions out of camera's view

    protected bool hitEffectAnimating = false; //if hit effect is animating
    protected float hitEffectTimer = 0; //timer for animating hit effect

    protected bool stealthAsleepAnimating = false; //if stealth or asleep display is animating
    protected float stealthAsleepTimer = 0; //timer for animating stealth, asleep, or awake display

    public Text enemyHeal;
    protected bool healAnimating = false;
    protected float animationTimer = 0;

    public AudioSource enemyAttackSound;
    public AudioSource enemySpecialSound;
    public AudioSource enemyHealSound;
    public AudioSource enemyMissSound;
    public AudioSource enemyAsleepSound;
    public AudioSource stealthSound;
    public AudioSource awakeSound;

    protected Color lethalColor;
    protected Color nonLethalColor;
    protected Color healColor;

    protected int sleepCounter = 1;
    protected int sleepLimit;

    protected bool miss = false;

    GameObject playerCharacter;
    Vector2 playerPositionReset;

    // Start is called before the first frame update
    void Start()
    {
        log = GameObject.Find("Log").GetComponent<Text>();

        battle = GameObject.Find("Battle").GetComponent<NewBattle>();

        playerCharacter = battle.GetPlayerCharacter();
        playerPositionReset = new Vector2(-838, -431);

        player = playerCharacter.GetComponent<Player>();
        enemy = gameObject.GetComponent<Enemy>();
        judge = GameObject.Find("Battle").GetComponent<NewBattleJudge>();

        maxPlayerHealth = player.GetHealth();
        maxEnemyHealth = enemy.GetHealth();
        healthText = enemyHealth;
        enemy.SetMaxHealth(maxEnemyHealth);
        enemy.SetMaxPsyche(maxEnemyPsyche);
        maxEnemyPsyche = enemy.GetPsyche();

        hitEffectAnim = hitEffect.GetComponent<Animator>();
        resetPosition = new Vector2(50, -414);

        lethalColor = new Color(0.89803921568f, 0.25882352941f, 0.25882352941f);
        nonLethalColor = new Color(0.58431372549f, 0.58039215686f, 0.8862745098f);
        healColor = new Color(0.37647058823f, 0.92549019607f, 0.26666666666f);

        sleepLimit = sleepCounter + 2;

        SetPsycheText(enemyPsyche);
    }

    // Update is called once per frame
    void Update()
    {
        //ANIMATIONS START
        if (hitEffectAnimating) //hit effect animation
        {
            hitEffectTimer += Time.deltaTime;
            playerHit.transform.Translate(Vector2.up * Time.deltaTime * 10);

            if (!miss)
            {
                playerCharacter.GetComponent<SpriteRenderer>().color = lethalColor;
                playerCharacter.transform.Translate(Vector2.left * Time.deltaTime * 40);
            }

            if (hitEffectTimer >= 0.40f) //hit effect animation done
            {
                playerCharacter.GetComponent<SpriteRenderer>().color = Color.white;
                playerCharacter.transform.position = Vector2.MoveTowards(playerCharacter.transform.position, playerPositionReset, 10);

                hitEffect.transform.position = resetPosition;
                hitEffectAnim.SetBool("hit", false);
            }
            if (hitEffectTimer >= 0.80f) //hit damage display done
            {
                playerHit.transform.position = resetPosition;

                hitEffectAnimating = false;
                hitEffectTimer = 0;

                miss = false;
                battle.ChangeTurn();
            }
        }

        if (healAnimating) //healAnimating effect animation
        {
            animationTimer += Time.deltaTime;
            enemyHeal.transform.Translate(Vector2.up * Time.deltaTime * 10);

            gameObject.GetComponent<SpriteRenderer>().color = healColor;
            if (animationTimer >= 0.80f) //heal display done
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                enemyHeal.transform.position = resetPosition;

                healAnimating = false;
                animationTimer = 0;

                battle.ChangeTurn();
            }
        }

        if (stealthAsleepAnimating) //display for player in stealth, enemy asleep/awake
        {
            stealthAsleepTimer += Time.deltaTime;
            playerHit.transform.Translate(Vector2.up * Time.deltaTime * 10);
            if (stealthAsleepTimer >= 0.80f) //hit damage display done
            {
                playerHit.transform.position = resetPosition;

                stealthAsleepAnimating = false;
                stealthAsleepTimer = 0;

                battle.ChangeTurn();
            }
        }
        //ANIMATIONS END
    }

    public void Attack() //when you do the actual RPG math and display the attack effects
    {
        if (judge.ChanceSuccess(enemy.GetHit()) && judge.ChanceSuccess(player.GetDodge()) == false)
        {
            //original formula
            //int attackResult = enemy.GetDamage() - player.GetDefense();

            //new formula
            //attack*(100/(100+defense))
            float damagePercentage = 100.0f / (100.0f + player.GetDefense());
            int attackResult = (int)Math.Floor(enemy.GetDamage() * damagePercentage);
            if (attackResult <= 0)
            {
                attackResult = 0;
            }

            //ANIMATION AND SOUND START
            playerHit.text = attackResult.ToString(); //display damage
            playerHit.color = lethalColor;
            playerHit.transform.position = new Vector2(-734, -321); //position on player

            hitEffect.transform.position = new Vector2(-849, -431); //position on player
            hitEffectAnim.SetBool("hit", true); //animate hit effect

            enemyAttackSound.Play(); //play sound effect of hitting enemy
            hitEffectAnimating = true;
            //ANIMATION AND SOUND END

            playerHealth.text = player.LowerHealth(attackResult).ToString();
            playerHealth.text += $"/{player.GetMaxHealth()}";
            log.text = $"Enemy hits {attackResult} damage";

            if (player.GetHealth() <= 0)
            {
                log.text = "Player defeated";
                battle.GameOver();
            }
        }
        else
        {
            playerHit.text = "MISS";
            playerHit.color = Color.white;
            playerHit.transform.position = new Vector2(-759, -321);

            enemyMissSound.Play();
            hitEffectAnimating = true;
            miss = true;

            log.text = "Enemy attack misses";
        }
    }

    public void SpecialAttack() //when you do the actual RPG math and display the attack effects
    {
        int specialHit = (int)Math.Ceiling(enemy.GetHit() * 1.10);
        int specialDamage = (int)Math.Ceiling(enemy.GetDamage() * 1.50);
        if (judge.ChanceSuccess(specialHit) && judge.ChanceSuccess(player.GetDodge()) == false)
        {
            //original formula
            //int attackResult = enemy.GetDamage() - player.GetDefense();

            //new formula
            //attack*(100/(100+defense))
            float damagePercentage = 100.0f / (100.0f + player.GetDefense());
            int attackResult = (int)Math.Floor(specialDamage * damagePercentage);
            if (attackResult <= 0)
            {
                attackResult = 0;
            }

            //ANIMATION AND SOUND START
            playerHit.text = attackResult.ToString(); //display damage
            playerHit.color = lethalColor;
            playerHit.transform.position = new Vector2(-734, -321); //position on player

            hitEffect.transform.position = new Vector2(-849, -431); //position on player
            hitEffectAnim.SetBool("hit", true); //animate hit effect

            enemySpecialSound.Play(); //play sound effect of hitting enemy
            hitEffectAnimating = true;
            //ANIMATION AND SOUND END

            playerHealth.text = player.LowerHealth(attackResult).ToString();
            playerHealth.text += $"/{player.GetMaxHealth()}";
            log.text = $"SPECIAL ATTACK! Enemy hits {attackResult} damage";

            if (player.GetHealth() <= 0)
            {
                log.text = "Player defeated";
                battle.GameOver();
            }
        }
        else
        {
            playerHit.text = "MISS";
            playerHit.color = Color.white;
            playerHit.transform.position = new Vector2(-759, -321);

            enemyMissSound.Play();
            miss = true;
            hitEffectAnimating = true;

            log.text = "Enemy attack misses";
        }
    }

    public void Heal() //when you do the actual RPG math and display the heal effects
    {
        enemyHealSound.Play();
        int plusHealth = (int) (maxEnemyHealth * 0.10f); //we're gonna add this much
        int healed = maxEnemyHealth - enemy.GetHealth();
        if (enemy.GetHealth() + plusHealth > maxEnemyHealth) //if we over heal
        {
            enemy.SetHealth(maxEnemyHealth); //back to full health
            log.text = $"Enemy heals {healed} health"; //log that we healed

            enemyHeal.text = healed.ToString();
        }
        else //normal heal
        {
            enemy.SetHealth(enemy.GetHealth() + plusHealth); //heal that amount
            log.text = $"Enemy heals {plusHealth} health"; //log that we healed

            enemyHeal.text = plusHealth.ToString();
        }

        enemyHeal.color = healColor;
        enemyHeal.transform.position = new Vector2(-204, -304); //on enemy

        healAnimating = true;

        enemyHealth.text = enemy.GetHealth().ToString(); //new health
        enemyHealth.text += $"/{maxEnemyHealth}"; //max health
    }

    public void Asleep() //enemy asleep and waking up by an increment of 1 until sleepLimit is reached
    {
        gameObject.GetComponent<SpriteRenderer>().color = nonLethalColor;
        enemyPsyche.text = $"ZZZ {sleepCounter}/{sleepLimit}"; //turns asleep / wake up turn
        sleepCounter++; //closer to waking up
        if (sleepCounter != sleepLimit + 2) //plus 2 to correctly match up our variables
        {
            enemyAsleepSound.Play(); //asleep sound effect
            log.text = "Enemy is asleep"; //log enemy is asleep

            NoAttack(); //animate enemy being asleep with ASLEEP display
        }
        if (sleepCounter == sleepLimit + 2) //wake up because turn is reached
        {
            log.text = "Enemy is awake"; //log enemy is awake

            Awakening(); //animate enemy awakening with AWAKE display
        }
    }

    public void NoAttack() //action to animate STEALTH and SLEEP displays if player is in stealth or enemy is asleep
    {
        if (enemy.GetAsleep() == false) //reached only if player is in stealth
        {
            log.text = "Player avoids enemy";
            stealthSound.Play();

            playerHit.text = "STEALTH";
            playerHit.color = Color.white;
            playerHit.transform.position = new Vector2(-459, -211);

            stealthAsleepAnimating = true;
        }

        if (enemy.GetAsleep()) //regardless of stealth, if the enemy is asleep
        {
            playerHit.text = "SLEEP";
            playerHit.color = nonLethalColor;
            playerHit.transform.position = new Vector2(-439, -211);

            stealthAsleepAnimating = true;
        }
    }

    public void Awakening() //action to animate AWAKE display and awaken the enemy from being asleep 
    {
        if (enemy.GetHealth() != 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            enemy.WakeUp(); //enemy asleep property is false
            sleepCounter = 1; //so we can fall asleep again
            SetPsyche(maxEnemyPsyche); //restore psyche
            enemyPsyche.text = $"{maxEnemyPsyche}/{maxEnemyPsyche}"; //display restored psyche

            playerHit.text = "AWAKE";
            playerHit.color = Color.white;
            playerHit.transform.position = new Vector2(-439, -211);

            awakeSound.Play();
            stealthAsleepAnimating = true;
        }
    }
}
