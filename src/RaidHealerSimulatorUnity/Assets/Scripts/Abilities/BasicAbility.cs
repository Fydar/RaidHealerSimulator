using UnityEngine;

public abstract class BasicAbility : Ability
{
    public enum BasicState
    {
        Passive,
        WarmingUp,
        RunningDown
    }

    [Space]
    [Tooltip("After the spell has finished the spell will go on cooldown.")]
    public float Cooldown = 1.0f;

    [Tooltip("After casting, the user must channel for the warmup time before the spell takes effect.")]
    public float WarmupTime = 0.0f;

    private BasicState CurrentBasicState;
    private float CurrentCooldown = 0.0f;
    protected Character CurrentTarget;

    public override float CooldownPercentage => Mathf.Min(CurrentCooldown / Cooldown,
        Owner.GlobalCooldownPercent);

    public override void Tick()
    {
        if (CurrentBasicState == BasicState.WarmingUp)
        {
            StatusPercent += Time.fixedDeltaTime / WarmupTime;
            if (StatusPercent >= 1.0f)
            {
                CurrentBasicState = BasicState.Passive;
                StatusString = null;
                OnWarmupComplete();
            }
        }

        if (CanCooldown())
        {
            CurrentCooldown += Time.fixedDeltaTime;
        }
    }

    public virtual bool CanCooldown()
    {
        if (CurrentBasicState == BasicState.WarmingUp)
        {
            return false;
        }

        return true;
    }

    protected abstract void OnWarmupComplete();

    public override void Interupt()
    {
        if (CurrentBasicState == BasicState.WarmingUp)
        {
            CurrentBasicState = BasicState.Passive;
            StatusString = null;
            StatusPercent = 0.0f;
        }
    }

    public override void Cast(Character target)
    {
        CurrentTarget = target;
        CurrentCooldown = 0.0f;
        Owner.TriggerGlobalCooldown();

        StatusPercent = 0.0f;

        if (WarmupTime > 0.0f)
        {
            StatusString = "Channeling";
            CurrentBasicState = BasicState.WarmingUp;
        }
    }
}
