using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//place on game object with Player script
//sword attack animation, sound effects, hit effect, attack result number
public class Sword : Attack
{
    Player player;

    int damage;
    Character attacker;
    Character defender;

    bool animating = false;
    float timer = 0;

    public AudioSource swordSwingSound;
    public AudioSource hitSound;
    public AudioSource missSound;

    Color lethalColor;
    Color nonLethalColor;
    Color missColor;
    Color healColor;

    Vector2 enemyReset;
    Vector2 resetPosition;

    bool timeMarkerA = false;
    bool timeMarkerB = false;
    bool timeMarkerC = false;
    bool miss = false;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<Player>();
        lethalColor = new Color(0.89803921568f, 0.25882352941f, 0.25882352941f);
        nonLethalColor = new Color(0.58431372549f, 0.58039215686f, 0.8862745098f);
        missColor = new Color(1, 1, 1);
        healColor = new Color(0.37647058823f, 0.92549019607f, 0.26666666666f);

        enemyReset = new Vector2(-322, -507);
        resetPosition =  new Vector2(50, -414);
    }

    // Update is called once per frame
    void Update()
    {
        //sword attack animation with hit effect, attack result number, colors, and enemy reacting to attack
        if (animating)
        {
            timer += Time.deltaTime;
            if (timer >= 0.10f && timeMarkerA == false)
            {
                timeMarkerA = true;
                swordSwingSound.Play();
            }
            //"lethalAttack" animation plays for 0.65f
            if (timer >= 0.75f && timeMarkerB == false) //the moment of the sword attack
            {
                timeMarkerB = true;

                //Begin LethalAttack(); the RPG math, the precise moment of attack, when the hit effect and attack result number starts
                if (LethalAttack(damage, attacker, defender, hitSound, missSound))
                {
                    hitSound.Play();

                    //attacker.GetAttackResultNumber().text = damage.ToString();
                    attacker.GetAttackResultNumber().text = GetDamageStore().ToString();
                    attacker.GetAttackResultNumber().color = lethalColor;

                    attacker.GetHitEffect().transform.position = new Vector2(-294, -414);
                    attacker.GetHitEffectAnim().SetBool("hit", true);
                }
                else
                {
                    missSound.Play();

                    attacker.GetAttackResultNumber().text = "MISS";
                    attacker.GetAttackResultNumber().color = Color.white;

                    miss = true;
                }
 
                attacker.GetAttackResultNumber().transform.position = new Vector2(-204, -304); //display on enemy

                //attacker.GetAttackResultNumber().transform.Translate(Vector2.up * Time.deltaTime * 10);

                //defender.GetCharacterSprite().transform.Translate(Vector2.right * Time.deltaTime * 40);
                if (attacker.GetAttackResultNumber().color == lethalColor)
                {
                    defender.GetCharacterSprite().color = lethalColor;
                }
                else if (attacker.GetAttackResultNumber().color == nonLethalColor)
                {
                    defender.GetCharacterSprite().color = nonLethalColor;
                }
            }
            if (timeMarkerB && timeMarkerC == false)
            {
                //attack result number, DAMAGE or MISS, translates up no matter what
                attacker.GetAttackResultNumber().transform.Translate(Vector2.up * Time.deltaTime * 10);

                //translate the enemy if the attack hits
                if (miss == false)
                {
                    defender.GetCharacterSprite().transform.Translate(Vector2.right * Time.deltaTime * 40);
                }
            }
            if (timer >= 1.15f && timeMarkerC == false) //hit effect animation done OG 0.40
            {
                timeMarkerC = true;
                defender.GetCharacterSprite().color = Color.white;
                //defender.GetCharacterSprite().transform.position = Vector2.MoveTowards(defender.GetCharacterSprite().transform.position, enemyReset, 10);

                attacker.GetHitEffect().transform.position = resetPosition;
                attacker.GetHitEffectAnim().SetBool("hit", false);
            }
            if (timeMarkerC)
            {
                //move enemy back to rest position
                defender.GetCharacterSprite().transform.position = Vector2.MoveTowards(defender.GetCharacterSprite().transform.position, enemyReset, 10);
            }
            if (timer >= 1.55f) //attack result number done OG 0.80
            {
                attacker.GetAttackResultNumber().transform.position = resetPosition;

                animating = false; //no longer animating let's end this player's turn
                timer = 0; //reset the time
                timeMarkerA = false;
                timeMarkerB = false;
                timeMarkerC = false;
                miss = false;
                player.SetCharacterAction(false);

                newBattle.ChangeTurn();
            }
        }
    }

    public void SwordAttack(int damage, Character attacker, Character defender)
    {
        //newBattle.SetPlayerAction(true);
        player.SetCharacterAction(true); //to prevent multiple button presses in Player.cs before action is done

        attacker.GetAnim().Play("lethalAttack");
        animating = true;

        this.damage = damage; //simply the player's attack damage; before we account for the enemy's defense
        this.attacker = attacker;
        this.defender = defender;
    }
}