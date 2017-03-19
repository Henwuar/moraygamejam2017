using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPortal : MonoBehaviour
{
    public float rotationSpeed1;
    public float rotationSpeed2;
    public float scaleInSpeed;
    public float scaleOutSpeed;
    public float lifetime;

    public Transform quad1;
    public Transform quad2;
    public ParticleSystem particles;

    private float quad1Scale;
    private float quad2Scale;

    private float speed;
    [SerializeField]
    private Queue<float> targetScales;
    private bool inOut = false;
    private float angle1;
    private float angle2;
    private float curScale;
    private float lifeTimer;


    public delegate void PortalOpened();
    public static event PortalOpened OnPortalOpen;

    // Use this for initialization
    void Start ()
    {
        targetScales = new Queue<float>();
        lifeTimer = lifetime;
        curScale = 0;
        quad1Scale = quad1.localScale.x;
        quad2Scale = quad2.localScale.x;
        //Play();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //update the scale
        if (targetScales.Count > 0)
        {
            if (Mathf.Abs(targetScales.Peek() - curScale) > speed * Time.deltaTime)
            {
                curScale = Mathf.Lerp(curScale, targetScales.Peek(), speed * Time.deltaTime);
            }
            else
            {
                print(targetScales.Peek());
                curScale = targetScales.Dequeue();
            }
        }
        else
        {
            if(inOut)
            {
                OnPortalOpen();
                if (lifeTimer > 0)
                {
                    lifeTimer -= Time.deltaTime;
                }
                else
                {
                    PlayBackDown();
                }
            }
        }
        quad1.localScale = new Vector3(quad1Scale * curScale, quad1Scale * curScale, quad1Scale * curScale);
        quad2.localScale = new Vector3(quad2Scale * curScale, quad2Scale * curScale, quad2Scale * curScale);

        //update rotation
        angle1 += rotationSpeed1 * Time.deltaTime;
        angle2 += rotationSpeed2 * Time.deltaTime;

        quad1.localRotation = Quaternion.Euler(quad1.localEulerAngles.x, quad1.localEulerAngles.y, angle1);
        quad2.localRotation = Quaternion.Euler(quad2.localEulerAngles.x, quad2.localEulerAngles.y, angle2);
    }

    public void Play()
    {
        if (!inOut)
        {
            particles.Play();
            inOut = true;
            targetScales.Enqueue(1.2f);
            targetScales.Enqueue(1.0f);
            speed = scaleInSpeed;
            lifeTimer = lifetime;
        }
    }

    void PlayBackDown()
    {
        inOut = false;
        //targetScales.Enqueue(1.2f);
        targetScales.Enqueue(0.0f);
        speed = scaleOutSpeed;
    }
}
