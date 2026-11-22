using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public PersistanceHandler persistanceHandler;
    public TMP_Text highScore;

    // Start is called before the first frame update
    void Start()
    {
        highScore.text = "HighScore: " + persistanceHandler.GetData().highScore.ToString("0.00") + "k";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene("Space1");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
