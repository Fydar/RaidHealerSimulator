using System;
using UnityEngine;

[Ability("Continuous Heal")]
[Serializable]
public class ContinuousHealAbility : BasicBeamAbility
{
    [Header("Ability")]
    public int HealAmountPerTick;

    public override bool IsValidTarget(Character target)
    {
        if (target == null)
        {
            return true;
        }

        return Owner.TeamId == target.TeamId;
    }

    protected override void OnBeamEffect()
    {
        var effectTarget = CurrentTarget;
        if (effectTarget == null)
        {
            effectTarget = Owner;
        }
        if (effectTarget.IsDead)
        {
            return;
        }
        effectTarget.Health.IncreaseValue(Owner, HealAmountPerTick);
    }
}
