using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Infiltrator(Player player)
    {
        player.SetHealth(80);
        player.SetPsyche(40);
        player.SetStealthCoins(12);
    }
}
