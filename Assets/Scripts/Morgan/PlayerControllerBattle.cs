
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBattle : MonoBehaviour
{
    Animator anim;
    bool wuPistolShoot;
    public static int startX = 1; //change this for starting position
    public static int startY = 1; //change this for starting position
    public static int move = 3; //change this to change Morgan's movement spaces
    public static int wuRange = 3; //change this to change range of Morgan's Wu Pistol
    public static int amRange = 3; //change this to change range of Morgan's AM MRS-4 Rifle
    public static int fultonRange = 1;
    public static int damageHealth = 20;
    public static int stunHealth = 3;
    public static int defense = 4;
    public static bool guarding = false;
    public static int bounty = 1;
    float speed = 10; //change this to change movement speed
    public static float horizontalInput;
    public static float verticalInput;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        wuPistolShoot = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}