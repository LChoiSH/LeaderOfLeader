using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDoor : MonoBehaviour
{

    public bool isOpen = false;
    float doorSpeed = 5.0f;
    AudioSource audio;
    Vector3 position;

    private void Start()
    {
        audio = GetComponent<AudioSource>();

        position = transform.position;
    }

    private void Update()
    {
        if (isOpen)
        {
            if (transform.position.x > -6)
            {
                position.x = Mathf.Clamp(transform.position.x - doorSpeed * Time.deltaTime, -6, 0);
                transform.position = position;
            }
        }
        else
        {
            if (transform.position.x < 0)
            {
                position.x = Mathf.Clamp(transform.position.x + doorSpeed * Time.deltaTime, -6, 0);
                transform.position = position;
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
