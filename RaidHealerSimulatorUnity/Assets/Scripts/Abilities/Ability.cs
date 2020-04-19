using UnityEngine;

public abstract class Ability : IAbility
{
	public string Name;
	public Sprite Icon;
	public int Level;

	public string StatusString { get; protected set; }
	public float StatusPercent { get; protected set; }
	public Character Owner { get; internal set; }

	public bool Unlocked => Level > 0;

	public bool Ready => Unlocked && CooldownPercentage >= 1.0f;

	public abstract float CooldownPercentage { get; }

	public abstract void Tick();

	public abstract void Cast(Character target);

	public abstract bool IsValidTarget(Character target);

	public abstract void Interupt();
}
