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

    protected virtual void Start()
    {
        Debug.Log("leader start");
        CharacterInfo leaderCharacter = DataManager.instance.characterDictionary[DataManager.instance.currentCharacter];
        skillTime = leaderCharacter.skillTime;

        Debug.Log(leaderCharacter.name + " " + leaderCharacter.skillTime);
    }

    private void Update()
    {
        DoSKill();
    }

    protected abstract void Skill();

    protected void DoSKill()
    {
        if (!GameController.instance.isGameActive) return;

        if (currentSkillTime <= 0 && !GameController.instance.isClear)
        {
            currentSkillTime = skillTime;
            Skill();
        }
        else
        {
            currentSkillTime -= Time.deltaTime;
        }
    }
}
