using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using Tobo.Util.Editor;
#endif

namespace Tobo.Inventory
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Item Library")]
    public class ItemLibrary : ScriptableObject
    {
        [Header("Fill through Menu")]
        public Item[] items;

#if UNITY_EDITOR
        [MenuItem("Inventory/Collect Items")]
        static void FillItems()
        {
            LibraryUtil.FillLibrary<ItemLibrary, Item>(nameof(ItemLibrary.items));
        }
#endif
    }
}
