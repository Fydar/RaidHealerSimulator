using UnityEngine;

public abstract class BasicBeamAbility : BasicAbility
{
    public enum BeamState
    {
        Passive,
        Beaming
    }

    [Header("Beam")]
    public int BeamTicks = 8;
    public float TimePerTick = 0.2f;

    private BeamState CurrentBeamState;
    private int CompletedTicks;
    private float TickProgress;
    private float CompletedTime;

    public override void Tick()
    {
        if (CurrentBeamState == BeamState.Beaming)
        {
            float maxTime = TimePerTick * BeamTicks;
            StatusPercent = 1.0f - (CompletedTime / maxTime);

            TickProgress += Time.deltaTime;
            CompletedTime += Time.deltaTime;

            while (TickProgress > TimePerTick)
            {
                CompletedTicks++;
                TickProgress -= TimePerTick;
                OnBeamEffect();

                if (CompletedTicks >= BeamTicks)
                {
                    CurrentBeamState = BeamState.Passive;
                    StatusString = null;
                    break;
                }
            }
        }
        else
        {
            base.Tick();
        }
    }

    protected abstract void OnBeamEffect();

    public override void Interupt()
    {
        base.Interupt();
        if (CurrentBeamState == BeamState.Beaming)
        {
            CurrentBeamState = BeamState.Passive;
            StatusString = null;
            StatusPercent = 0.0f;
        }
    }

    protected override void OnWarmupComplete()
    {
        CurrentBeamState = BeamState.Beaming;
        CompletedTicks = 0;
        TickProgress = 0.0f;
        CompletedTime = 0.0f;
        StatusString = "Channeling";
    }

    public override bool CanCooldown()
    {
        return CurrentBeamState == BeamState.Passive
            && base.CanCooldown();
    }
}
