using System.Collections.Generic;
using UnityEngine;

public class Hotbar : MonoBehaviour
{
    private static readonly Dictionary<int, KeyCode> mappings = new()
    {
        [0] = KeyCode.Alpha1,
        [1] = KeyCode.Alpha2,
        [2] = KeyCode.Alpha3,
        [3] = KeyCode.Alpha4,
        [4] = KeyCode.Alpha5,
        [5] = KeyCode.Alpha6,
        [6] = KeyCode.Alpha7,
        [7] = KeyCode.Alpha8,
        [8] = KeyCode.Alpha9,
        [9] = KeyCode.Minus,
        [10] = KeyCode.Equals,
    };

    [SerializeField] private Character character;

    [Space]
    [SerializeField] private RectTransform holder;
    [SerializeField] private UIPool<HotbarSlot> slotPool;

    private void Start()
    {
        slotPool.Flush();
        int index = 0;
        foreach (var ability in character.Abilities)
        {
            var slotRenderer = slotPool.Grab(holder);

            if (!mappings.TryGetValue(index, out var mapping))
            {
                mapping = KeyCode.None;
            }

            slotRenderer.Setup((Ability)ability, mapping);
            index++;
        }
    }
}
