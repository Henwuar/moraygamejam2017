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
        SceneManager.LoadScene("Spikes");
	}
}
