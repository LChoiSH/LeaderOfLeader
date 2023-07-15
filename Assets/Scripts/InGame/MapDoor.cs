using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDoor : MonoBehaviour
{
    public GameController gameController;

    float doorSpeed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        if (!gameController.isGameActive) return;

        if (!gameController.isClear)
        {
            if (transform.position.x < -2)
            {
                transform.Translate(Vector3.right * doorSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (transform.position.x > -8.5)
            {
                transform.Translate(Vector3.left * doorSpeed * Time.deltaTime);
            }
        }
    }
}
