using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MovePlayer : MonoBehaviour
{
    public static Leader instance;

    // turn
    [SerializeField] float turnSpeed = 100.0f;

    // bound
    private Rigidbody selfRb;
    public bool isCollide = false;
    [SerializeField] float boundPower = 500.0f;

    protected float speed = 6.0f;

    // move
    public VariableJoystick variableJoystick;
    private float inputAngle, subAngle;
    Vector3 turnDirection;

    void Start()
    {
        variableJoystick = GameObject.Find("Joystick").GetComponent<VariableJoystick>();

        selfRb = GetComponent<Rigidbody>();
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
        if (isCollide) return;

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (!GameController.instance.isGameActive) return;
        if (variableJoystick.Direction.x == 0 && variableJoystick.Direction.y == 0) return;

        inputAngle = Mathf.Atan2(variableJoystick.Direction.x, variableJoystick.Direction.y) * Mathf.Rad2Deg;

        subAngle = (transform.eulerAngles.y + 360 - inputAngle) % 360;
        turnDirection = (subAngle > 180 ? transform.up : -transform.up);

        transform.Rotate(turnDirection, Time.deltaTime * turnSpeed);
    }
}
