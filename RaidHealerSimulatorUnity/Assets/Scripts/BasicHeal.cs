using System;
using UnityEngine;

[Ability("Basic Heal")]
[Serializable]
public class BasicHeal : BasicAbility
{
	[Header("Ability")]
	public int HealAmount;

	public override bool IsValidTarget(Character target)
	{
		return true;
	}

	protected override void OnWarmupComplete()
	{
		var effectTarget = CurrentTarget;
		if (effectTarget == null)
		{
			effectTarget = Owner;
		}

		effectTarget.Health.IncreaseValue(HealAmount);


		if (Owner.CurrentAbility == this)
		{
			Owner.CurrentAbility = null;
		}
	}
}
