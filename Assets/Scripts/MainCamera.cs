using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MainCamera : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset = new Vector3(0, 17.0f, -8.0f);
    public float speed = 200.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
