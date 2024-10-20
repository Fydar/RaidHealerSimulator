using System;
using UnityEngine;

[Ability("Continuous Attack")]
[Serializable]
public class ContinuousAttackAbility : BasicBeamAbility
{
    [Header("Ability")]
    public int DamageAmountPerTick;

    public override bool IsValidTarget(Character target)
    {
        if (target == null)
        {
            return false;
        }

        return Owner.TeamId != target.TeamId;
    }

    protected override void OnBeamEffect()
    {
        CurrentTarget.Health.ReduceValue(Owner, DamageAmountPerTick);
    }
}
