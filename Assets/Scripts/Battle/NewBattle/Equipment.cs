using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public string weaponType;
    public int damage;
    public int stun;
    public int accuracy;
    public int ammo;
    public float weight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NonLethalLoadout(Player player)
    {
        player.SetDamage(20);
        player.SetStun(6);
    }

    public string GetWeaponType()
    {
        return weaponType;
    }

    public void SetWeaponType(string weaponType)
    {
        this.weaponType = weaponType;
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

    public int GetAccuracy()
    {
        return accuracy;
    }

    public void SetAccuracy(int accuracy)
    {
        this.accuracy = accuracy;
    }

    public int GetAmmo()
    {
        return ammo;
    }

    public void SetAmmo(int ammo)
    {
        this.ammo = ammo;
    }

    public float GetWeight()
    {
        return weight;
    }

    public void SetWeight(float weight)
    {
        this.weight = weight;
    }
}
