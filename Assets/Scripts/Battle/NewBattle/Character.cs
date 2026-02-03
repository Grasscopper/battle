using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//character stats, sprite, hit effect sprite and animator, attack result number
public class Character : MonoBehaviour
{
    public int health;
    public int psyche;

    public int damage;
    public int stun;

    public int defense;
    public int resistance;

    public int hit;
    public int dodge;

    public int abilityPoints;

    public int stealthCoins;
    public bool stealth;
    public bool asleep = false;

    protected Player player;
    protected Enemy enemy;
    protected NewBattle battle;
    protected NewBattleJudge judge;

    protected Text healthText;
    protected Text psycheText;
    protected Text hitDisplay;

    protected int maxPlayerHealth;
    protected int maxPlayerPsyche;
    protected int maxAbilityPoints;
    protected int maxStealthCoins;

    protected int maxEnemyHealth;
    protected int maxEnemyPsyche;

    protected int maxHealth;
    protected int maxPsyche;

    public Animator anim;

    protected bool characterAction;
    protected SpriteRenderer characterSprite; //the character itself
    protected GameObject hitEffect; //the effect of the attack hitting the target
    protected Animator hitEffectAnim;
    protected Text attackResultNumber; //the numbers that appear above the target after an attack

    //protected int sleepCounter = 1;
    //protected int sleepLimit = 3;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool GetCharacterAction()
    {
        return characterAction;
    }

    public void SetCharacterAction(bool characterAction)
    {
        this.characterAction = characterAction;
    }

    public SpriteRenderer GetCharacterSprite()
    {
        return characterSprite;
    }

    public void SetCharacterSprite(SpriteRenderer characterSprite)
    {
        this.characterSprite = characterSprite;
    }

    public Text GetAttackResultNumber()
    {
        return attackResultNumber;
    }

    public void SetAttackResultNumber(Text attackResultNumber)
    {
        this.attackResultNumber = attackResultNumber;
    }

    public Animator GetHitEffectAnim()
    {
        return hitEffectAnim;
    }

    public void SetHitEffectAnim(Animator hitEffectAnim)
    {
        this.hitEffectAnim = hitEffectAnim;
    }

    public GameObject GetHitEffect()
    {
        return hitEffect;
    }

    public void SetHitEffect(GameObject hitEffect)
    {
        this.hitEffect = hitEffect;
    }

    public Animator GetAnim()
    {
        return anim;
    }

    public void SetAnim(Animator anim)
    {
        this.anim = anim;
    }

    public int GetMaxHealth()
    {
        return maxPlayerHealth;
        //OG maxHealth
    }

    public void SetMaxHealth(int maxPlayerHealth)
    {
        this.maxPlayerHealth = maxPlayerHealth;
        //OG maxHealth
    }

    public void SetHealthText(string healthText)
    {
        this.healthText.text = healthText;
    }

    public Text GetPsycheText()
    {
        return psycheText;
    }

    public void SetPsycheText(Text psycheText)
    {
        this.psycheText = psycheText;
    }

    public int GetMaxStealthCoins()
    {
        return maxStealthCoins;
    }

    public void SetMaxStealthCoins(int maxStealthCoins)
    {
        this.maxStealthCoins = maxStealthCoins;
    }

    public int GetMaxPsyche()
    {
        return maxPsyche;
    }

    public void SetMaxPsyche(int maxPsyche)
    {
        this.maxPsyche = maxPsyche;
    }

    //public void Asleep() //enemy asleep and waking up by an increment of 1 until sleepLimit is reached
    //{
    //    enemyPsyche.text = $"ZZZ {sleepCounter}/{sleepLimit}"; //turns asleep / wake up turn
    //    sleepCounter++; //closer to waking up
    //    if (sleepCounter != sleepLimit + 2) //plus 2 to correctly match up our variables
    //    {
    //        log.text = "Enemy is asleep"; //log enemy is asleep

    //        NoAttack(); //animate enemy being asleep with ASLEEP display
    //    }
    //    if (sleepCounter == sleepLimit + 2) //wake up because turn is reached
    //    {
    //        log.text = "Enemy is awake"; //log enemy is awake

    //        Awakening(); //animate enemy awakening with AWAKE display
    //    }
    //}

    //public void Awakening() //action to animate AWAKE display and awaken the enemy from being asleep 
    //{
    //    if (enemy.GetHealth() != 0)
    //    {
    //        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    //        enemy.WakeUp(); //enemy asleep property is false
    //        sleepCounter = 1; //so we can fall asleep again
    //        SetPsyche(maxEnemyPsyche); //restore psyche
    //        enemyPsyche.text = $"{maxEnemyPsyche}/{maxEnemyPsyche}"; //display restored psyche

    //        playerHit.text = "AWAKE";
    //        playerHit.color = Color.white;
    //        playerHit.transform.position = new Vector2(-439, -211);

    //        awakeSound.Play();
    //        stealthAsleepAnimating = true;
    //    }
    //}

    public int LowerHealth(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
        }
        return health;
    }

    public int LowerPsyche(int stun)
    {
        psyche -= stun;
        if (psyche <= 0)
        {
            psyche = 0;
            asleep = true;
        }
        return psyche;
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }

    public int GetPsyche()
    {
        return psyche;
    }

    public void SetPsyche(int psyche)
    {
        this.psyche = psyche;
    }

    public int GetDamage()
    {
        return damage;
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public int GetStun()
    {
        return stun;
    }

    public void SetStun(int stun)
    {
        this.stun = stun;
    }

    public int GetDefense()
    {
        return defense;
    }

    public void SetDefense(int defense)
    {
        this.defense = defense;
    }

    public int GetResistance()
    {
        return resistance;
    }

    public void SetResistance(int resistance)
    {
        this.resistance = resistance;
    }

    public int GetHit()
    {
        return hit;
    }

    public void SetHit(int hit)
    {
        this.hit = hit;
    }

    public int GetDodge()
    {
        return dodge;
    }

    public void SetDodge(int dodge)
    {
        this.dodge = dodge;
    }

    public int GetAbilityPoints()
    {
        return abilityPoints;
    }

    public void SetAbilityPoints(int abilityPoints)
    {
        this.abilityPoints = abilityPoints;
    }

    public int GetStealthCoins()
    {
        return stealthCoins;
    }

    public void SetStealthCoins(int stealthCoins)
    {
        this.stealthCoins = stealthCoins;
    }

    public void PayStealthCost()
    {
        stealthCoins--;
        if (stealthCoins <= 0)
        {
            stealthCoins = 0;
        }
    }

    public void Refund()
    {
        stealthCoins++;
    }

    public bool GetStealth()
    {
        return stealth;
    }

    public void EnterStealth()
    {
        stealth = true;
    }

    public void LeaveStealth()
    {
        stealth = false;
    }
    
    public bool GetAsleep()
    {

        return asleep;
    }
    public void FallAsleep()
    {
        asleep = true;
    }

    public void WakeUp()
    {
        asleep = false;
    }
}