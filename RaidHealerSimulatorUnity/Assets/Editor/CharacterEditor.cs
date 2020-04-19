using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Character))]
public class CharacterEditor : Editor
{
	private GenericMenu addMenu;

	public override void OnInspectorGUI()
	{
		if (addMenu == null)
		{
			Reload();
		}
		var character = (Character)target;
		if (character.Abilities == null)
		{
			character.Abilities = new List<IAbility>();
		}

		DrawDefaultInspector();

		if (EditorGUILayout.DropdownButton(new GUIContent("Add Ability"), FocusType.Keyboard))
		{
			addMenu.ShowAsContext();
		}
	}

	private void Reload()
	{
		addMenu = new GenericMenu();

		var assembly = typeof(Character).Assembly;

		foreach (var type in assembly.GetTypes())
		{
			if (type.IsAbstract)
			{
				continue;
			}
			if (typeof(Ability).IsAssignableFrom(type))
			{
				object[] attributes = type.GetCustomAttributes(typeof(AbilityAttribute), false);

				string path;
				if (attributes.Length == 0)
				{
					path = type.Namespace;
				}
				else
				{
					var attribute = (AbilityAttribute)attributes[0];
					path = attribute.Path;
				}

				addMenu.AddItem(new GUIContent(path), false,
					AddItem, type);
			}
		}
	}

	private void AddItem(object typeObject)
	{
		var type = (Type)typeObject;
		var character = (Character)target;

		object newAbility = Activator.CreateInstance(type);

		character.Abilities.Add((Ability)newAbility);

		EditorUtility.SetDirty(character);
	}
}
