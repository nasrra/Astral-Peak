using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AnimationPathUpdater : EditorWindow
{
    [MenuItem("Tools/Update Animation Bone Paths")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AnimationPathUpdater));
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Update Animation Bone Paths"))
        {
            UpdateBonePaths();
        }
    }

    static void UpdateBonePaths()
    {
        // Get all animation clips in the selected GameObject
        GameObject selectedObj = Selection.activeGameObject;
        if (selectedObj == null)
        {
            Debug.LogError("No GameObject selected.");
            return;
        }

        Animator animator = selectedObj.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No Animator found on selected GameObject.");
            return;
        }

        // Get the current root bone path and the new path
        Transform[] allTransforms = selectedObj.GetComponentsInChildren<Transform>();
        Dictionary<string, string> oldToNewBonePaths = new Dictionary<string, string>();

        foreach (Transform t in allTransforms)
        {
            string oldPath = t.name; // Assume old name matches the bone name, adjust if needed
            string newPath = selectedObj.name + "/" + t.name; // Adjust this logic to match the new hierarchy
            oldToNewBonePaths[oldPath] = newPath;
        }

        // Iterate through all animation clips and update the bone paths
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            foreach (var binding in AnimationUtility.GetCurveBindings(clip))
            {
                if (binding.path.Contains(selectedObj.name)) // Check if the binding path needs updating
                {
                    // Update the curve with the new path
                    string updatedPath = oldToNewBonePaths.ContainsKey(binding.path) ? oldToNewBonePaths[binding.path] : binding.path;
                    AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);
                    AnimationUtility.SetEditorCurve(clip, binding, curve);
                }
            }

            // Refresh the animation to apply changes
            EditorUtility.SetDirty(clip);
        }

        AssetDatabase.SaveAssets();
        Debug.Log("Animation bone paths updated.");
    }
}

