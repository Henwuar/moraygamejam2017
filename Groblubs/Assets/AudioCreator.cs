using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioCreator : MonoBehaviour
{
    
	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Menu");
	}

    void Update()
    {
        print(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            if(Input.GetButtonDown("Submit"))
            {
                SceneManager.LoadScene("Spikes");
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }
}
