using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float currentTime;
    private float repeatRate = 1;

    internal void StartTimer()
    {
        InvokeRepeating(nameof(AddTimer), 1, repeatRate);
    }

    private void AddTimer()
    {
        currentTime++;
        //int hrs = Mathf.FloorToInt(currentTime / 3600);
        int min = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}",min,seconds);

        //timerText.text = Mathf.Floor(currentTime / 60).ToString("00") + ":" + Mathf.FloorToInt(currentTime % 60).ToString("00");
    }

    internal void ResetTime()
    {
        currentTime = 0;
        timerText.text = "00:00";

        CancelInvoke();
        StartTimer();
    }

    internal void StopTimer()
    {
        CancelInvoke();
    }
}
