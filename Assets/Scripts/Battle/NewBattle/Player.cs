using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//player actions and logic, hit sound effects and animations
public class Player : Character
{
    //public GameObject morgan;

    Text log;
    Text playerHealth;
    Text coins;
    Text enemyHealth;
    Text enemyPsyche;

    bool animating = false;
    float animationTimer = 0;
    bool healing = false;

    public Text enemyHit;
    public Text playerHeal;
    public new GameObject hitEffect;
    new Animator hitEffectAnim;
    Vector2 resetPosition;

    public AudioSource swordAttackSound;
    public AudioSource swordMissSound;
    public AudioSource stunGrenadeSound;
    public AudioSource tranqAttackSound;
    public AudioSource tranqMissSound;

    Color lethalColor;
    Color nonLethalColor;
    Color missColor;
    Color healColor;

    //y = a * ((1-b)to the x)
    int baseEnemyDamage; //a
    float damageDecay = 0.15f; //b
    //with decay as 0.15f it takes 4 stuns to reduce to half damage no matter the base damage
    int time = 1; //x

    GameObject playerCharacter; //for healing player animations
    GameObject enemyCharacter; //for attacking enemy animations
    Vector2 enemyReset;

    Attack attackScript;
    Sword swordScript;

    bool playerAction;

    // Start is called before the first frame update
    void Start()
    {
        battle = GameObject.Find("Battle").GetComponent<NewBattle>();
        judge = GameObject.Find("Battle").GetComponent<NewBattleJudge>();
        player = gameObject.GetComponent<Player>();
        enemy = battle.GetEnemyCharacter().GetComponent<Enemy>();
        enemyCharacter = battle.GetEnemyCharacter();
        enemy.SetCharacterSprite(enemyCharacter.GetComponent<SpriteRenderer>());
        enemyReset = new Vector2(-322, -507);

        attackScript = player.GetComponent<Attack>();
        swordScript = player.GetComponent<Sword>();

        playerCharacter = battle.GetPlayerCharacter();

        maxPlayerHealth = player.GetHealth();
        player.SetMaxHealth(maxPlayerHealth);

        player.SetHitEffect(hitEffect);
        player.SetAttackResultNumber(enemyHit);

        maxAbilityPoints = player.GetAbilityPoints();
        maxStealthCoins = player.GetStealthCoins();
        player.SetMaxStealthCoins(maxStealthCoins);

        maxEnemyHealth = enemy.GetHealth();
        maxEnemyPsyche = enemy.GetPsyche();
        baseEnemyDamage = enemy.GetDamage();

        log = GameObject.Find("Log").GetComponent<Text>();
        playerHealth = GameObject.Find("PlayerHealth").GetComponent<Text>();

        healthText = GameObject.Find("PlayerHealth").GetComponent<Text>();
        hitDisplay = enemyHit;

        coins = GameObject.Find("StealthCoins").GetComponent<Text>();
        enemyHealth = GameObject.Find("EnemyHealth").GetComponent<Text>();
        enemyPsyche = GameObject.Find("EnemyPsyche").GetComponent<Text>();

        hitEffectAnim = hitEffect.GetComponent<Animator>();
        player.SetHitEffectAnim(hitEffectAnim);
        resetPosition = new Vector2(50, -414);

        lethalColor = new Color(0.89803921568f, 0.25882352941f, 0.25882352941f);
        nonLethalColor = new Color(0.58431372549f, 0.58039215686f, 0.8862745098f);
        missColor = new Color(1, 1, 1);
        healColor = new Color(0.37647058823f, 0.92549019607f, 0.26666666666f);

        baseEnemyDamage = enemy.GetDamage();
    }

