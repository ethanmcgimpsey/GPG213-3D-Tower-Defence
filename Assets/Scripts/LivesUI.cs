using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    public Text baseHP;
    // Update is called once per frame
    void Update()
    {
        baseHP.text = PlayerStats.Lives.ToString() + " HP";
    }
}
