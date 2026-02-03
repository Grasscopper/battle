using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int position;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move()
    {
        int spacesAway = position - PlayerCharacter.position;
        if (Dice.rolled >= spacesAway)
        {
            GameObject.Find("Player").transform.position = gameObject.transform.position;
            PlayerCharacter.position = position;
        }
        else
        {
            Debug.Log("Too far away.");
        }
    }
}
