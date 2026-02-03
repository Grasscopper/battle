using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stealth : MonoBehaviour
{
    AudioSource noPointsSound;
    Text log;

    // Start is called before the first frame update
    void Start()
    {
        noPointsSound = GameObject.Find("NoPointsSound").GetComponent<AudioSource>();
        log = GameObject.Find("Log").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    bool StealthCheck(Character character)
    {
        if (character.GetStealth() && character.GetStealthCoins() < 0)
        {
            noPointsSound.Play();
            log.text = "Not enough coins to attack in stealth";
            return false;
        }
        else
        {
            return true;
        }
    }
}