    // Update is called once per frame
    void Update()
    {
        //if (battle.GetPlayerAction() == false && Input.GetKeyDown("m"))
        //{
        //    attackScript.LethalAttack(player.GetDamage(), player, enemy);
        //}
        if (battle.GetTurn()) //it's the player's turn
        {
            if (player.GetCharacterAction() == false && Input.GetKeyDown("m")) //player hasn't performed a successful action yet
            {
                if (attackScript.StealthCheck(player)) //not in stealth or player has enough stealth coins to attack
                {
                    swordScript.SwordAttack(player.GetDamage(), player, enemy);
                }
            }
        }

        if (animating) //hit effect animation
        {
            animationTimer += Time.deltaTime;
            enemyHit.transform.Translate(Vector2.up * Time.deltaTime * 10);

            enemyCharacter.transform.Translate(Vector2.right * Time.deltaTime * 40);
            if (enemyHit.color == lethalColor)
            {
                enemyCharacter.GetComponent<SpriteRenderer>().color = lethalColor;
            }
            else if (enemyHit.color == nonLethalColor)
            {
                enemyCharacter.GetComponent<SpriteRenderer>().color = nonLethalColor;
            }

            if (animationTimer >= 0.40f) //hit effect animation done
            {
                enemyCharacter.GetComponent<SpriteRenderer>().color = Color.white;
                enemyCharacter.transform.position = Vector2.MoveTowards(enemyCharacter.transform.position, enemyReset, 10);

                hitEffect.transform.position = resetPosition;
                hitEffectAnim.SetBool("hit", false);
            }
            if (animationTimer >= 0.80f) //hit damage display done
            {
                enemyHit.transform.position = resetPosition;
                animating = false;
                animationTimer = 0;
                battle.ChangeTurn();
            }
        }

        if (healing) //healing effect animation
        {
            animationTimer += Time.deltaTime;
            playerHeal.transform.Translate(Vector2.up * Time.deltaTime * 10);

            playerCharacter.GetComponent<SpriteRenderer>().color = healColor;
            if (animationTimer >= 0.80f) //heal display done
            {
                playerCharacter.GetComponent<SpriteRenderer>().color = Color.white;
                playerHeal.transform.position = resetPosition;
                healing = false;
                animationTimer = 0;
                battle.ChangeTurn();
            }
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

    public void Sword()
    {
        if (player.GetStealth())
        {
            player.PayStealthCost();
            coins.text = $"{player.GetStealthCoins()}/{maxStealthCoins}";
            LethalAttack(player.GetDamage());
            //attackScript.LethalAttack(player.GetDamage(), player, enemy);
        }
        else
        {
            LethalAttack(player.GetDamage());
            //attackScript.LethalAttack(player.GetDamage(), player, enemy);
        }
    }

    public void EnergyBlast()
    {
        if (player.GetStealth())
        {
            if (player.stealthCoins > 0)
            {
                player.PayStealthCost();
                coins.text = $"{player.GetStealthCoins()}/{maxStealthCoins}";
                int newAttack = (int)Math.Ceiling(player.GetDamage() * 1.25);
                LethalAttack(newAttack);
                //attackScript.LethalAttack(newAttack, player, enemy);
            }
            else
            {
                log.text = "No coins to attack while in stealth";
            }
        }
        else
        {
            int newAttack = (int)Math.Ceiling(player.GetDamage() * 1.25);
            LethalAttack(newAttack);
            //attackScript.LethalAttack(newAttack, player, enemy);
        }
    }

    public void StunGrenade()
    {
        if (player.GetStealth())
        {
            if (player.stealthCoins > 0)
            {
                player.PayStealthCost();
                coins.text = $"{player.GetStealthCoins()}/{maxStealthCoins}";
                int newAttack = (int)Math.Ceiling(player.GetStun() * 1.50);
                NonLethalAttack(newAttack);
            }
            else
            {
                log.text = "No coins to attack while in stealth";
            }
        }
        else
        {
            int newAttack = (int)Math.Ceiling(player.GetStun() * 1.50);
            NonLethalAttack(newAttack);
        }
    }

    public void Heal()
    {
        int plusHealth = (int)(maxPlayerHealth * 0.20f); //we're gonna add this much
        int healed = maxPlayerHealth - player.GetHealth();
        if (player.GetHealth() + plusHealth > maxPlayerHealth) //if we over heal
        {
            player.SetHealth(maxPlayerHealth); //back to full health
            log.text = $"Player heals {healed} health"; //log that we healed

            playerHeal.text = healed.ToString();
        }
        else //normal heal
        {
            player.SetHealth(player.GetHealth() + plusHealth); //heal that amount
            log.text = $"Player heals {plusHealth} health"; //log that we healed

            playerHeal.text = plusHealth.ToString();
        }

        playerHeal.color = healColor;
        playerHeal.transform.position = new Vector2(-734, -321); //on player

        healing = true;

        playerHealth.text = player.GetHealth().ToString(); //new health
        playerHealth.text += $"/{maxPlayerHealth}"; //max health
    }

    void LethalAttack(int damage)
    {
        if (judge.ChanceSuccess(player.GetHit()) && judge.ChanceSuccess(enemy.GetDodge()) == false || enemy.GetAsleep())
        {
            if (enemy.GetAsleep())
            {
                damage = (int)Math.Floor(damage * 1.5);
            }
            //original formula
            //int attackResult = damage - enemy.GetDefense();

            //new formula
            //attack*(100/(100+defense))
            float damagePercentage = 100.0f / (100.0f + enemy.GetDefense());
            int attackResult = (int)Math.Floor(damage * damagePercentage);
            if (attackResult <= 0)
            {
                attackResult = 0;
            }

            //ANIMATION AND SOUND START
            enemyHit.text = attackResult.ToString();
            enemyHit.color = lethalColor;
            enemyHit.transform.position = new Vector2(-204, -304);

            hitEffect.transform.position = new Vector2(-294, -414);
            hitEffectAnim.SetBool("hit", true);

            swordAttackSound.Play();
            animating = true;
            //ANIMATION AND SOUND END

            enemyHealth.text = enemy.LowerHealth(attackResult).ToString();
            enemyHealth.text += $"/{maxEnemyHealth}";
            log.text = $"Player hits {attackResult} damage";

            if (enemy.GetAsleep())
            {
                enemy.Awakening();
                battle.ChangeTurn();
            }
            if (enemy.GetHealth() <= 0)
            {
                log.text = "Enemy defeated";
                battle.GameOver();
            }
        }
        else
        {
            enemyHit.text = "MISS";
            enemyHit.color = missColor;
            enemyHit.transform.position = new Vector2(-204, -304);

            swordMissSound.Play();
            animating = true;

            log.text = "Player attack misses";
        }
    }

    public void Tranq()
    {
        if (player.GetStealth())
        {
            if (player.GetStealthCoins() > 0)
            {
                player.PayStealthCost();
                coins.text = $"{player.GetStealthCoins()}/{maxStealthCoins}";
                NonLethalAttack(player.GetStun());
            }
            else
            {
                log.text = "No coins to attack while in stealth";
            }
        }
        else
        {
            NonLethalAttack(player.GetStun());
        }
    }

    void NonLethalAttack(int stun)
    {
        if (judge.ChanceSuccess(player.GetHit()) && judge.ChanceSuccess(enemy.GetDodge()) == false)
        {
            //original formula
            //int attackResult = stun - enemy.GetResistance();

            //new formula
            //attack*(100/(100+defense))
            float damagePercentage = 100.0f / (100.0f + enemy.GetResistance());
            int attackResult = (int)Math.Floor(stun * damagePercentage);
            if (attackResult <= 0)
            {
                attackResult = 0;
            }

            //ANIMATION AND SOUND START
            enemyHit.text = attackResult.ToString();
            enemyHit.color = nonLethalColor;
            enemyHit.transform.position = new Vector2(-204, -304);

            hitEffect.transform.position = new Vector2(-294, -414);
            hitEffectAnim.SetBool("hit", true);

            if (stun > player.GetStun())
            {
                stunGrenadeSound.Play();
            }
            else
            {
                tranqAttackSound.Play();
            }
            animating = true;
            //ANIMATION AND SOUND END

            enemyPsyche.text = enemy.LowerPsyche(attackResult).ToString();
            log.text = $"Player hits {attackResult} stun";
            enemyPsyche.text += $"/{maxEnemyPsyche}";

            if (enemy.GetPsyche() <= 0)
            {
                enemyCharacter.GetComponent<SpriteRenderer>().color = nonLethalColor;
                enemyPsyche.text = "ZZZ";

                //int lowerDamage = (int)Math.Floor(enemy.GetDamage() * 0.20);
                //int damageBarrier = (int)Math.Floor(baseEnemyDamage * 0.60); //was .40
                //if (enemy.GetDamage() - lowerDamage > damageBarrier)
                //{
                //    enemy.SetDamage(enemy.GetDamage() - lowerDamage);
                //}

                //y = a * ((1-b)to the x)
                float decay = 1 - damageDecay;
                float exponentialDecay = (float)Math.Pow(decay, time);
                int lowerDamage = (int)Math.Floor(baseEnemyDamage * exponentialDecay);
                //Debug.Log($"Damage: {enemy.GetDamage()}, Decay: {lowerDamage}");
                enemy.SetDamage(lowerDamage);
                time++;
            }
        }
        else
        {
            enemyHit.text = "MISS";
            enemyHit.color = missColor;
            enemyHit.transform.position = new Vector2(-204, -304);

            tranqMissSound.Play();
            animating = true;

            log.text = "Player attack misses";
        }
    }

    public bool GetAnimating()
    {
        return animating;
    }
}