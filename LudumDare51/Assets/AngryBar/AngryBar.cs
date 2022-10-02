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

enum AngryBarState
{
    Normal,
    Angry,
    AngryBarFull
}

public class AngryBar : MonoBehaviour, IAngryBar
{
    public event Action OnAngry;
    public event Action OnAngryBarFull;
    [SerializeField] private Slider slider;

    [SerializeField] private Image imgBossFace1;
    [SerializeField] private Image imgBossFace2;
    [SerializeField] private Image imgBossFace3;
    [SerializeField] private Image imgBossFaceHappy;

    private Image sliderFillImage;

    [SerializeField]
    private ParticleSystem particleSystemNegative;

    [SerializeField]
    private ParticleSystem particleSystemPostive;

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

        animator = GetComponentInChildren<Animator>();

        imgBossFace1.enabled = false;
        imgBossFace2.enabled = false;
        imgBossFace3.enabled = false;
        imgBossFaceHappy.enabled = false;

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
        var emission = particleSystemNegative.emission;
        if (slider.value < 0.3f)
        {
            emission.rateOverTime = 3;
            ChangeSliderColor(firstColor, AngryBarState.Normal, isLessAngry);
        }
        else if (slider.value < 0.6f)
        {
            emission.rateOverTime = 6;
            ChangeSliderColor(midColor, AngryBarState.Angry, isLessAngry);
        }
        else
        {
            emission.rateOverTime = 10;
            ChangeSliderColor(lastColor, AngryBarState.AngryBarFull, isLessAngry);
        }
    }

    private void ChangeSliderColor(Color color, AngryBarState state, bool isLessAngry, float duration = 2.0f)
    {
        StartCoroutine(LerpColorSlider(sliderFillImage.color, color, duration, isLessAngry));
        var settings = particleSystemNegative.main;

        if (!isLessAngry)
        {
            StartCoroutine(StartAngryParticleSystem(duration, state));
        }    
        else
        {
            StartCoroutine(StartPositiveParticleSystem(duration));
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


    IEnumerator StartAngryParticleSystem(float duration, AngryBarState state)
    {
        EnableTheRightImage(state, true);
        particleSystemNegative.Play();
        yield return new WaitForSecondsRealtime(duration);
        particleSystemNegative.Stop();
          EnableTheRightImage(state, false);
    }

    
    IEnumerator StartPositiveParticleSystem(float duration)
    {
        imgBossFaceHappy.enabled = true;
        particleSystemPostive.Play();
        yield return new WaitForSecondsRealtime(duration);
        particleSystemPostive.Stop();
        imgBossFaceHappy.enabled = false;
    }

    void EnableTheRightImage(AngryBarState state, bool enable)
    {
        if(state == AngryBarState.Normal)
        {
            imgBossFace1.enabled = enable;
        }
        else if(state == AngryBarState.Angry)
        {
            imgBossFace2.enabled = enable;
        }
        else
        {
            imgBossFace3.enabled = enable;
        }
    }
}


