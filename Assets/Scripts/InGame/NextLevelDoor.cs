using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelDoor : MonoBehaviour
{
    public Light guideLight;
    private float maxLightIntensity = 7.0f;
    public Collider selfCollider;
    public GameController gameController;
    public RewardManager rewardManager;

    void Start()
    {
        guideLight = GetComponentInChildren<Light>();
        guideLight.intensity = 0;

        selfCollider= GetComponentInChildren<Collider>();
        selfCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        try
        {
            if (other.gameObject.CompareTag("Member") || other.transform.parent.CompareTag("Member"))
            {
                rewardManager.gameObject.SetActive(true);
                rewardManager.RewardScreenIn();
                selfCollider.enabled = false;
            }
        } catch
        {
        }
    }

    public void LightOn()
    {
        StartCoroutine(LightOnIenumerator());
    }

    IEnumerator LightOnIenumerator()
    {
        float onDuration = 3.0f;
        float currentTime = 0;

        selfCollider.enabled = true;

        while (currentTime < onDuration || guideLight.intensity < maxLightIntensity)
        {
            guideLight.intensity = Mathf.Clamp(currentTime / onDuration, 0f, 1f) * maxLightIntensity;
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
