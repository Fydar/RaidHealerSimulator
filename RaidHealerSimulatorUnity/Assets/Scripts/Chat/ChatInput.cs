using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatInput : MonoBehaviour
{
	public static ChatInput Instance;

	public InputField Field;
	public Button Submit;
	private bool allowEnter;
	private int lookback = -1;
	public bool IsDirty = false;

	private readonly List<string> lastInputs = new List<string>();

	private void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
		bool send = (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter));
		if (allowEnter && Field.text.Length > 0 && send)
		{
			Send();
			lookback = -1;
			allowEnter = false;
		}
		else if (send || (!Field.isFocused && Input.GetKey(KeyCode.T)))
		{
			Field.Select();
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				Field.text = "/";
				Field.selectionAnchorPosition = 1;
				Field.selectionFocusPosition = 1;
			}
		}
		else
		{
			allowEnter = Field.isFocused || Field.isFocused;
		}

		if (Field.isFocused)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				if (lookback + 1 < lastInputs.Count)
				{
					lookback++;
					Field.text = lastInputs[lastInputs.Count - 1 - lookback];
					Field.selectionAnchorPosition = Field.text.Length;
					Field.selectionFocusPosition = Field.text.Length;
				}
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				if (lookback - 1 >= 0)
				{
					lookback--;
					Field.text = lastInputs[lastInputs.Count - 1 - lookback];
					Field.selectionAnchorPosition = Field.text.Length;
					Field.selectionFocusPosition = Field.text.Length;
				}
				else
				{
					lookback = -1;
					Field.text = "";
				}
			}
			else if (Input.anyKeyDown)
			{
				lookback = -1;
			}
		}
	}

	public void Send()
	{
		string text = Field.text;
		if (string.IsNullOrEmpty(text))
		{
			return;
		}

		Chat.Instance.Log($"<color=#777>{Game.Instance.Username}:</color> {text}");

		IsDirty = true;

		lastInputs.Add(text);
		if (lastInputs.Count > 30)
		{
			lastInputs.RemoveAt(lastInputs.Count - 1);
		}

		Field.text = "";
	}
}
