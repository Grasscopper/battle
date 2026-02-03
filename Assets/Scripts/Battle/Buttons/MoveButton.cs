using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveButton : MonoBehaviour
{
    Text moveText;
    // Start is called before the first frame update
    void Start()
    {
        moveText = GetComponentInChildren<Text>();
        moveText.text += " - ";
        moveText.text += PlayerControllerBattle.move;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
