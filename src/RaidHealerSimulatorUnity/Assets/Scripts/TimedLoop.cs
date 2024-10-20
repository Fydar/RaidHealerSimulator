using System.Collections;
using System.Collections.Generic;

public sealed class TimedLoop : IEnumerator<float>, IEnumerable<float>
{
    private float duration;
    private bool endNext;

    public float Current => Percent;

    public float Duration
    {
        get => duration;
        set
        {
            duration = value;

            if (Time > duration)
            {
                Time = duration;
                endNext = true;
            }
        }
    }

    public float Time { get; set; }

    public float Percent
    {
        get => Time / duration;
        set => Time = duration * value;
    }

    private TimedLoop GetEnumerator()
    {
        return this;
    }

    IEnumerator<float> IEnumerable<float>.GetEnumerator()
    {
        return this;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this;
    }

    object IEnumerator.Current => Percent;

    public TimedLoop(float _duration)
    {
        Time = 0.0f;
        duration = _duration;
        endNext = false;
    }

    public void End()
    {
        Time = duration;
    }

    public void Break()
    {
        Time = duration;
        endNext = true;
    }

    public bool MoveNext()
    {
        Time += UnityEngine.Time.deltaTime;

        if (Time < duration)
        {
            return true;
        }

        if (endNext == false)
        {
            endNext = true;
            Time = duration;
            return true;
        }

        return false;
    }

    public void Reset()
    {
        Time = 0.0f;
        endNext = false;
    }

    public void Dispose()
    {
    }
}
