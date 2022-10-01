using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngryBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    private Image sliderFillImage;
    private ParticleSystem particleSystem;
    private Animator animator;

    private Color firstColor = Color.green;
    private Color midColor = Color.yellow;
    private Color lastColor = Color.red;

    void Awake()
    {
        sliderFillImage = slider.fillRect.GetComponent<Image>();
        particleSystem = slider.GetComponentInChildren<ParticleSystem>();
        
        animator = GetComponentInChildren<Animator>();

        var settings = particleSystem.main;
        settings.startColor = firstColor;
        sliderFillImage.color = firstColor;
        GameObject.FindObjectOfType<Timer>().OnTenSecondsPassed += MoreAngry;
    }

    void MoreAngry()
    {
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
        if (slider.value < 0.3f)
        {
            ChangeSliderColor(firstColor);
        }
        else if (slider.value < 0.6f)
        {
            ChangeSliderColor(midColor);
        }
        else
        {
            ChangeSliderColor(lastColor);
        }
    }

    public void ChangeSliderColor(Color color, float duration = 2.0f)
    {
        StartCoroutine(LerpColorSlider(sliderFillImage.color, color, duration));
        var settings = particleSystem.main;
        StartCoroutine(LerpColorParticleSystem(settings.startColor.color, color, duration));

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


    IEnumerator LerpColorParticleSystem(Color startColor, Color endColor, float duration)
    {
        float time = 0f;
        particleSystem.Play();
        while (time < duration)
        {
            time += Time.deltaTime;
            var settings = particleSystem.main;
            settings.startColor = Color.Lerp(startColor, endColor, time / duration);
            yield return null;
        }
        particleSystem.Stop();
    }
}

