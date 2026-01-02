using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class levelmanager : MonoBehaviour
{
    public GameObject score;
    public void sceneload(int level) {
        SceneManager.LoadScene(level);
    }
    private void Start()
    {
        if (score.activeSelf == true) {
            score.GetComponent<Text>().text = PlayerPrefs.GetInt("lastscore").ToString();
        }
    }
}
