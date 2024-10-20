using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private Text TextField;
    [SerializeField] private string TextFormat = "{0:#}/{1:#}";

    [Header("Primary Bar")]
    [SerializeField] private Image primaryBar;

    [Header("Secondary Bar")]
    [SerializeField] private Image secondaryBar;
    [SerializeField] private float secondaryBarDelay = 1.0f;

    private CharacterResource resource;

    private void Update()
    {
        if (secondaryBar != null)
        {
            if (secondaryBar.fillAmount > primaryBar.fillAmount)
            {
                secondaryBar.fillAmount = Mathf.Lerp(secondaryBar.fillAmount, primaryBar.fillAmount, Time.unscaledDeltaTime * secondaryBarDelay);
            }
            else
            {
                secondaryBar.fillAmount = primaryBar.fillAmount;
            }
        }
    }

    public void SetTarget(CharacterResource resource)
    {
        this.resource = resource;

        resource.OnValueChanged += UpdateBars;

        UpdateBars(null, 0);
    }

    private void UpdateBars(Character character, int damage)
    {
        float value = resource.CurrentValue;
        float max = resource.MaxValue;

        primaryBar.fillAmount = value / max;

        if (TextField != null)
        {
            TextField.text = string.Format(TextFormat, value, max);
        }
    }
}
