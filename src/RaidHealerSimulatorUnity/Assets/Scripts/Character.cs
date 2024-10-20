using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static Character PlayerSelected;
    public Character SelectedTarget;

    public PartyGenerator Party;
    public string DisplayName;


    [SerializeField] public int TeamId;
    [SerializeField] public float ThreatMultiplier;

    [SerializeField] public Sprite ClassIcon;
    [SerializeField] public Color ClassIconColour;


    [SerializeField] public CharacterResource Health;
    [SerializeField] public CharacterResource Mana;

    [Space]
    [SerializeReference] public List<IAbility> Abilities;

    [Header("Global Cooldown")]
    [SerializeField] public float GlobalCooldownTime;
    [SerializeField] public float GlobalCooldownPercent;
    public bool IsOnGlobalCooldown => GlobalCooldownPercent <= 1.0f;

    public bool IsDead => Health.CurrentValue == 0;

    public Ability CurrentAbility;

    private void Start()
    {
        Health.Owner = this;
        Mana.Owner = this;

        foreach (var abilityBase in Abilities)
        {
            var ability = (Ability)abilityBase;

            ability.Owner = this;
        }

        Health.OnValueChanged += OnHealthChanged;
    }

    private void OnHealthChanged(Character dealer, int amount)
    {
        if (IsDead && amount < 0)
        {
            CurrentAbility?.Interupt();
            Chat.Instance.Log($"<color=#b53f3f>{DisplayName} have been slaim by {dealer.DisplayName}!</color>");
        }
    }

    private void FixedUpdate()
    {
        Health.Tick();
        Mana.Tick();

        foreach (var abilityBase in Abilities)
        {
            var ability = (Ability)abilityBase;
            ability.Tick();
        }

        GlobalCooldownPercent += Time.fixedDeltaTime / GlobalCooldownTime;
    }

    public void TriggerGlobalCooldown()
    {
        GlobalCooldownPercent = 0.0f;
    }
}
