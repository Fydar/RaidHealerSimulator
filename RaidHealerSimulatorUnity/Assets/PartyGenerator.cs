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
	[NonSerialized] public List<Character> Party;
	public NameGenerator Names;

	private void Awake()
	{
		Party = new List<Character>();
		Party.Add(Leader);
		Leader.Party = this;

		foreach (var classSelector in Classes)
		{
			for (int i = 0; i < classSelector.PerTeam; i++)
			{
				var clone = Instantiate(classSelector.Template, transform);
				Party.Add(clone);
				clone.Party = this;
				clone.DisplayName = Names.GetRandomName();
			}
		}
	}
}
