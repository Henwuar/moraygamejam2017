using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject treasure;
    private bool gameOver = false;

    private Canvas canvas;

    void OnEnable()
    {
        EndGoalController.OnBulgeEnd += EndLevel;
    }

    void OnDisable()
    {
        EndGoalController.OnBulgeEnd -= EndLevel;
    }

	// Use this for initialization
	void Start ()
    {
        treasure = GameObject.FindGameObjectWithTag("Treasure");
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(gameOver)
        {
            canvas.enabled = true;        
            if(Input.GetButtonDown("Submit"))
            {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene("main");
            }
        }
	}

    void EndLevel()
    {
        if(treasure)
        {
            return;
        }
        Time.timeScale = 0;
        gameOver = true;
    }
}
