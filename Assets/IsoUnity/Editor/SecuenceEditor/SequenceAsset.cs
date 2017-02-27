﻿using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SequenceAsset : Sequence {

    [SerializeField]
    private bool assetinited = false;

    void OnEnable()
    {
        if (!assetinited)
        {
            InitAsset();
        }
    }

    public void InitAsset()
    {
        if(this.localVariables == null){
            this.localVariables = ScriptableObject.CreateInstance<IsoSwitches>();
        }

        AssetDatabase.AddObjectToAsset(this.localVariables, this);
        AssetDatabase.SaveAssets();
        assetinited = true;
    }

    public override SequenceNode CreateNode(string id, object content = null, int childSlots = 0)
    {
        var node = CreateInstance<SequenceNodeAsset>();
        AssetDatabase.AddObjectToAsset(node, this);

        node.init(this);
        this.nodeDict.Add(id, node);
        node.Content = content;

        AssetDatabase.SaveAssets();

        return node;
    }

    public override SequenceNode CreateNode(object content = null, int childSlots = 0)
    {
        var node = CreateInstance<SequenceNodeAsset>();

        AssetDatabase.AddObjectToAsset(node, this);

        node.init(this);
        this.nodeDict.Add(node.GetInstanceID().ToString(), node);
        node.Content = content;

        AssetDatabase.SaveAssets();

        return node;
    }
    
    public override bool RemoveNode(SequenceNode node)
    {
        var r = base.RemoveNode(node);
        if (r)
            AssetDatabase.SaveAssets();
        return r;
    }
}
