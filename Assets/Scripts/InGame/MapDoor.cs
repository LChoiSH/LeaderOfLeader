using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDoor : MonoBehaviour
{

    public bool isOpen = false;
    float doorSpeed = 5.0f;
    AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();

    }

    private void Update()
    {
        if (isOpen)
        {
            if (transform.position.x > -7)
            {
                transform.Translate(Vector3.left * doorSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (transform.position.x < 0)
            {
                transform.Translate(Vector3.right * doorSpeed * Time.deltaTime);
            }
        }
    }

    public void CloseDoor()
    {
        isOpen = false;
        audio.Play();
    }

    public void OpenDoor()
    {
        isOpen = true;
        audio.Play();

        
    }
}
