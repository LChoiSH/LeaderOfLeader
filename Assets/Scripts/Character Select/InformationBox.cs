using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;

public class InformationBox : MonoBehaviour
{
    public TextMeshProUGUI linkText;
    public string currentText = "";
    [SerializeField] private float writeSpeed = 0.03f;
    private Coroutine playingWriting;

    void Start()
    {
        linkText = GameObject.Find("Info Text").GetComponent<TextMeshProUGUI>();
    }

    public void OpenBox(string text)
    {
        currentText = text;

        if (playingWriting != null) StopCoroutine(playingWriting);
        playingWriting = StartCoroutine(WriteText());
    }

    IEnumerator WriteText()
    {
        linkText.text = "";
        int index = 0;

        while(index < currentText.Length)
        {
            yield return new WaitForSeconds(writeSpeed);

            linkText.text = linkText.text + currentText[index++];
        }
    }
}
