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

    [SerializeField] float boundPower = 500.0f;

    protected float speed = 6.0f;

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
            Destroy(gameObject);
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
    }

    private void FixedUpdate()
    {
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
}
