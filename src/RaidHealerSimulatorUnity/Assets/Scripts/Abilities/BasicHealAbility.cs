using System;
using UnityEngine;

[Ability("Basic Heal")]
[Serializable]
public class BasicHealAbility : BasicAbility
{
    [Header("Ability")]
    public int HealAmount;
    public bool IsRevive;

    public override bool IsValidTarget(Character target)
    {
        if (target == null)
        {
            return !IsRevive;
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

        if (effectTarget.IsDead == IsRevive)
        {
            effectTarget.Health.IncreaseValue(Owner, HealAmount);
        }

        if (Owner.CurrentAbility == this)
        {
            Owner.CurrentAbility = null;
        }
    }
}
