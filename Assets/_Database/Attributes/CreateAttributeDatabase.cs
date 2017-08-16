﻿using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;

namespace RPG.Database
{
    public class CreateAttributeDatabase : MonoBehaviour
    {

        public static ItemAttributeList asset;                                                  //The List of all Items

#if UNITY_EDITOR
        public static ItemAttributeList createItemAttributeDatabase()                                    //creates a new ItemDatabase(new instance)
        {
            asset = ScriptableObject.CreateInstance<ItemAttributeList>();                       //of the ScriptableObject InventoryItemList

            AssetDatabase.CreateAsset(asset, "Assets/Resources/Database/AttributeDatabase.asset");            //in the Folder Assets/Resources/ItemDatabase.asset
            AssetDatabase.SaveAssets();                                                         //and than saves it there        
            return asset;
        }
#endif

    }
}