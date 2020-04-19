using System;
using UnityEngine;

[Ability("AoE Attack")]
[Serializable]
public class AoEAttack : BasicAbility
{
	[Header("Ability")]
	public int DamageAmount;

	public override bool IsValidTarget(Character target)
	{
		if (target == null)
		{
			return false;
		}

		return Owner.TeamId != target.TeamId;
	}

	protected override void OnWarmupComplete()
	{
		foreach (var ally in CurrentTarget.Party.Members)
		{
			ally.Health.ReduceValue(Owner, DamageAmount);
		}

		if (Owner.CurrentAbility == this)
		{
			Owner.CurrentAbility = null;
		}
	}
}
