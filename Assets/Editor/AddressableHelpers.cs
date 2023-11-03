using UnityEditor;
using UnityEngine;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using System.IO;

#if UNITY_EDITOR

public class AddressableHelpers : MonoBehaviour
{
    [MenuItem("Assets/ConvertToAddressable")]
    static void ConvertToAddressable()
    {
        // Do something with you variable
        // Debug.Log("Selected type: " + Selection.activeObject?.GetType()?.Name);
        // Debug.Log(Selection.activeObject?.name);
        // Debug.Log(AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID()));

        if (Selection.activeObject == null)
        {
            return;
        }

        var path = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
        Debug.Log($"Convert '{path}' to addressable.");
        ConvertAssetToAddressable(path, "Garden", "Garden");     
    }

    [MenuItem("Assets/ConvertToAddressable", true)]
    static bool ConvertToAddressableValidation()
    {
        var obj = Selection.activeObject;
        // This returns true when the selected object is an Asset (the menu item will be disabled otherwise).
        return obj is DefaultAsset || obj is Material || obj is SceneAsset || obj is GameObject;
    }

    private static void ConvertAssetToAddressable(string path, string groupName, string labelName)
    {

        if (Directory.Exists(path))
        {
             Debug.Log($"Iterate over path '{path}'.");
            // Path is a directory
            // Convert files to addressables            
            DirectoryInfo dir = new DirectoryInfo(path);
            var fullNames =  dir.GetFiles();
            foreach (var file in fullNames) 
            {
                ConvertAssetToAddressable(file.FullName, groupName, labelName);
            }

            // Recursivly convert sub directories
            var directoryNames = dir.GetDirectories(); 
            foreach(var directory in directoryNames)
            {
                ConvertAssetToAddressable(directory.FullName, groupName, labelName);
            }

            return;
        }

        var relativePath = Path.Join("Assets", Path.GetRelativePath(Application.dataPath, path)).ToString().Replace('\\','/');
        if (relativePath != path)
        {
            path = relativePath;
        }
        
        // Path is a file convert it to addressable and assign group        
        var settings = AddressableAssetSettingsDefaultObject.Settings;

        // Make a gameobject an addressable
        var group = settings.FindGroup(groupName);
        var guid = AssetDatabase.AssetPathToGUID(path);

        if (guid == "")
            return;

        Debug.Log($"Convert file '{path}' guid: {guid} to addressable.");

        // This is the function that actually makes the object addressable
        var entry = settings.CreateOrMoveEntry(guid, group);
        // entry.labels.Add(labelName);
        
        // You'll need these to run to save the changes!
        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
        AssetDatabase.SaveAssets();
    }

        // var settings = AddressableAssetSettingsDefaultObject.Settings;



        // Create and Remove groups and labels and custom address:

        // string group_name = "YourGroupName";

        // string label_name = "YourLabelName";

        // string path_to_object = "Assets/path/to/your/object/";

        // string custom_address = "Custom address here";



        // //Create a group with no schemas

        // //settings.CreateGroup(group_name, false, false, false, new List<AddressableAssetGroupSchema> { settings.DefaultGroup.Schemas[0] });

        // //Create a group with the default schemas

        // settings.CreateGroup(sim_name, false, false, false, settings.DefaultGroup.Schemas);



        // //Create a Label

        // settings.AddLabel(label_name, false);



        // //Remove a group

        // AddressableAssetGroup g = settings.FindGroup(group_name);

        // settings.RemoveGroup(g);

        // //Remove a label

        // settings.RemoveLabel(label_name, false);

}

#endif