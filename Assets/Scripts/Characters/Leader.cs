using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Leader : Member
{
    // turn
    [SerializeField] float turnSpeed = 100.0f;

    // joystick
    public VariableJoystick variableJoystick;

    private Rigidbody selfRb;
    public bool isCollide = false;
    [SerializeField] float boundPower = 1000.0f;

    // move
    private float currentDirection, direction, angle;

    // active Skill
    public Button activeSkillButton;
    public GameObject skillTimeTextWrap;
    public TMP_Text skillTimeText;
    protected float skillTime = 10.0f;
    protected float currentSkillTime = 0;

    public Vector3  startingPoint = Vector3.zero;

    override protected void Start()
    {
        base.Start();
        transform.position = startingPoint;

        CharacterInfo leaderCharacter = DataManager.instance.characterDictionary[DataManager.instance.currentCharacterId];

        variableJoystick = GameObject.Find("Joystick").GetComponent<VariableJoystick>();

        SetCharacterInfo(leaderCharacter);
        selfRb = GetComponent<Rigidbody>();
        activeSkillButton = GameObject.Find("Active Skill Button").GetComponent<Button>();
        skillTimeTextWrap = GameObject.Find("Skill Time");
        skillTimeTextWrap.SetActive(false);
        skillTimeText = skillTimeTextWrap.GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentSkillTime <= 0)
        {
            StartCoroutine(TimeCheck());
            Skill();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    protected virtual void Skill()
    {
        currentSkillTime = skillTime;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bound") || other.CompareTag("Prison"))
        {
            StartCoroutine(boundCollide());
            selfRb.AddForce(-transform.forward * boundPower);
        }

        if (currentHp <= 0)
        {
            gameController.GameOver();
        }
    }

    void MovePlayer()
    {
        if (!gameController.isGameActive) return;
        if (isCollide) return;

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        currentDirection = transform.eulerAngles.y;

        if (variableJoystick.Direction.x == 0 && variableJoystick.Direction.y == 0) return ;

        angle = Mathf.Atan2(variableJoystick.Direction.x, variableJoystick.Direction.y) * Mathf.Rad2Deg;
        if (angle < 0)
        {
            angle += 360; // 각도를 0도에서 360도로 변환
        }

        direction = angle;

        currentDirection = (currentDirection + 360 - direction) % 360;

        if (transform.eulerAngles.y != direction)
        {
            if (180 < currentDirection)
            {
                transform.Rotate(transform.up, Time.deltaTime * turnSpeed);
            }
            else
            {
                transform.Rotate(-transform.up, Time.deltaTime * turnSpeed);
            }
        }
    }

    protected IEnumerator boundCollide()
    {
        isCollide= true;
        yield return new WaitForSeconds(1);
        isCollide = false;
    }

    private IEnumerator TimeCheck()
    {
        skillTimeTextWrap.SetActive(true);
        currentSkillTime = skillTime;
        skillTimeText.text = currentSkillTime.ToString() + "s";

        while (currentSkillTime > 0)
        {
            yield return new WaitForSeconds(1);
            currentSkillTime -= 1;
            skillTimeText.text = currentSkillTime.ToString() + "s";
        }

        skillTimeTextWrap.SetActive(false);
    }
}
