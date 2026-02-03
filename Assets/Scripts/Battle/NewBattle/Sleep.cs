using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sleep : MonoBehaviour
{
    bool animating = false;
    float timer;

    Text awakeText;
    AudioSource awakeSound;
    // Start is called before the first frame update
    void Start()
    {
        awakeText = GameObject.Find("AwakeText").GetComponent<Text>();
        awakeSound = GameObject.Find("AwakeSound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animating)
        {
            timer += Time.deltaTime;

            awakeText.transform.position = new Vector2(-439, -211);
            awakeText.transform.Translate(Vector2.up * Time.deltaTime * 10);
            if (timer >= 0.5f)
            {
                animating = false;
                timer = 0;

                awakeText.transform.position = new Vector2(439, 211);
            }
        }
    }

    public void Awaken(Character character) //action to animate AWAKE display and awaken the enemy from being asleep
    {
        if (character.GetHealth() > 0)
        {
            //enemy asleep property is false, sprite color back to normal, replenish psyche, reset psyche text to max psyche
            character.WakeUp(); //enemy asleep property is false
            character.GetCharacterSprite().color = Color.white; //awake color, characterSprite of enemy set in Player.cs, this will refer to it
            character.SetPsyche(character.GetMaxPsyche()); //replenish psyche, maxEnemyPsyche set in Enemy.cs
            character.GetPsycheText().text = $"{character.GetMaxPsyche()}/{character.GetMaxPsyche()}"; //display restored psyche, psycheText set in Enemy.cs

            awakeSound.Play();
            animating = true;
        }
    }
}
