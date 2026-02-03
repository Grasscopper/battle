using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dice : MonoBehaviour
{
    public static int rolled;
    public Text displayRoll;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Roll();
        }

        if (Input.GetKeyDown("h"))
        {
            SceneManager.LoadScene("NewBattle");
        }
    }

    void Roll()
    {
        rolled = Random.Range(1, 7);
        displayRoll.text = rolled.ToString();
    }
}
