using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BountyHunter : MonoBehaviour
{
    public static int blue = 3;
    public static int red = 3;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ReduceTicket(string team)
    {
        if (team == "blue")
        {
            blue -= 1;
            if (blue <= 0)
            {
                blue = 0;
            }
            GameObject.Find("BlueTicketCount").GetComponent<Text>().text = blue.ToString();
        }
        else if (team == "red")
        {
            red -= 1;
            if (red <= 0)
            {
                red = 0;
            }
            GameObject.Find("RedTicketCount").GetComponent<Text>().text = red.ToString();
        }
    }

    public static void Fulton(string team, int bounty)
    {
        if (team == "blue")
        {
            ReduceTicket("blue");
            int newTicketCount = red;
            newTicketCount += bounty;
            red = newTicketCount;
        }
        else if (team == "red")
        {
            ReduceTicket("red");
            int newTicketCount = blue;
            newTicketCount += bounty;
            blue = newTicketCount;
        }
    }
}
