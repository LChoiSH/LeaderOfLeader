using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    public string characterName;
    Animator animator;
    public SelectCharacter[] characters;
    public GameObject selectRing;
    public GameStartButton gameStartButton;

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
        if (DataManager.instance.isLoading) return;

        OnSelected();
    }

    private void OnSelected()
    {

        foreach (SelectCharacter characterOption in characters)
        {
            characterOption.animator.SetFloat("Speed_f", 0);
            characterOption.selectRing.SetActive(false);
        }

        animator.SetFloat("Speed_f", 10.0f);
        selectRing.SetActive(true);
    }

}
