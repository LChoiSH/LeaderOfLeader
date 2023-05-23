using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    public string characterName;
    Animator animator;
    public SelectCharacter[] characters;
    public GameObject selectRing;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Transform selectRingTransform = transform.Find("Selection Ring");
        if(selectRingTransform != null)
        {
            selectRing = selectRingTransform.gameObject;
        }
    }

    private void OnMouseDown()
    {
        DataManager.instance.currentCharacter = characterName;

        OnSelected();
    }

    private void OnSelected()
    {
        foreach(SelectCharacter characterOption in characters)
        {
            characterOption.animator.SetFloat("Speed_f", 0);
            characterOption.selectRing.SetActive(false);
        }

        animator.SetFloat("Speed_f", 10.0f);
        selectRing.SetActive(true);
    }
}
