using UnityEngine;
using System.Collections;

namespace RPG.Database
{
    public class InputManager : ScriptableObject
    {
        public bool UFPS;
        public KeyCode reloadWeapon = KeyCode.R;

        public KeyCode SplitItem;
        public KeyCode InventoryKeyCode;
        public KeyCode StorageKeyCode;
        public KeyCode CharacterSystemKeyCode;
        public KeyCode CraftSystemKeyCode;
    }
}
