using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
	public static Game Instance;

	[Header("General")]
	public CanvasGroup FadeToBlack;
	public Character PlayerCharacter;

	[Header("Name Input")]
	public CanvasGroup EnterNameDialogue;
	public Button EnterNameButton;
	public InputField EnterNameField;

	[Space]
	public GameObject EnterNameChannelParty;
	public GameObject EnterNameChannelGlobal;

	[Header("Chat")]
	public CanvasGroup ChatFader;
	public string Username = null;
	public float RepeatStartTime = 0.0f;
	public float RepeatWait = 16.0f;

	[Header("Invite Dialogue")]
	public CanvasGroup InputDialogue;
	public bool AcceptedInvite;

	[Header("Game World")]
	public PartyGenerator PlayerParty;
	public PartyGenerator EnemyParty;

	[Header("Game Over")]
	public CanvasGroup GameOverDialogue;
	private bool Restart = false;

	private void Awake()
	{
		Instance = this;
	}

	private IEnumerator Start()
	{
		ChatFader.gameObject.SetActive(false);
		GameOverDialogue.gameObject.SetActive(false);
		InputDialogue.gameObject.SetActive(false);

		EnterNameChannelParty.SetActive(false);
		EnterNameChannelGlobal.SetActive(true);

		FadeToBlack.gameObject.SetActive(true);
		EnterNameDialogue.gameObject.SetActive(true);
		FadeToBlack.alpha = 1.0f;
		EnterNameButton.interactable = false;

		foreach (float time in new TimedLoop(1.0f))
		{
			EnterNameDialogue.alpha = time;
			yield return null;
		}

		while (!IsValidName(Username))
		{
			if (IsValidName(EnterNameField.text))
			{
				EnterNameButton.interactable = true;
			}
			else
			{
				EnterNameButton.interactable = false;
			}
			yield return null;
		}

		PlayerCharacter.DisplayName = Username;

		foreach (float time in new TimedLoop(1.0f))
		{
			EnterNameDialogue.alpha = 1.0f - time;
			yield return null;
		}

		ChatFader.gameObject.SetActive(true);

		foreach (float time in new TimedLoop(1.0f))
		{
			ChatFader.alpha = time;
			yield return null;
		}


		RepeatStartTime = Time.time - RepeatWait;
		while (true)
		{
			if (RepeatStartTime + RepeatWait < Time.time)
			{
				RepeatStartTime = Time.time;
				Chat.Instance.Log($"<color=#D4AF37>xxKillerZZz:</color> LF 1 Healer for world boss, level 55 minimum");
			}
			yield return null;

			if (ChatInput.Instance.IsDirty)
			{
				ChatInput.Instance.IsDirty = false;
				break;
			}
		}

		yield return new WaitForSeconds(1.0f);

		InputDialogue.gameObject.SetActive(true);
		foreach (float time in new TimedLoop(0.2f))
		{
			InputDialogue.alpha = time;
			yield return null;
		}

		while (!AcceptedInvite)
		{
			yield return null;
		}

		InputDialogue.gameObject.SetActive(true);
		foreach (float time in new TimedLoop(0.2f))
		{
			InputDialogue.alpha = 1.0f - time;
			yield return null;
		}

		EnterNameChannelParty.SetActive(true);
		EnterNameChannelGlobal.SetActive(false);

		Chat.Instance.Log($"<color=#D4AF37>You have been added to a party.</color>");
		Chat.Instance.Log($"<color=#D4AF37>Loot distribution has been set to round-robin.</color>");

		FadeToBlack.alpha = 0.0f;
		FadeToBlack.gameObject.SetActive(false);

		yield return new WaitForSeconds(2.5f);

		Chat.Instance.Log($"<color=#23459C>xxKillerZZz:</color> Ayyy we finally have a header, let's go");

		yield return new WaitForSeconds(1.5f);

		Chat.Instance.Log($"<color=#23459C>xxKillerZZz:</color> Tank start it");

		yield return new WaitForSeconds(1.5f);

		foreach (var member in PlayerParty.Members)
		{
			foreach (var enemy in EnemyParty.Members)
			{
				var memberAi = member.GetComponent<AiController>();
				if (memberAi != null)
				{
					var encounter = memberAi.GetOrCreateEncounter(enemy);

					if (enemy == EnemyParty.Leader)
					{
						encounter.Threat += 10;
					}
				}
			}
		}

		while (PlayerParty.IsAnyAlive || EnemyParty.Leader.IsDead)
		{
			yield return null;
		}

		yield return new WaitForSeconds(1.5f);

		// TODO: Insert code to flame the player

		FadeToBlack.gameObject.SetActive(true);
		foreach (float time in new TimedLoop(1.0f))
		{
			FadeToBlack.alpha = time;
			yield return null;
		}

		GameOverDialogue.gameObject.SetActive(true);
		foreach (float time in new TimedLoop(1.0f))
		{
			GameOverDialogue.alpha = time;
			yield return null;
		}

		while (!Restart)
		{
			yield return null;
		}

		SceneManager.LoadScene(0, LoadSceneMode.Single);
	}

	public void UiEnterName()
	{
		if (!IsValidName(EnterNameField.text))
		{
			return;
		}
		Username = EnterNameField.text.Trim().Replace(" ", "_");
	}

	public void UiAcceptedInvite()
	{
		AcceptedInvite = true;
	}

	public void UiRestartGame()
	{
		Restart = true;
	}

	private bool IsValidName(string name)
	{
		return !string.IsNullOrWhiteSpace(name)
			&& name.Length < 32
			&& name.Length > 3;
	}
}
