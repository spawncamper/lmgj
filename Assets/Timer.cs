using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // On the Timer canvas.  Here we call the GameOverTimeRanOut event when Timer runs out
    [SerializeField] float currentTimeRemaining = 10;
    [SerializeField] float sliderDangerZone = 0.2f;
    [SerializeField] Image sliderFill;
    private bool timerRanOut = false;
    [SerializeField] private Text timerText;
    [SerializeField] private Slider slider;
    [SerializeField] private Animator animator;
    float initialTimeRemaining;

    public delegate void TimerTunOut();
    public static event TimerTunOut TimeRunOutEvent;

    private void Start()
    {
        animator.enabled = false;

        initialTimeRemaining = currentTimeRemaining;

        DisplayTime(initialTimeRemaining);
        slider.maxValue = initialTimeRemaining;
        slider.value = slider.maxValue;
    }

    public void UpdateTimer()
    {
        UpdateTimeRemaining();
        UpdateAnimator();
    }

    private void UpdateTimeRemaining()
    {
        if (currentTimeRemaining > Time.deltaTime)
        {
            currentTimeRemaining -= Time.deltaTime;
            DisplayTime(currentTimeRemaining);
        }
        else if (currentTimeRemaining <= Time.deltaTime && timerRanOut == false)
        {
            timerRanOut = true;

            if (TimeRunOutEvent != null)
                TimeRunOutEvent();
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 0.5f - Time.deltaTime;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void UpdateAnimator()
    {
        if (slider.value <= slider.maxValue * sliderDangerZone)
        {
            animator.enabled = true;
        }
    }

    public void ResetTimer()
    {
        currentTimeRemaining = initialTimeRemaining;
        slider.maxValue = initialTimeRemaining;
        slider.value = slider.maxValue;
        sliderFill.color = new Color32(0, 255, 34, 255);
        animator.enabled = false;
        timerRanOut = false;
    }
}