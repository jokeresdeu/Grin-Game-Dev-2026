using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsScript : MonoBehaviour
{

    public Text shadowText;

    // Use this for initialization
    void Start()
    {
        string text = "Coins: " + PlayerPrefsManager.GetNumOfCoins();
        Text myText = GetComponent<Text>();
        if (myText != null)
            myText.text = text;
        if (shadowText != null)
            shadowText.text = text;
    }
}
