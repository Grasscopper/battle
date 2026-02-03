using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NonLethalButton : MonoBehaviour
{
    Text nonLethalText;
    // Start is called before the first frame update
    void Start()
    {
        nonLethalText = GetComponentInChildren<Text>();
        nonLethalText.text += " - ";
        nonLethalText.text += PlayerControllerBattle.wuRange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
