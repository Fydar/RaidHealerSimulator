using UnityEngine;

[RequireComponent(typeof(Character))]
public class PlayerController : MonoBehaviour
{
	private Character character;

	private void Start()
	{
		character = GetComponent<Character>();
	}

	private void Update()
	{
		character.SelectedTarget = Character.PlayerSelected;

		bool scrollRight = false;
		bool scrollLeft = false;
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				scrollLeft = true;
			}
			else
			{
				scrollRight = true;
			}
		}

		if (scrollRight)
		{
			int currentIndex = character.Party.Party.IndexOf(character.SelectedTarget);
			if (currentIndex != -1)
			{
				currentIndex++;

				if (currentIndex >= character.Party.Party.Count)
				{
					currentIndex = 0;
				}
				character.SelectedTarget = character.Party.Party[currentIndex];
			}
		}

		if (scrollLeft)
		{
			int currentIndex = character.Party.Party.IndexOf(character.SelectedTarget);
			if (currentIndex != -1)
			{
				currentIndex--;

				if (currentIndex < 0)
				{
					currentIndex = character.Party.Party.Count - 1;
				}
				character.SelectedTarget = character.Party.Party[currentIndex];
			}
		}
	}
}
