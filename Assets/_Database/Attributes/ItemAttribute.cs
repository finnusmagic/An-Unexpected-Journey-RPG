using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Database
{
    [System.Serializable]
    public class ItemAttribute
    {

        public string attributeName;
        public int attributeValue;
        public ItemAttribute(string attributeName, int attributeValue)
        {
            this.attributeName = attributeName;
            this.attributeValue = attributeValue;
        }

        public ItemAttribute() { }

    }
}
