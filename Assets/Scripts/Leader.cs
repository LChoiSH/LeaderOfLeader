using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leader : Member
{
    // turn
    [SerializeField] float turnSpeed = 100.0f;

    private Rigidbody selfRb;
    public bool isCollide = false;
    [SerializeField] float boundPower = 1000.0f;

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

        CharacterInfo leaderCharacter = DataManager.instance.characterDictionary[DataManager.instance.currentCharacter];

        if(DataManager.instance.currentCharacter == null)
        {
            leaderCharacter = DataManager.instance.characterDictionary.ElementAt(0).Value;
        }

        SetCharacterInfo(leaderCharacter);
        selfRb = GetComponent<Rigidbody>();
        activeSkillButton = GameObject.Find("Active Skill Button").GetComponent<Button>();
        skillTimeTextWrap = GameObject.Find("Skill Time");
        skillTimeTextWrap.SetActive(false);
        skillTimeText = skillTimeTextWrap.GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.Space) && currentSkillTime <= 0)
        {
            StartCoroutine(TimeCheck());
            Skill();
        }
    }

    protected virtual void Skill()
    {
        currentSkillTime = skillTime;
    }

    override protected void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

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

        float currentDirection = transform.eulerAngles.y;
        float direction = currentDirection;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction = 0;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = 90;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            direction = 180;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction = 270;
        }

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
