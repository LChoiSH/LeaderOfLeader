using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MovePlayer : MonoBehaviour
{
    public static MovePlayer instance;

    public Member member;

    // turn
    [SerializeField] float turnSpeed = 100.0f;
    public float speed = 4.0f;

    // move
    public VariableJoystick variableJoystick;
    private float inputAngle, subAngle;
    Vector3 turnDirection;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        member = GetComponent<Member>();

        variableJoystick = GameObject.Find("Joystick").GetComponent<VariableJoystick>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (!GameController.instance.isGameActive) return;
        if (other.CompareTag("Environment"))
        {
            Vector3 incidentVector = transform.forward; // 입사
            Vector3 normalVector = other.transform.forward; // 법선

            Vector3 reflectionVector = Vector3.Reflect(incidentVector, normalVector);

            transform.rotation = Quaternion.LookRotation(reflectionVector);
        }
    }

    void Move()
    {
        if (member != null && member.GetCurrentHp() <= 0) return ;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (!GameController.instance.isGameActive) return;
        if (variableJoystick.Direction.x == 0 && variableJoystick.Direction.y == 0) return;

        inputAngle = Mathf.Atan2(variableJoystick.Direction.x, variableJoystick.Direction.y) * Mathf.Rad2Deg;

        subAngle = (transform.eulerAngles.y + 360 - inputAngle) % 360;
        turnDirection = (subAngle > 180 ? transform.up : -transform.up);

        transform.Rotate(turnDirection, Time.deltaTime * turnSpeed);
    }
}
