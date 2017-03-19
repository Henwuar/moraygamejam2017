using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float scrollSpeed;
    public int minionValue;
    public float treasureValue;

    private GameObject treasure;
    private bool gameOver = false;

    private Canvas canvas;
    private RectTransform childImage;
    private int endState = 0;
    private float targetY;

    public Text minionDisplay;
    public Text treasureDisplay;
    public Text totalDisplay;
    private int numMinions = 0;

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

        childImage = transform.GetChild(0).GetComponent<RectTransform>();
        targetY = childImage.anchoredPosition.y;
        childImage.anchoredPosition = new Vector2(childImage.anchoredPosition.y, -childImage.sizeDelta.y);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(gameOver)
        {
            canvas.enabled = true;        
            switch(endState)
            {
                case 0:
                    Stage0();
                    break;
                case 1:
                    Stage1();
                    break;
                case 2:
                    Stage2();
                    break;
                default:
                    break;
            }
        }
	}

    void Stage0()
    {
        Vector2 pos = childImage.anchoredPosition;
        if(Mathf.Abs(targetY - childImage.anchoredPosition.y) > scrollSpeed)
        {
            pos.y += scrollSpeed;
            
        }
        else
        {
            pos.y = targetY;
            endState++;
        }


        if (Input.GetButtonDown("Submit"))
        {
            pos.y = targetY;
            endState++;
        }

        childImage.anchoredPosition = pos;
    }

    void Stage1()
    {
        int minions = GameObject.Find("Spawn").GetComponent<MinionSpawner>().NumMinions();

        int value;// = numMinions * minionValue;

        if (minions - numMinions <= 0)
        {
            value = minions * minionValue;
            minionDisplay.text = "$" + string.Format("{0:n0}", value);
            endState++;
        }
        else
        {
            numMinions++;
        }

        value = numMinions * minionValue;

        minionDisplay.text = "-$" + string.Format("{0:n0}", value);

        totalDisplay.text = "$" + string.Format("{0:n0}", treasureValue - value);
        treasureDisplay.text = "$" + string.Format("{0:n0}", treasureValue);

        if (Input.GetButtonDown("Submit"))
        {
            value = minions * minionValue;
            minionDisplay.text = "$" + string.Format("{0:n0}", value);
            endState++;
        }
    }

    void Stage2()
    {
        if(Input.GetButtonDown("Submit"))
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("Menu");
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
