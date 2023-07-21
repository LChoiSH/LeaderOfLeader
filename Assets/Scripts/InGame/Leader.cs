using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Leader : MonoBehaviour
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

    // active Skill
    public Button activeSkillButton;
    public GameObject skillTimeTextWrap;
    public RawImage skillImage;
    public TMP_Text skillTimeText;
    public float skillTime = 10.0f;
    public float currentSkillTime = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

     void Start()
    {
        CharacterInfo leaderCharacter = DataManager.instance.characterDictionary[DataManager.instance.currentCharacter];

        variableJoystick = GameObject.Find("Joystick").GetComponent<VariableJoystick>();

        selfRb = GetComponent<Rigidbody>();

        // skill setting
        activeSkillButton = GameObject.Find("Active Skill Button").GetComponent<Button>();
        skillImage = GameObject.Find("Skill Image").GetComponent<RawImage>();

        string skillImagePath = "Image/Skill/" + leaderCharacter.skillImage;
        Texture2D imageTexture = Resources.Load<Texture2D>(skillImagePath);
        
        skillImage.texture = imageTexture;

        skillTimeTextWrap = GameObject.Find("Skill Time");
        skillTimeTextWrap.SetActive(false);
        skillTimeText = skillTimeTextWrap.GetComponentInChildren<TMP_Text>();

        activeSkillButton.onClick.AddListener(DoSkill);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public virtual void DoSkill()
    {
        if (currentSkillTime > 0) return;

        StartCoroutine(SkillTimeCheck());
    }

    private IEnumerator SkillTimeCheck()
    {
        skillTimeTextWrap.SetActive(true);
        currentSkillTime = skillTime;
        skillTimeText.text = currentSkillTime.ToString();

        while (currentSkillTime > 0)
        {
            yield return new WaitForSeconds(1);
            currentSkillTime -= 1;
            skillTimeText.text = currentSkillTime.ToString();
        }

        skillTimeTextWrap.SetActive(false);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (!GameController.instance.isGameActive) return;
        if (other.CompareTag("Bound") || other.CompareTag("Prison"))
        {
            StartCoroutine(boundCollide());
            selfRb.AddForce(-transform.forward * boundPower);
        }
    }

    void MovePlayer()
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

    protected IEnumerator boundCollide()
    {
        isCollide= true;
        yield return new WaitForSeconds(1);
        isCollide = false;
    }
}
