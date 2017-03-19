using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    public GameObject minion;
    public Color player1Colour;
    public Color player2Colour;
    public bool twoPlayer;

    private GameObject player1 = null;
    private GameObject player2 = null;

    void OnEnable()
    {
        EndGoalController.OnBulgeEnd += Spawn;
    }

    void OnDisable()
    {
        EndGoalController.OnBulgeEnd -= Spawn;
    }

	void Start()
    {
        GameObject.Find("EndGoal").GetComponent<EndGoalController>().StartCoroutine("BulgeBackward");
    }

    void Update()
    {
        if(player1 == null)
        {
            //SpawnMinion();
            GameObject.Find("EndGoal").GetComponent<EndGoalController>().StartCoroutine("BulgeBackward");
        }
        else
        {
            if(!player1.GetComponent<MinionMovement>().Controlled())
            {
                player1 = null;
            }
        }
        if (twoPlayer)
        {
            if (player2 == null)
            {
                GameObject.Find("EndGoal").GetComponent<EndGoalController>().StartCoroutine("BulgeBackward");
            }
            else
            {
                if (!player2.GetComponent<MinionMovement>().Controlled())
                {
                    player2 = null;
                }
            }
        }
    }

    public void SpawnMinion(int player = 1)
    {
        GameObject newMinion = Instantiate(minion, transform.position, Quaternion.LookRotation(-Vector3.up, Vector3.right)) as GameObject;
        newMinion.GetComponent<Rigidbody>().velocity = Vector3.right * Random.Range(2.0f, 5.0f);
        newMinion.GetComponent<MinionMovement>().player = player;
        Color color;
        if (player == 1)
        {
            color = player1Colour;
            player1 = newMinion;
        }
        else
        {
            color = player2Colour;
            player2 = newMinion;
        }

        newMinion.GetComponent<Renderer>().material.color = color;
    }

    void Spawn()
    {
        if (player1 == null)
        {
            SpawnMinion();
        }
        if (player2 == null && twoPlayer)
        {
            SpawnMinion(2);
        }
    }

    public GameObject Player(int player)
    {
        if(player == 1)
        {
            return player1;
        }
        else
        {
            return player2;
        }
    }
}
