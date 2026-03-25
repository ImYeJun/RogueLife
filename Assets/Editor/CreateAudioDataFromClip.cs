using UnityEngine;
using UnityEditor;
using System.IO;

public class AudioDataCreator
{
    [MenuItem("Assets/Create/Scriptable Objects/AudioData from Clip", false)]
    public static void CreateAudioDataFromClip()
    {
        foreach (Object obj in Selection.objects)
        {
            AudioClip audioClip = obj as AudioClip;
            if (audioClip != null)
            {
                AudioData audioData = ScriptableObject.CreateInstance<AudioData>();

                SerializedObject so = new SerializedObject(audioData);
                so.FindProperty("clip").objectReferenceValue = audioClip;
                so.ApplyModifiedProperties();

                string clipPath = AssetDatabase.GetAssetPath(audioClip);
                string directory = Path.GetDirectoryName(clipPath);
                string fileName = audioClip.name + ".asset";
                string assetPath = Path.Combine(directory, fileName);

                assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);
                AssetDatabase.CreateAsset(audioData, assetPath);
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Create/Scriptable Objects/AudioData from Clip", true)]
    public static bool ValidateCreateAudioDataFromClip()
    {
        return Selection.activeObject is AudioClip;
    }
}