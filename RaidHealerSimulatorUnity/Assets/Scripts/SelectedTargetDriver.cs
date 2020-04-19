using UnityEngine;

[RequireComponent(typeof(CharacterPortraitRenderer))]
public class SelectedTargetDriver : MonoBehaviour
{
	[SerializeField] private CanvasGroup fader;
	[SerializeField] private float fadeSpeed = 12.0f;

	private CharacterPortraitRenderer characterPortraitRenderer;

	private void Awake()
	{
		characterPortraitRenderer = GetComponent<CharacterPortraitRenderer>();
		fader.alpha = 0.0f;
	}

	private void Update()
	{
		float targetAlpha = Character.PlayerSelected != null
			? 1.0f
			: 0.0f;

		if (characterPortraitRenderer.Target != Character.PlayerSelected
			&& Character.PlayerSelected != null)
		{
			characterPortraitRenderer.SetTarget(Character.PlayerSelected);
		}

		fader.alpha = Mathf.Lerp(fader.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
	}
}
