using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;

public class FPScounter : MonoBehaviour
{
    private TextMeshProUGUI fpsText;
    private float pollingTime = 0.5f;
    private float time;
    private int frameCount;

    private void Awake()
    {
        fpsText = GetComponent<TextMeshProUGUI>();

        Application.targetFrameRate = 120;
        StartCoroutine("WaitForNextFrame");
    }

    void Update()
    {
        time += Time.unscaledDeltaTime;
        frameCount++;

        if (time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            fpsText.text = "FPS: " + frameRate;

            time -= pollingTime;
            frameCount = 0;
        }
    }
}
