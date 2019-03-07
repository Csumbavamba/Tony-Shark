using UnityEngine;

[CreateAssetMenu(menuName = "WaveSetting/MovementSettings")]
public class WaveMovementSettings : ScriptableObject
{
    public float TravelSpeed;
    public float RaiseSpeed;
    public float WaveRaiseHeight;
    public float TravelDistance;
}
