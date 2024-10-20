using System;
using System.Collections.Generic;
using UnityEngine;

public class PartyGenerator : MonoBehaviour
{
    [Serializable]
    public struct ClassSelector
    {
        public Character Template;
        public int PerTeam;
    }

    public ClassSelector[] Classes;

    public Character Leader;
    [NonSerialized] public List<Character> Members;
    public NameGenerator Names;

    public bool IsAnyAlive
    {
        get
        {
            foreach (var member in Members)
            {
                if (!member.IsDead)
                {
                    return true;
                }
            }
            return false;
        }
    }

    private void Awake()
    {
        Members = new List<Character>();

        foreach (var classSelector in Classes)
        {
            for (int i = 0; i < classSelector.PerTeam; i++)
            {
                var clone = Instantiate(classSelector.Template, transform);
                Members.Add(clone);
                clone.Party = this;
                clone.DisplayName = Names.GetRandomName();
            }
        }

        Members.Add(Leader);
        Leader.Party = this;
    }
}
