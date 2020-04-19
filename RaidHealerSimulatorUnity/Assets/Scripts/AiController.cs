using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class AiController : MonoBehaviour
{
	[Serializable]
	public class Encounter
	{
		public float Threat;
		public Character Other;
	}

	private Character character;

	public List<Encounter> Encounters;

	public NameGenerator LowHealthFlame;
	public NameGenerator RevivePleaseFlame;
	public NameGenerator WonSpeach;
	public NameGenerator LostSpeach;

	private void Awake()
	{
		character = GetComponent<Character>();

		character.Health.OnValueChanged += OnHealthChanged;
	}

	private void Start()
	{
		if (LowHealthFlame != null)
		{
			StartCoroutine(HealMeFlamingSubroutine());
		}
		if (RevivePleaseFlame != null)
		{
			StartCoroutine(ReviveFlamingSubroutine());
		}
	}

	private IEnumerator ReviveFlamingSubroutine()
	{
		while (!Game.IsGameOver)
		{
			if (character.IsDead)
			{
				Chat.Instance.Log($"<color=#23459C>{character.DisplayName}:</color> {RevivePleaseFlame.GetRandomName()}");
			}

			yield return new WaitForSeconds(UnityEngine.Random.Range(8, 22));
		}
	}

	private IEnumerator HealMeFlamingSubroutine()
	{
		while (!Game.IsGameOver)
		{
			float percent = character.Health.CurrentValue / (float)character.Health.MaxValue;
			if (percent < 0.3f)
			{
				Chat.Instance.Log($"<color=#23459C>{character.DisplayName}:</color> {LowHealthFlame.GetRandomName()}");
			}

			yield return new WaitForSeconds(UnityEngine.Random.Range(4, 16));
		}
		if (WonSpeach == null)
		{
			yield break;
		}
		for (int i = 0; i < UnityEngine.Random.Range(1, 3); i++)
		{
			if (Game.IsGameWon)
			{
				Chat.Instance.Log($"<color=#23459C>{character.DisplayName}:</color> {WonSpeach.GetRandomName()}");
			}
			else
			{
				Chat.Instance.Log($"<color=#23459C>{character.DisplayName}:</color> {LostSpeach.GetRandomName()}");
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(4, 8));
		}
		yield return new WaitForSeconds(UnityEngine.Random.Range(4, 12));

		Chat.Instance.Log($"<color=#23459C>{character.DisplayName} has left the party:</color>");
	}

	private void FixedUpdate()
	{
		if (character.IsDead)
		{
			return;
		}

		var target = GetCurrentTarget();
		character.SelectedTarget = target?.Other;

		if (target == null)
		{
			return;
		}

		if (character.CurrentAbility == null)
		{
			PlayAnAbility(target.Other);
		}
		else if (string.IsNullOrEmpty(character.CurrentAbility.StatusString))
		{
			PlayAnAbility(target.Other);
		}
	}

	private void PlayAnAbility(Character target)
	{
		for (int i = 0; i < 50; i++)
		{
			var randomAbility = (Ability)character.Abilities[UnityEngine.Random.Range(0, character.Abilities.Count)];

			if (randomAbility.Ready
				&& randomAbility.IsValidTarget(target))
			{
				if (randomAbility.Owner.CurrentAbility != null)
				{
					randomAbility.Owner.CurrentAbility.Interupt();
				}
				randomAbility.Owner.CurrentAbility = randomAbility;
				randomAbility.Cast(target);
				return;
			}
		}
	}

	private void OnHealthChanged(Character dealer, int damage)
	{
		if (dealer.TeamId != character.TeamId)
		{
			var encounter = GetOrCreateEncounter(dealer);
			encounter.Threat -= damage * dealer.ThreatMultiplier;
		}
	}

	public Encounter GetEncounter(Character other)
	{
		foreach (var encounter in Encounters)
		{
			if (encounter.Other == other)
			{
				return encounter;
			}
		}
		return null;
	}

	public Encounter GetCurrentTarget()
	{
		float maxThreat = float.MinValue;
		Encounter maxEncounter = null;

		foreach (var encounter in Encounters)
		{
			if (encounter.Threat > maxThreat)
			{
				maxThreat = encounter.Threat;
				maxEncounter = encounter;
			}
		}
		return maxEncounter;
	}

	public Encounter GetOrCreateEncounter(Character other)
	{
		foreach (var encounter in Encounters)
		{
			if (encounter.Other == other)
			{
				return encounter;
			}
		}
		var newencounter = new Encounter()
		{
			Other = other,
			Threat = 0
		};
		Encounters.Add(newencounter);
		return newencounter;
	}
}
