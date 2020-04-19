using System;
using UnityEngine;

[Ability("AoE Heal")]
[Serializable]
public class AoEHealAbility : BasicAbility
{
	[Header("Ability")]
	public int HealAmount;
	public bool IsRevive;

	public override bool IsValidTarget(Character target)
	{
		if (target == null)
		{
			return true;
		}

		if (target.IsDead != IsRevive)
		{
			return true;
		}

		return Owner.TeamId == target.TeamId;
	}

	protected override void OnWarmupComplete()
	{
		var effectTarget = CurrentTarget;
		if (effectTarget == null)
		{
			effectTarget = Owner;
		}

		foreach (var ally in effectTarget.Party.Members)
		{
			if (ally.IsDead != IsRevive)
			{
				continue;
			}
			ally.Health.IncreaseValue(Owner, HealAmount);
		}

		if (Owner.CurrentAbility == this)
		{
			Owner.CurrentAbility = null;
		}
	}
}
