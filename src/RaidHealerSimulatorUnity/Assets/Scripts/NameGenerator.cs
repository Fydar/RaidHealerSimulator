using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NameGenerator : ScriptableObject
{
	public string[] Names;

	public string GetRandomName()
	{
		return Names[Random.Range(0, Names.Length)];
	}
}
