﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Dialog))]
public class DialogEditor : Editor {

    private void OnEnable()
    {
        // Add listeners to draw events

        fragmentsReorderableList = new ReorderableList(new ArrayList(), typeof(Fragment), true, true, true, true);
        fragmentsReorderableList.drawHeaderCallback += DrawFragmentsHeader;
        fragmentsReorderableList.drawElementCallback += DrawFragment;
        fragmentsReorderableList.onAddCallback += AddFragment;
        fragmentsReorderableList.onRemoveCallback += RemoveFragment;
        fragmentsReorderableList.onReorderCallback += ReorderFragments;
    }

    private ReorderableList fragmentsReorderableList;
    private Dialog dialog;
    private Vector2 scroll = Vector2.zero;

    public override void OnInspectorGUI()
    {
        dialog = target as Dialog;

        GUIStyle style = new GUIStyle();
        style.padding = new RectOffset(5, 5, 5, 5);
        dialog.id = UnityEditor.EditorGUILayout.TextField("Name", dialog.id);

        fragmentsReorderableList.list = dialog.Fragments;

        EditorGUILayout.HelpBox("You have to add at least one", MessageType.None);
        if (fragmentsReorderableList.list != null)
        {
            bool isScrolling = false;
            if (fragmentsReorderableList.list.Count > 3)
            {
                scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.ExpandWidth(true), GUILayout.Height(250));
                isScrolling = true;
            }

            fragmentsReorderableList.elementHeight = fragmentsReorderableList.list.Count == 0 ? 20 : 70;
            fragmentsReorderableList.DoLayoutList();

            if (isScrolling)
                EditorGUILayout.EndScrollView();
        }
    }



    private Rect moveRect(Rect target, Rect move)
    {
        Rect r = new Rect(move.x + target.x, move.y + target.y, target.width, target.height);

        if (r.x + r.width > move.x + move.width)
        {
            r.width = (move.width + 25) - r.x;
        }

        return r;
    }

    /*****************************
     * FRAGMENTS LIST OPERATIONS
     *****************************/

    Rect entityRect = new Rect(0, 2, 40, 15);
    Rect characterRect = new Rect(0, 2, 95, 15);
    Rect parameterRect = new Rect(100, 2, 190, 15);
    Rect nameRect = new Rect(0, 20, 190, 15);
    Rect textRect = new Rect(0, 35, 190, 30);
    private void DrawFragmentsHeader(Rect rect)
    {
        GUI.Label(rect, "Dialog fragments");
    }

    private void DrawFragment(Rect rect, int index, bool active, bool focused)
    {
        Fragment frg = (Fragment)fragmentsReorderableList.list[index];

        EditorGUI.LabelField(moveRect(entityRect, rect), "Target: ");
        frg.Character = EditorGUI.TextField(moveRect(characterRect, rect), frg.Character);
        frg.Parameter = EditorGUI.TextField(moveRect(parameterRect, rect), frg.Parameter);
        frg.Name = EditorGUI.TextField(moveRect(nameRect, rect), frg.Name);
        frg.Msg = EditorGUI.TextArea(moveRect(textRect, rect), frg.Msg);

        // If you are using a custom PropertyDrawer, this is probably better
        // EditorGUI.PropertyField(rect, serializedObject.FindProperty("list").GetArrayElementAtIndex(index));
        // Although it is probably smart to cach the list as a private variable ;)
    }

    private void AddFragment(ReorderableList list)
    {
        dialog.AddFragment();
    }

    private void RemoveFragment(ReorderableList list)
    {
        dialog.RemoveFragment(dialog.Fragments[list.index]);

    }

    private void ReorderFragments(ReorderableList list)
    {
        List<Fragment> l = (List<Fragment>)fragmentsReorderableList.list;
        dialog.Fragments = l;
    }

}