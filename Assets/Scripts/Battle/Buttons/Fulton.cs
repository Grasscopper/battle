using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fulton : MonoBehaviour
{
    Text fultonText;
    // Start is called before the first frame update
    void Start()
    {
        fultonText = GetComponentInChildren<Text>();
        fultonText.text += " - ";
        fultonText.text += PlayerControllerBattle.fultonRange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}