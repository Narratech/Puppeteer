﻿using UnityEngine;
using System.Collections;
using UnityEditor;

public class SequenceNodeAsset : SequenceNode {

    public override object Content
    {
        get
        {
            return base.Content;
        }

        set
        {
			#if UNITY_EDITOR
			if (value != null && value is Object)
	            {
	                var objectVal = value as Object;
	                if (value != Content)
	                {
	                    if (objectVal is IAssetSerializable)
	                    {
	                        (objectVal as IAssetSerializable).SerializeInside(this);
	                        AssetDatabase.SaveAssets();
	                    }
	                    else if (!AssetDatabase.IsMainAsset(objectVal) && !AssetDatabase.IsSubAsset(objectVal))
	                    {
	                        AssetDatabase.AddObjectToAsset(objectVal, this);
	                        AssetDatabase.SaveAssets();
	                    }
	                }
	            }
	                

	            if (isUnityObject && value != Content && Content != null && AssetDatabase.IsSubAsset(objectContent))
	            {
	                ScriptableObject.DestroyImmediate(objectContent, true);
	                AssetDatabase.SaveAssets();
				}

			#endif

            base.Content = value;
        }
    }

    protected virtual void OnDestroy()
    {
		#if UNITY_EDITOR

		if (!Application.isPlaying && isUnityObject)
            ScriptableObject.DestroyImmediate(objectContent, true);

		#endif 
    }
}
