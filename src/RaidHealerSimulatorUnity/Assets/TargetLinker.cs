using UnityEngine;

public class TargetLinker : MonoBehaviour
{
    private CharacterPortraitRenderer thisPortrait;
    public CharacterPortraitRenderer OtherPortrait;

    private void Start()
    {
        thisPortrait = GetComponent<CharacterPortraitRenderer>();
    }

    private void Update()
    {
        if (thisPortrait.Target?.SelectedTarget == null)
        {
            OtherPortrait.gameObject.SetActive(false);
            return;
        }

        if (OtherPortrait.Target != thisPortrait.Target.SelectedTarget)
        {
            OtherPortrait.SetTarget(thisPortrait.Target.SelectedTarget);
        }
    }
}
