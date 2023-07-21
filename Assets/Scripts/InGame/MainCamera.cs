using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MainCamera : MonoBehaviour
{
    public static MainCamera instance;

    private GameObject player;
    [SerializeField]private Vector3 offset = new Vector3(0, 17.0f, -8.0f);

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { return; }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void LateUpdate()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
            return;
        }
        transform.position = player.transform.position + offset;
    }
}
