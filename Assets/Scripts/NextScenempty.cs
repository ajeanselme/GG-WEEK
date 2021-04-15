using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScenempty : MonoBehaviour
{
    public int currentLevel;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if(currentLevel == 1)
            {
                SceneManager.LoadScene("VictoryScreen1");
            } else
            {
                SceneManager.LoadScene("VictoryScreen2");

            }
        }
    }
}
