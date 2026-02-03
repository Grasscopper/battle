using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guard : MonoBehaviour
{
    Text guardText;
    // Start is called before the first frame update
    void Start()
    {
        guardText = GetComponentInChildren<Text>();
        guardText.text += " - ";
        guardText.text += PlayerControllerBattle.defense;
        guardText.text += " defense";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}