using System;
using UnityEngine;

[Ability("Basic Attack")]
[Serializable]
public class BasicAttack : BasicAbility
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
        CurrentTarget.Health.ReduceValue(Owner, DamageAmount);

        if (Owner.CurrentAbility == this)
        {
            Owner.CurrentAbility = null;
        }
    }
}
