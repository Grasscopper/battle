using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Power : MonoBehaviour
{
    Player player;

    public int power;
    public int attack;
    public int armor;
    public int dexterity;
    public Text totalPower;

    Stack damageStack;
    int baseDamage;

    Stack stunStack;
    int baseStun;

    public Text attackPower;
    public Text attackLimit;
    int attackCounter = 0;
    float boostOne = 0; //attack boost

    Stack armorStack;
    int baseDefense;

    public Text armorPower;
    public Text armorLimit;
    int armorCounter = 0;
    float boostTwo = 0; //armor boost

    Stack hitStack;
    int baseHit;

    Stack dodgeStack;
    int baseDodge;

    public Text dexterityPower;
    public Text dexterityLimit;
    int dexterityCounter = 0;
    float boostThree = 0; //dexterity boost

    public AudioSource powerUpSound;
    public AudioSource powerDownSound;

    NewBattle battle;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<Player>();

        totalPower.text += power.ToString();
        attackLimit.text += attack.ToString();
        armorLimit.text += armor.ToString();
        dexterityLimit.text += dexterity.ToString();

        damageStack = new Stack();
        baseDamage = player.GetDamage();
        damageStack.Push(baseDamage);

        stunStack = new Stack();
        baseStun = player.GetStun();
        stunStack.Push(baseStun);

        armorStack = new Stack();
        baseDefense = player.GetDefense();
        armorStack.Push(baseDefense);

        hitStack = new Stack();
        baseHit = player.GetHit();
        hitStack.Push(baseHit);

        dodgeStack = new Stack();
        baseDodge = player.GetDodge();
        dodgeStack.Push(baseDodge);

        battle = GameObject.Find("Battle").GetComponent<NewBattle>();
    }

    // Update is called once per frame
    void Update()
    {
        totalPower.text = power.ToString();

        attackPower.text = attackCounter.ToString();
        //if we input to power up attack
        //&& we have at least one power
        //&& we are not at max power for attack
        if (battle.GetTurn() && battle.GetAnimating() == false && player.GetAnimating() == false)
        {

            if (Input.GetKeyDown("1") && power > 0 && attackCounter < attack) //ATTACK: damage and stun
            {
                powerUpSound.Play();
                power -= 1; //minus 1 power from main power supply
                attackCounter += 1; //attack power is increased by 1
                boostOne += 0.20f; //every time this block is called, attack boost is increased

                int damageBoost = (int)Math.Floor(baseDamage * boostOne);
                int newDamage = baseDamage + damageBoost;
                damageStack.Push(newDamage);
                int currentDamage = (int)damageStack.Peek();
                player.SetDamage(currentDamage);

                int stunBoost = (int)Math.Floor(baseStun * boostOne);
                int newStun = baseStun + stunBoost;
                stunStack.Push(newStun);
                int currentStun = (int)stunStack.Peek();
                player.SetStun(currentStun);
            }

            if (Input.GetAxis("R2") > 0 && Input.GetButtonDown("Square") && power > 0 && attackCounter < attack) //ATTACK: damage and stun
            {
                powerUpSound.Play();
                power -= 1; //minus 1 power from main power supply
                attackCounter += 1; //attack power is increased by 1
                boostOne += 0.20f; //every time this block is called, attack boost is increased

                int damageBoost = (int)Math.Floor(baseDamage * boostOne);
                int newDamage = baseDamage + damageBoost;
                damageStack.Push(newDamage);
                int currentDamage = (int)damageStack.Peek();
                player.SetDamage(currentDamage);

                int stunBoost = (int)Math.Floor(baseStun * boostOne);
                int newStun = baseStun + stunBoost;
                stunStack.Push(newStun);
                int currentStun = (int)stunStack.Peek();
                player.SetStun(currentStun);
            }

            if (Input.GetKeyDown("q")) //Power down ATTACK
            {
                if (damageStack.Count > 1)
                {
                    powerDownSound.Play();
                    power += 1;
                    attackCounter -= 1;
                    boostOne -= 0.20f;

                    damageStack.Pop();
                    int currentDamage = (int)damageStack.Peek();
                    player.SetDamage(currentDamage);
                }

                if (stunStack.Count > 1)
                {
                    stunStack.Pop();
                    int currentStun = (int)stunStack.Peek();
                    player.SetStun(currentStun);
                }
            }

            if (Input.GetAxis("L2") > 0 && Input.GetButtonDown("Square")) //Power down ATTACK
            {
                if (damageStack.Count > 1)
                {
                    powerDownSound.Play();
                    power += 1;
                    attackCounter -= 1;
                    boostOne -= 0.20f;

                    damageStack.Pop();
                    int currentDamage = (int)damageStack.Peek();
                    player.SetDamage(currentDamage);
                }

                if (stunStack.Count > 1)
                {
                    stunStack.Pop();
                    int currentStun = (int)stunStack.Peek();
                    player.SetStun(currentStun);
                }
            }

            armorPower.text = armorCounter.ToString();
            if (Input.GetKeyDown("2") && power > 0 && armorCounter < armor) //ARMOR: defense
            {
                powerUpSound.Play();
                power -= 1;
                armorCounter += 1;
                boostTwo += 0.20f;

                int defenseBoost = (int)Math.Floor(baseDefense * boostTwo); //1 power
                int newDefense = baseDefense + defenseBoost;
                armorStack.Push(newDefense);
                int currentDefense = (int)armorStack.Peek();
                player.SetDefense(currentDefense);
            }

            if (Input.GetAxis("R2") > 0 && Input.GetButtonDown("Triangle") && power > 0 && armorCounter < armor) //ARMOR: defense
            {
                powerUpSound.Play();
                power -= 1;
                armorCounter += 1;
                boostTwo += 0.20f;

                int defenseBoost = (int)Math.Floor(baseDefense * boostTwo); //1 power
                int newDefense = baseDefense + defenseBoost;
                armorStack.Push(newDefense);
                int currentDefense = (int)armorStack.Peek();
                player.SetDefense(currentDefense);
            }

            if (Input.GetKeyDown("w")) //Power down ARMOR
            {
                if (armorStack.Count > 1)
                {
                    powerDownSound.Play();
                    power += 1;
                    armorCounter -= 1;
                    boostTwo -= 0.20f;

                    armorStack.Pop();
                    int currentDefense = (int)armorStack.Peek();
                    player.SetDefense(currentDefense);
                }
            }

            if (Input.GetAxis("L2") > 0 && Input.GetButtonDown("Triangle")) //Power down ARMOR
            {
                if (armorStack.Count > 1)
                {
                    powerDownSound.Play();
                    power += 1;
                    armorCounter -= 1;
                    boostTwo -= 0.20f;

                    armorStack.Pop();
                    int currentDefense = (int)armorStack.Peek();
                    player.SetDefense(currentDefense);
                }
            }

            dexterityPower.text = dexterityCounter.ToString();
            if (Input.GetKeyDown("3") && power > 0 && dexterityCounter < dexterity) //DEXTERITY: hit and dodge
            {
                powerUpSound.Play();
                power -= 1;
                dexterityCounter += 1;

                //boostThree += 0.10f;

                //OLD start
                //int hitBoost = (int)Math.Ceiling(baseHit * boostThree);
                //int newHit = baseHit + hitBoost;
                //OLD end

                //NEW start
                int compareHit = player.GetHit();
                int newHit = player.GetHit();
                if (compareHit < 60)
                {
                    newHit += 6;
                }
                if (compareHit >= 60 && compareHit < 70)
                {
                    newHit += 5;
                }
                else if (compareHit >= 70 && compareHit < 80)
                {
                    newHit += 4;
                }
                else if (compareHit >= 80 && compareHit < 90)
                {
                    newHit += 3;
                }
                else if (compareHit >= 90)
                {
                    newHit += 2;
                }
                //NEW end

                hitStack.Push(newHit);
                int currentHit = (int)hitStack.Peek();
                player.SetHit(currentHit);

                //Dodge
                //float boostFour = boostThree * 2.0f; //dodge boost, has to be higher because dodge is so low

                //OLD start
                //int dodgeBoost = (int)Math.Ceiling(baseDodge * boostFour);
                //int newDodge = baseDodge + dodgeBoost;
                //OLD end

                //NEW start
                int compareDodge = player.GetDodge();
                int newDodge = player.GetDodge();
                if (compareDodge >= 0 && compareDodge < 10)
                {
                    newDodge += 5;
                }
                if (compareDodge >= 10 && compareDodge < 20)
                {
                    newDodge += 4;
                }
                else if (compareDodge >= 20 && compareDodge < 30)
                {
                    newDodge += 3;
                }
                else if (compareDodge >= 30 && compareDodge < 40)
                {
                    newDodge += 2;
                }
                else if (compareDodge >= 50)
                {
                    newDodge += 1;
                }
                //NEW end

                dodgeStack.Push(newDodge);
                int currentDodge = (int)dodgeStack.Peek();
                player.SetDodge(currentDodge);
            }

            if (Input.GetAxis("R2") > 0 && Input.GetButtonDown("Circle") && power > 0 && dexterityCounter < dexterity) //DEXTERITY: hit and dodge
            {
                powerUpSound.Play();
                power -= 1;
                dexterityCounter += 1;

                //boostThree += 0.10f;

                //OLD start
                //int hitBoost = (int)Math.Ceiling(baseHit * boostThree);
                //int newHit = baseHit + hitBoost;
                //OLD end

                //NEW start
                int compareHit = player.GetHit();
                int newHit = player.GetHit();
                if (compareHit < 60)
                {
                    newHit += 6;
                }
                if (compareHit >= 60 && compareHit < 70)
                {
                    newHit += 5;
                }
                else if (compareHit >= 70 && compareHit < 80)
                {
                    newHit += 4;
                }
                else if (compareHit >= 80 && compareHit < 90)
                {
                    newHit += 3;
                }
                else if (compareHit >= 90)
                {
                    newHit += 2;
                }
                //NEW end

                hitStack.Push(newHit);
                int currentHit = (int)hitStack.Peek();
                player.SetHit(currentHit);

                //Dodge
                //float boostFour = boostThree * 2.0f; //dodge boost, has to be higher because dodge is so low

                //OLD start
                //int dodgeBoost = (int)Math.Ceiling(baseDodge * boostFour);
                //int newDodge = baseDodge + dodgeBoost;
                //OLD end

                //NEW start
                int compareDodge = player.GetDodge();
                int newDodge = player.GetDodge();
                if (compareDodge >= 0 && compareDodge < 10)
                {
                    newDodge += 5;
                }
                if (compareDodge >= 10 && compareDodge < 20)
                {
                    newDodge += 4;
                }
                else if (compareDodge >= 20 && compareDodge < 30)
                {
                    newDodge += 3;
                }
                else if (compareDodge >= 30 && compareDodge < 40)
                {
                    newDodge += 2;
                }
                else if (compareDodge >= 50)
                {
                    newDodge += 1;
                }
                //NEW end

                dodgeStack.Push(newDodge);
                int currentDodge = (int)dodgeStack.Peek();
                player.SetDodge(currentDodge);
            }

            if (Input.GetKeyDown("e")) //Power down DEXTERITY
            {
                if (hitStack.Count > 1)
                {
                    powerDownSound.Play();
                    power += 1;
                    dexterityCounter -= 1;
                    //boostThree -= 0.10f;

                    hitStack.Pop();
                    int currentHit = (int)hitStack.Peek();
                    player.SetHit(currentHit);
                }

                if (dodgeStack.Count > 1)
                {
                    dodgeStack.Pop();
                    int currentDodge = (int)dodgeStack.Peek();
                    player.SetDodge(currentDodge);
                }
            }

            if (Input.GetAxis("L2") > 0 && Input.GetButtonDown("Circle")) //Power down DEXTERITY
            {
                if (hitStack.Count > 1)
                {
                    powerDownSound.Play();
                    power += 1;
                    dexterityCounter -= 1;
                    boostThree -= 0.10f;

                    hitStack.Pop();
                    int currentHit = (int)hitStack.Peek();
                    player.SetHit(currentHit);
                }

                if (dodgeStack.Count > 1)
                {
                    dodgeStack.Pop();
                    int currentDodge = (int)dodgeStack.Peek();
                    player.SetDodge(currentDodge);
                }
            }
        }
    }
}
