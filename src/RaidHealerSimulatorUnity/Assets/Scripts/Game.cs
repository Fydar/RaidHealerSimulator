using System.Collections;
using System.Linq;
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

    public static bool IsGameOver = false;
    public static bool IsGameWon = false;
    public static bool IsGameLost = false;

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        IsGameOver = false;
        IsGameWon = false;
        IsGameLost = false;
        ChatFader.gameObject.SetActive(false);
        GameOverDialogue.gameObject.SetActive(false);
        InputDialogue.gameObject.SetActive(false);

        EnterNameChannelParty.SetActive(true);
        EnterNameChannelGlobal.SetActive(false);

        ChatFader.gameObject.SetActive(true);

        foreach (float time in new TimedLoop(1.0f))
        {
            ChatFader.alpha = time;
            yield return null;
        }

        Chat.Instance.Log($"<color=#ADB437>You have been added to a party.</color>");
        Chat.Instance.Log($"<color=#ADB437>Loot distribution has been set to round-robin.</color>");

        yield return new WaitForSeconds(1f);

        var partyLeader = PlayerParty.Members
            .Where(c => c.TryGetComponent<AiController>(out _))
            .First();

        Chat.Instance.Log($"<color=#D4AF37>{partyLeader.DisplayName}:</color> nice, we finally got a healer");
        yield return new WaitForSeconds(0.5f);
        Chat.Instance.Log($"<color=#D4AF37>{partyLeader.DisplayName}:</color> everyone rdy?");
        yield return new WaitForSeconds(0.8f);

        foreach (var member in PlayerParty.Members)
        {
            if (member == partyLeader)
            {
                continue;
            }
            if (member.TryGetComponent<AiController>(out var memberAi))
            {
                switch (Random.Range(0, 3))
                {
                    case 0:
                    case 1:
                        Chat.Instance.Log($"<color=#23459C>{memberAi.character.DisplayName}:</color> r");
                        break;
                    case 2:
                        Chat.Instance.Log($"<color=#23459C>{memberAi.character.DisplayName}:</color> ready");
                        break;
                }
                yield return new WaitForSeconds(Random.Range(0.2f, 0.7f));
            }
        }

        yield return new WaitForSeconds(0.8f);

        RepeatStartTime = Time.time - RepeatWait;
        while (true)
        {
            if (RepeatStartTime + RepeatWait < Time.time)
            {
                RepeatStartTime = Time.time;
                switch (Random.Range(0, 3))
                {
                    case 0:
                    case 1:
                        Chat.Instance.Log($"<color=#D4AF37>{partyLeader.DisplayName}:</color> healer, are you rdy?");
                        break;
                    case 2:
                        Chat.Instance.Log($"<color=#D4AF37>{partyLeader.DisplayName}:</color> send 'r' in the chat if yr ready");
                        break;
                }
            }
            yield return null;

            if (ChatInput.Instance.IsDirty)
            {
                ChatInput.Instance.IsDirty = false;
                break;
            }
        }

        yield return new WaitForSeconds(1.0f);

        Chat.Instance.Log($"<color=#23459C>{partyLeader.DisplayName}:</color> ok");
        yield return new WaitForSeconds(1.0f);
        switch (Random.Range(0, 3))
        {
            case 0:
                Chat.Instance.Log($"<color=#D4AF37>{partyLeader.DisplayName}:</color> wait for tank to start");
                break;
            case 1:
                Chat.Instance.Log($"<color=#D4AF37>{partyLeader.DisplayName}:</color> tank start it");
                break;
            case 2:
                Chat.Instance.Log($"<color=#D4AF37>{partyLeader.DisplayName}:</color> tank pull please");
                break;
        }

        yield return new WaitForSeconds(1.5f);

        foreach (var member in PlayerParty.Members)
        {
            foreach (var enemy in EnemyParty.Members)
            {
                if (member.TryGetComponent<AiController>(out var memberAi))
                {
                    var encounter = memberAi.GetOrCreateEncounter(enemy);

                    if (enemy == EnemyParty.Leader)
                    {
                        encounter.Threat += 10;
                    }
                }
            }
        }

        while (PlayerParty.IsAnyAlive && !EnemyParty.Leader.IsDead)
        {
            yield return null;
        }
        IsGameOver = true;

        IsGameWon = PlayerParty.IsAnyAlive;
        IsGameLost = !IsGameWon;

        yield return new WaitForSeconds(1.5f);

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
