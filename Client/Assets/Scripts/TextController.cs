using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    public static string text = "";

    public static string leaderboardText = "";

    public static string winnerText = "";

    public static bool gameReset;

    public Text resetNotice;

    public GameObject fireworkPrefab;

    Text outputText;

    Text winText;

    Text leaderboard;

    void Awake()
    {
        outputText = GameObject.Find("Output").GetComponent<Text>();
        winText = GameObject.Find("Winner Corner").GetComponent<Text>();
        leaderboard = GameObject.Find("Leaderboard").GetComponent<Text>();
    }
    void Update()
    {
        if (winText.text == "Username must be longer than 3 characters!" && UDPNetwork.nameSet)
        {
            winText.text = "";
        }

        if (text.Length > 0 && text.Length < 16)
        {
            outputText.text = text;
            outputText.color = Color.white * 10;
        }
        else if(text == "Username must be longer than 3 characters!")
        {
            winText.text = text;
            winText.color = Color.white * 5;
        }
        
        if (gameReset)
        {
            winText.text = winnerText;
            winText.color = Color.white * 25;
            resetNotice.color = Color.white * 10;
            Instantiate(fireworkPrefab, new Vector3(0, 3, 0), Quaternion.identity);
            gameReset = false;
        }

        leaderboard.text = leaderboardText;
        text = "";
        resetNotice.color -= new Color(0, 0, 0, 0.01f);
        outputText.color -= new Color(0, 0, 0, 0.01f);
        winText.color -= new Color(0, 0, 0, 0.01f);
    }
}
