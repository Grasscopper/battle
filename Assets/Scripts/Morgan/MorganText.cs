using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Takes dialogue from MorganDialogue, and displays it on the MorganTextPanel
public class MorganText : MorganDialogue
{
    Text morgan;
    int index;
    GameObject textPanel;
    string[] savedDialogue;
    
    // Start is called before the first frame update
    void Start()
    {
        morgan = GetComponent<Text>();
        index = 0;
        textPanel = GameObject.Find("MorganTextPanel");
    }

    // Update is called once per frame
    void Update()
    {
        talk(haldomWelcome);
     
        void talk(string[] dialogue)
        {
            morgan.text = dialogue[index];
            savedDialogue = dialogue;
        }

        if (Input.GetKeyDown("space"))
        {
            index++;
            if (index == savedDialogue.Length)
            {
                Destroy(textPanel);
                index = 0;
            }
            else
            {
                talk(savedDialogue);
            }
        }
    }
}
