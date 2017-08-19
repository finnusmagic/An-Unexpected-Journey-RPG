using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Database
{
    [System.Serializable]
    public class ItemAttribute
    {

        public string attributeName;
        public float attributeValue;
        public ItemAttribute(string attributeName, float attributeValue)
        {
            this.attributeName = attributeName;
            this.attributeValue = attributeValue;
        }

        public ItemAttribute() { }

    }
}
