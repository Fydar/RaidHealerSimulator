using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable] public class HotbarSlotPool : UIPool<HotbarSlot> { }

public class HotbarSlot : MonoBehaviour
{
	public Image Faded;
	public Image Main;

	[Space]
	public Color MainReady = new Color(1.0f, 1.0f, 1.0f);
	public Color MainNotReady = new Color(0.5f, 0.5f, 0.5f);
	public float MainColourFade = 1.0f;

	private Ability ability;
	private KeyCode binding;

	private void Update()
	{
		float percent = ability.CooldownPercentage;
		Main.fillAmount = percent;

		var mainTargetColor = ability.Ready ? MainReady : MainNotReady;
		Main.color = Color.Lerp(Main.color, mainTargetColor, Time.deltaTime * MainColourFade);

		if (binding != KeyCode.None && Input.GetKeyDown(binding))
		{
			UiClick();
		}
	}

	public void Setup(Ability ability, KeyCode binding)
	{
		this.ability = ability;
		this.binding = binding;

		Faded.sprite = ability.Icon;
		Main.sprite = ability.Icon;
	}

	public void UiClick()
	{
		if (ability.Ready)
		{
			if (ability.Owner.CurrentAbility != null)
			{
				ability.Owner.CurrentAbility.Interupt();
			}
			ability.Owner.CurrentAbility = ability;
			ability.Cast(ability.Owner.SelectedTarget);
		}
	}
}
