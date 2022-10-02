using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public interface IAngryBar
{
    event Action OnAngry;
    event Action OnAngryBarFull;
    void LessAngry();
}

public class AngryBar : MonoBehaviour, IAngryBar
{
    public event Action OnAngry;
    public event Action OnAngryBarFull;
    [SerializeField] private Slider slider;

    private Image sliderFillImage;
    private ParticleSystem particleSystem;
    private Animator animator;

    [SerializeField]
    private Color firstColor = Color.green;
    [SerializeField]
    private Color midColor = Color.yellow;
    [SerializeField]
    private Color lastColor = Color.red;

    void Awake()
    {
        sliderFillImage = slider.fillRect.GetComponent<Image>();
        particleSystem = slider.GetComponentInChildren<ParticleSystem>();

        animator = GetComponentInChildren<Animator>();

        sliderFillImage.color = firstColor;
        GameObject.FindObjectOfType<Timer>().OnTenSecondsPassed += MoreAngry;
    }

    private void Update()
    {
        if(slider.value >= 1)
        {
            OnAngryBarFull?.Invoke();
        }
    }

    public void LessAngry()
    {
        StartCoroutine(LerpSliderValue(slider.value, slider.value - 0.10f, 2f));
    }

    void MoreAngry()
    {
        if(OnAngry != null)
        {
            OnAngry();
        }

        // lerp slider value to +10% of current value in 2 seconds
        StartCoroutine(LerpSliderValue(slider.value, slider.value + 0.10f, 2f));
    }

    IEnumerator LerpSliderValue(float startValue, float endValue, float duration)
    {
        ManageColor();
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            slider.value = Mathf.Lerp(startValue, endValue, time / duration);
            yield return null;
        }
    }

    private void ManageColor()
    {
        var emission = particleSystem.emission;
        if (slider.value < 0.3f)
        {
            emission.rateOverTime = 3;
            ChangeSliderColor(firstColor);
        }
        else if (slider.value < 0.6f)
        {
            emission.rateOverTime = 6;
            ChangeSliderColor(midColor);
        }
        else
        {
            emission.rateOverTime = 10;
            ChangeSliderColor(lastColor);
        }
    }

    public void ChangeSliderColor(Color color, float duration = 2.0f)
    {
        StartCoroutine(LerpColorSlider(sliderFillImage.color, color, duration));
        var settings = particleSystem.main;
        StartCoroutine(StartParticleSystem(2.0f));
    }

    IEnumerator LerpColorSlider(Color startColor, Color endColor, float duration)
    {
        animator.SetBool("Play", true);
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            sliderFillImage.color = Color.Lerp(startColor, endColor, time / duration);
            yield return null;
        }
        animator.SetBool("Play", false);
    }


    IEnumerator StartParticleSystem(float duration)
    {
        particleSystem.Play();
        yield return new WaitForSecondsRealtime(duration);
        particleSystem.Stop();
    }
}


