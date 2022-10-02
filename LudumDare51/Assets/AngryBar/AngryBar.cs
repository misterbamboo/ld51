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

    [SerializeField] private Image imgBossFace;

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

        imgBossFace.enabled = false;

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
        StartCoroutine(LerpSliderValue(slider.value, slider.value - 0.10f, 2f, true));
    }

    void MoreAngry()
    {
        if(OnAngry != null)
        {
            OnAngry();
        }

        // lerp slider value to +10% of current value in 2 seconds
        StartCoroutine(LerpSliderValue(slider.value, slider.value + 0.10f, 2f, false));
    }

    IEnumerator LerpSliderValue(float startValue, float endValue, float duration, bool isLessAngry)
    {
        ManageColor(isLessAngry);
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            slider.value = Mathf.Lerp(startValue, endValue, time / duration);
            yield return null;
        }
    }

    private void ManageColor(bool isLessAngry)
    {
        var emission = particleSystem.emission;
        if (slider.value < 0.3f)
        {
            emission.rateOverTime = 3;
            ChangeSliderColor(firstColor, isLessAngry);
        }
        else if (slider.value < 0.6f)
        {
            emission.rateOverTime = 6;
            ChangeSliderColor(midColor, isLessAngry);
        }
        else
        {
            emission.rateOverTime = 10;
            ChangeSliderColor(lastColor, isLessAngry);
        }
    }

    public void ChangeSliderColor(Color color, bool isLessAngry, float duration = 2.0f)
    {
        StartCoroutine(LerpColorSlider(sliderFillImage.color, color, duration, isLessAngry));
        var settings = particleSystem.main;

        print("isLessAngry: " + isLessAngry);
        if (!isLessAngry)
        {
            StartCoroutine(StartAngryParticleSystem(duration));
        }        
    }

    IEnumerator LerpColorSlider(Color startColor, Color endColor, float duration, bool isLessAngry)
    {
        if(!isLessAngry)
        {
            animator.SetBool("Play", true);
        }

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            sliderFillImage.color = Color.Lerp(startColor, endColor, time / duration);
            yield return null;
        }

        if(!isLessAngry)
        {
            animator.SetBool("Play", false);
        }

    }


    IEnumerator StartAngryParticleSystem(float duration)
    {
        imgBossFace.enabled = true;
        particleSystem.Play();
        yield return new WaitForSecondsRealtime(duration);
        particleSystem.Stop();
        imgBossFace.enabled = false;
    }
}


