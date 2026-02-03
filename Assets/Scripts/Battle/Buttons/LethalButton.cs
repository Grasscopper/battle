using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LethalButton : MonoBehaviour
{
    Text LethalText;
    // Start is called before the first frame update
    void Start()
    {
        LethalText = GetComponentInChildren<Text>();
        LethalText.text += " - ";
        LethalText.text += PlayerControllerBattle.amRange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
