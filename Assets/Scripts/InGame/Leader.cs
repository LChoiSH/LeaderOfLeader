using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class Leader : MonoBehaviour
{
    // active Skill
    public float skillTime = 10.0f;
    public float currentSkillTime = 0;

    void Start()
    {
        CharacterInfo leaderCharacter = DataManager.instance.characterDictionary[DataManager.instance.currentCharacter];
    }

    private void Update()
    {
        if(currentSkillTime <= 0)
        {
            DoSKill();
        }
    }

    protected abstract void Skill();

    protected virtual void DoSKill()
    {
        if (currentSkillTime > 0) return;

        StartCoroutine(SkillTimeCheck());
        Skill();
    }

    IEnumerator SkillTimeCheck()
    {
        currentSkillTime = skillTime;

        while (currentSkillTime > 0)
        {
            currentSkillTime -= (Time.deltaTime);
            Mathf.Clamp(currentSkillTime, 0, skillTime);
            yield return null;
        }
    }
}
