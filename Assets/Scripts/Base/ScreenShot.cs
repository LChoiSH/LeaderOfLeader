using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    public string screenshotPath = "Screenshots";

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CaptureScreenshot();
        }
    }

    void CaptureScreenshot()
    {
        // save file path
        string fileName = $"{screenshotPath}/screenshot_{System.DateTime.Now:yyyy-MM-dd-HH-mm-ss}.png";

        // capture
        ScreenCapture.CaptureScreenshot(fileName);
        Debug.Log("Screenshot captured: " + fileName);
    }
}
