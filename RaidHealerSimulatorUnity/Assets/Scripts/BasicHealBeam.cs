using System;
using UnityEngine;

[Ability("Basic Heal Beam")]
[Serializable]
public class BasicHealBeam : BasicBeamAbility
{
	[Header("Ability")]
	public int HealAmountPerTick;

	public override bool IsValidTarget(Character target)
	{
		return true;
	}

	protected override void OnBeamEffect()
	{
		var effectTarget = CurrentTarget;
		if (effectTarget == null)
		{
			effectTarget = Owner;
		}

		effectTarget.Health.IncreaseValue(HealAmountPerTick);
	}
}
