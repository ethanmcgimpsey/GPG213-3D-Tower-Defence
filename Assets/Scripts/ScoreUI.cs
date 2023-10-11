using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public Text scoreText;

    void Update()
    {
        scoreText.text = PlayerStats.Score.ToString() + " :Points";
    }
}
