using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable] public class CharacterPortraitRendererPool : UIPool<CharacterPortraitRenderer> { }

public class CharacterPortraitRenderer : MonoBehaviour
{
	public Image ClassIcon;
	public Text Name;

	[Space]
	public ResourceBar Health;
	public ResourceBar Mana;
	public ActivityRenderer ActivityRenderer;

	[Space]
	public Character Target;

	[Header("Scaling")]
	public RectTransform ScaleTarget;
	public float ScaleLerp = 12.0f;
	public float UnselectedScale = 1.0f;
	public float SelectedScale = 1.0f;

	[Space]
	public Image Selection;
	public float SelectionFadeLerp;

	private void Start()
	{
		if (Target != null)
		{
			SetTarget(Target);
		}
	}

	private void Update()
	{
		if (ScaleTarget != null)
		{
			float target = Character.PlayerSelected == Target
				? SelectedScale
				: UnselectedScale;
			ScaleTarget.localScale = Vector3.Lerp(ScaleTarget.localScale, Vector3.one * target, Time.deltaTime * ScaleLerp);
		}
		if (Selection != null)
		{
			var cur = Selection.color;
			var target = Character.PlayerSelected == Target
				? new Color(cur.r, cur.g, cur.b, 1.0f)
				: new Color(cur.r, cur.g, cur.b, 0.0f);
			Selection.color = Color.Lerp(Selection.color, target, Time.deltaTime * SelectionFadeLerp);
		}
	}

	public void SetTarget(Character target)
	{
		Target = target;

		Health.SetTarget(Target.Health);
		Mana.SetTarget(Target.Mana);

		ClassIcon.sprite = Target.ClassIcon;
		ClassIcon.color = Target.ClassIconColour;

		if (Name != null)
		{
			Name.text = target.DisplayName;
		}

		if (ActivityRenderer != null)
		{
			ActivityRenderer.SetTarget(target);
		}
	}

	public void UiSelect()
	{
		Character.PlayerSelected = Target;
	}
}
