using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tobo.Inventory
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Item Library")]
    public class ItemLibrary : ScriptableObject
    {
        [Header("Fill through Menu")]
        public Item[] items;
    }
}
