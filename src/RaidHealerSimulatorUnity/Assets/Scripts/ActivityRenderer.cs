using UnityEngine;
using UnityEngine.UI;

public class ActivityRenderer : MonoBehaviour
{
    public Image AbilityIcon;
    public Image ProgressBar;
    public Text Status;

    [Header("Fade")]
    public CanvasGroup Fader;
    public float FadeSpeed = 16.0f;

    [Space]
    public Character Target;

    private void Start()
    {
        if (Target != null)
        {
            var target = Target;
            Target = null;
            SetTarget(target);
        }
    }

    public void SetTarget(Character target)
    {
        Target = target;
    }

    private void Update()
    {
        if (Target == null)
        {
            return;
        }

        float targetAlpha;
        if (Target.CurrentAbility == null)
        {
            targetAlpha = 0.0f;
        }
        else if (string.IsNullOrEmpty(Target.CurrentAbility.StatusString))
        {
            targetAlpha = 0.0f;
        }
        else
        {
            targetAlpha = 1.0f;
            Status.text = Target.CurrentAbility.StatusString;
            ProgressBar.fillAmount = Target.CurrentAbility.StatusPercent;
            AbilityIcon.sprite = Target.CurrentAbility.Icon;
        }

        Fader.alpha = Mathf.Lerp(Fader.alpha, targetAlpha, Time.deltaTime * FadeSpeed);
    }
}
