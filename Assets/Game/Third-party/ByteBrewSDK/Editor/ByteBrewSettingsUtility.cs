﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class ByteBrewSettingsUtility
{
	[MenuItem("Window/ByteBrew/Create ByteBrew GameObject")]
	public static void CreateByteBrewGameObject()
	{

		// Make sure the file name is unique, in case an existing Prefab has the same name.
		Object bytePref = AssetDatabase.LoadAssetAtPath("Assets/Game/Third-party/ByteBrewSDK/Prefabs/ByteBrew.prefab", typeof(GameObject));

		// Create the new Prefab.
		PrefabUtility.InstantiatePrefab(bytePref);
		EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
	}

	[MenuItem("Window/ByteBrew/Select ByteBrew settings")]
	public static void SelectSettings()
	{
		//ByteBrewSettings asset = ScriptableObject.CreateInstance<ByteBrewSettings>();

		//AssetDatabase.CreateAsset(asset, "Assets/Game/Third-party/ByteBrewSDK/Resources/ByteBrewSettings.asset");

		ByteBrewSettings asset = AssetDatabase.LoadAssetAtPath<ByteBrewSettings>("Assets/Game/Third-party/ByteBrewSDK/Resources/ByteBrewSettings.asset");

		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;
	}
}
