using System;

public class AbilityAttribute : Attribute
{
	public string Path { get; }

	public AbilityAttribute(string path)
	{
		Path = path;
	}
}
