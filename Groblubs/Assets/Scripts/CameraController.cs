using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed;
    public float defaultDistance;
    public float distanceMultiplier;
    public float lookAhead;
    public float rise;
    public float lookSpeed;

    private Vector3 lookAtPos;
    private GameObject spawnPoint;

    private GameObject following;

	// Use this for initialization
	void Start ()
    {
        lookAtPos = transform.position + transform.forward;
        spawnPoint = GameObject.Find("Spawn");
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 averagePos = Vector3.zero;
        Vector3 averageForward = Vector3.zero;
        if (following)
        {
            averagePos = following.transform.position;
            averageForward = following.transform.forward;
        }

        GameObject[] minions = GameObject.FindGameObjectsWithTag("Minion");
        float maxDistance = defaultDistance;
        /*foreach(GameObject minion in minions)
        {
            {
                averagePos += minion.transform.position;
            }
        }
        if (minions.Length > 0)
        {
            averagePos /= minions.Length;
        }*/
        Vector3 player1Pos = Vector3.zero;
        Vector3 player2Pos = Vector3.zero;
        int foundPlayers = 1;
        if (spawnPoint.GetComponent<MinionSpawner>().Player(1))
        {
            following = spawnPoint.GetComponent<MinionSpawner>().Player(1);
        }
            /*if (spawnPoint.GetComponent<MinionSpawner>().Player(1))
            {
                foundPlayers++;
                player1Pos = spawnPoint.GetComponent<MinionSpawner>().Player(1).transform.position;
                averageForward += spawnPoint.GetComponent<MinionSpawner>().Player(1).transform.forward;
            }
            if (spawnPoint.GetComponent<MinionSpawner>().Player(2))
            {
                foundPlayers++;
                player2Pos = spawnPoint.GetComponent<MinionSpawner>().Player(2).transform.position;
                averageForward += spawnPoint.GetComponent<MinionSpawner>().Player(2).transform.forward;
            }*/

            float distance = Vector3.Distance(player1Pos, player2Pos);
        if (distance < defaultDistance || foundPlayers < 2)
            distance = defaultDistance;

        /*if (foundPlayers > 0)
        {
            averagePos = (player1Pos + player2Pos) / foundPlayers;
            averageForward /= foundPlayers;
        }
        else
        {
            averagePos = spawnPoint.transform.position;
        }*/
        averagePos += Vector3.up * rise;
        transform.position = Vector3.Lerp(transform.position, averagePos - Vector3.forward * distance * distanceMultiplier, moveSpeed * Time.deltaTime);
        lookAtPos = Vector3.Lerp(lookAtPos, averagePos + averageForward*lookAhead, lookSpeed * Time.deltaTime);
        transform.LookAt(lookAtPos);
	}
}
