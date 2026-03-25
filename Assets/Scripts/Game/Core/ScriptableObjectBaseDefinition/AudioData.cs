using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "Scriptable Objects")]
public class AudioData : ScriptableObject {
    [SerializeField] private AudioClip clip;

    public AudioClip Clip { get => clip; }
}