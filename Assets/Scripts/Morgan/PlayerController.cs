using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator anim;
    bool wuPistolShoot;
    float speed = 10;
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
        void idle()
        {
            anim.SetBool("right", false);
            anim.SetBool("left", false);
            anim.SetBool("up", false);
            anim.SetBool("down", false);
            anim.SetBool("wu_pistol_shoot", false);
        }
        horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput > 0)
        {
            transform.Translate(Vector2.right * Time.deltaTime * horizontalInput * speed);
            anim.Play("MainCharacter_Walk_Right");
            anim.SetBool("right", true);

        }
        else if (horizontalInput < 0)
        {
            transform.Translate(Vector2.right * Time.deltaTime * horizontalInput * speed);
            anim.Play("MainCharacter_Walk_Left");
            anim.SetBool("left", true);
        }

        verticalInput = Input.GetAxis("Vertical");
        if (verticalInput > 0)
        {
            transform.Translate(Vector2.up * Time.deltaTime * verticalInput * speed);
            if (horizontalInput == 0)
            {
                anim.Play("MainCharacter_Walk_Up");
                anim.SetBool("up", true);
            }
        }
        else if (verticalInput < 0)
        {
            transform.Translate(Vector2.up * Time.deltaTime * verticalInput * speed);
            if (horizontalInput == 0)
            {
                anim.Play("MainCharacter_Walk_Down");
                anim.SetBool("down", true);
            }
        }

        if (verticalInput == 0 && horizontalInput == 0)
        {
            if (wuPistolShoot == false)
            {
                anim.Play("MainCharacter_Idle_Down");
            }

            //anim.SetBool("right", false);
            //anim.SetBool("left", false);
            //anim.SetBool("up", false);
            //anim.SetBool("down", false);
            idle();
            //anim.SetBool("wu_pistol_shoot", false);
            //if (Input.GetKeyDown("space"))
            //{
              //  anim.Play("MainCharacter_Wu_Pistol_Right");
                //anim.SetBool("wu_pistol_shoot", true);
                //anim.SetBool("right", false);
                //anim.SetBool("left", false);
                //anim.SetBool("up", false);
                //anim.SetBool("down", false);
                //idle();

            //}
        }
        if (Input.GetKeyDown("space"))
        {
            anim.Play("MainCharacter_Wu_Pistol_Right");
            anim.SetBool("wu_pistol_shoot", true);
            wuPistolShoot = true;
        }
    }
}
