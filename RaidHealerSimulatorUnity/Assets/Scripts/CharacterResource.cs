using System;
using UnityEngine;

[Serializable]
public class CharacterResource
{
	[SerializeField] private int currentValue = 100;

	[SerializeField] private int maxValue = 100;

	[Header("Health Regeneration")]
	[SerializeField] private int partialRegenPerTick = 1;
	[SerializeField] private int partialRegensPerRegen = 1;
	[SerializeField] private int resourcesPerRegen = 1;
	[SerializeField] private int regenDelay = 20;

	private int currentRegens = 0;
	private int currentDelay = 0;

	public int CurrentValue => currentValue;
	public int MaxValue => maxValue;

	public event Action OnValueChanged;

	public void Tick()
	{
		if (currentDelay > 0)
		{
			currentDelay--;
		}
		else if (currentValue != maxValue)
		{
			currentRegens += partialRegenPerTick;

			if (currentRegens >= partialRegensPerRegen)
			{
				currentRegens = 0;
				currentValue += resourcesPerRegen;
				if (currentValue > maxValue)
				{
					currentValue = maxValue;
				}
				OnValueChanged?.Invoke();
			}
		}
	}

	public void ReduceValue(int amount)
	{
		currentValue -= amount;
		if (currentValue <= 0)
		{
			currentValue = 0;
		}
		currentDelay = regenDelay;
		OnValueChanged?.Invoke();
	}

	public void IncreaseValue(int amount)
	{
		if (currentValue == maxValue)
		{
			return;
		}
		currentValue += amount;
		if (currentValue > maxValue)
		{
			currentValue = maxValue;
		}
		OnValueChanged?.Invoke();
	}

	public bool CanConsume(int amount)
	{
		return currentValue > amount;
	}
}
