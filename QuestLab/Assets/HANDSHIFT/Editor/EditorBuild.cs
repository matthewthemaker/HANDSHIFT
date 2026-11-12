using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;

namespace HandShift.Quest.Editor
{
    public static class EditorBuild
    {
        private const string ScenePath = "Assets/Scenes/HANDSHIFT-Lab.unity";

        [MenuItem("HANDSHIFT/Build Quest Lab APK")]
        public static void BuildAndroid()
        {
            Directory.CreateDirectory("Assets/Scenes");
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var rig = new GameObject("HANDSHIFT_XR_Origin");
            var origin = rig.AddComponent<XROrigin>();
            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.SetParent(rig.transform, false);
            var camera = cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();
            origin.Camera = camera;

            var tracking = rig.AddComponent<HandShiftHandTracker>();
            var visualizer = rig.AddComponent<HandJointVisualizer>();
            var reality = rig.AddComponent<RealitySenseWorker>();
            var cameraPermission = rig.AddComponent<CameraPermissionGate>();
            var serializedPermission = new SerializedObject(cameraPermission);
            serializedPermission.FindProperty("worker").objectReferenceValue = reality;
            serializedPermission.ApplyModifiedPropertiesWithoutUndo();
            var gameFirst = rig.AddComponent<GameFirstRuntime>();

            var serializedVisualizer = new SerializedObject(visualizer);
            serializedVisualizer.FindProperty("tracker").objectReferenceValue = tracking;
            serializedVisualizer.ApplyModifiedPropertiesWithoutUndo();

            var serializedGameFirst = new SerializedObject(gameFirst);
            serializedGameFirst.FindProperty("handTracker").objectReferenceValue = tracking;
            serializedGameFirst.FindProperty("realitySense").objectReferenceValue = reality;
            serializedGameFirst.ApplyModifiedPropertiesWithoutUndo();

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };

            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.handshift.questlab");
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);

            Directory.CreateDirectory("Build");
            var options = new BuildPlayerOptions {
                scenes = new[] { ScenePath },
                locationPathName = "Build/HANDSHIFT-QuestLab.apk",
                target = BuildTarget.Android,
                options = BuildOptions.None
            };
            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
                throw new System.Exception("HANDSHIFT Quest APK build failed: " + report.summary.result);
            Debug.Log("HANDSHIFT APK built: " + options.locationPathName);
        }
    }
}
