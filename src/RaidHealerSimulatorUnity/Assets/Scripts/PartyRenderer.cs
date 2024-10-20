﻿using UnityEngine;

public class PartyRenderer : MonoBehaviour
{
    public PartyGenerator Generator;

    public RectTransform Holder;
    public UIPool<CharacterPortraitRenderer> PartyMemberPool;

    private void Start()
    {
        PartyMemberPool.Flush();

        foreach (var member in Generator.Members)
        {
            var clone = PartyMemberPool.Grab(Holder);
            clone.SetTarget(member);
        }
    }
}
