using UnityEngine;

public class PartyRenderer : MonoBehaviour
{
	public PartyGenerator Generator;

	public RectTransform Holder;
	public CharacterPortraitRendererPool PartyMemberPool;

	private void Start()
	{
		PartyMemberPool.Flush();

		foreach (var member in Generator.Party)
		{
			var clone = PartyMemberPool.Grab(Holder);
			clone.SetTarget(member);
		}
	}
}
