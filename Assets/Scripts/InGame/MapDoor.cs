using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDoor : MonoBehaviour
{
    float doorSpeed = 5.0f;
    public bool isInDoor;

    // Update is called once per frame
    void Update()
    {
        if (!GameController.instance.isGameActive) return;

        if (!GameController.instance.isClear)
        {
            if (!isInDoor) return;
            if (transform.position.x < 0)
            {
                transform.Translate(Vector3.right * doorSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (isInDoor) return;
            if (transform.position.x > -7)
            {
                transform.Translate(Vector3.left * doorSpeed * Time.deltaTime);
            }
        }
    }
}
