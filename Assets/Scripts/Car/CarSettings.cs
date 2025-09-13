using UnityEngine;

[CreateAssetMenu(fileName = "Car Settings", menuName = "Car/Settings", order = 1)]
public class CarSettings : ScriptableObject
{
    [Header("Camera Settings / ��������� ������")]

    [Range(0.1f, 10f),Tooltip("Camera follow smoothness (lower value = smoother movement) / ��������� ���������� ������ �� ������� (������ �������� = ����� ������� ��������)")]
    public float Smooth = 5f;
    [Tooltip("Camera offset relative to the car / �������� ������ ������������ ������")]
    public Vector3 Offset = new Vector3(5, 3, 10);

    [Space, Header("----------------------------"), Space]

    [Header("Main Settings / �������� ���������")]

    [Tooltip("Car mass (kg) / ����� ������ (��)")]
    public float Mass = 10f;
    [Space, Tooltip("Engine Max Speed (km/h) / ������������ �������� ��� ���������� ��������� (��/�)")]
    public float EngineMaxSpeed = 10f;
    [Tooltip("Coast Max Speed (km/h) / ������������ �������� ������� �� ������� (��/�)")]
    public float CoastMaxSpeed = 5f;
    [Tooltip("Acceleration (higher value = faster speed gain) / ��������� (��� ���� ��������, ��� ������� ������)")]
    public float Acceleration = 10f;
    [Space, Range(0.1f, 10f), Tooltip("Brake force applied when braking (higher = stops faster) / ���� ���������� ��� ������� �� ������ (��� ����, ��� ������� ��������������� ������)")]
    public float BrakeForce = 2f;

    [Space]

    [Space, Header("----------------------------"), Space]

    [Header("Physics Settings / ��������� ������")]

    [Tooltip("Physics material applied to the car's body (affects friction and bounciness) / ���������� ��������, ���������� �� ����� ���������� (������ �� ������ � ���������)")]
    public PhysicsMaterial2D CarPhysicsMaterial;
    [Range(0f, 1f), Tooltip("How much the car resists sliding (0 = no friction, 1 = maximum friction) / ��������� ���������� �������������� ���������� (0 = ���������� ������, 1 = ������������ ������)")]
    public float Friction = 0.6f;

    [Header("Air Speed Effect / ��������� �������� � �������")]

    [Range(5f, 100f), Tooltip("The force of rotation of the machine in the air / ���� �������� ������ � �������")]
    public float AirTorque = 20f;
    [Tooltip("Deceleration of speed during ascent (when the car is flying up) / ���������� �������� ��� ������� (����� ������ ����� �����)")]
    public float AirSlowEffect = 3f;
    [Tooltip("Acceleration of speed during descent (when the car is flying down) / ��������� �������� ��� ������ (����� ������ ����� ����)")]
    public float AirBoostEffect = 7f;

    [Space, Header("----------------------------"), Space]

    [Space, Header("Suspension Settings / ��������� ��������")]
    [Range(3f, 12f), Tooltip("Stiffness of the front suspension (range 3-12) / ��������� �������� �������� (�������� 3�12)")]
    public float FrontSuspensionStiffness = 8f;
    [Range(3f, 12f), Tooltip("Stiffness of the back suspension (range 3-12) / ��������� ������ �������� (�������� 3�12)")]
    public float BackSuspensionStiffness = 8f;

    [Space, Header("----------------------------"), Space]

    [Space, Header("Particle Settings / ��������� ���������")]
    [Range(1f, 20f), Tooltip("Fade-out speed of particles when car is airborne / �������� ������������ ������, ����� ������ � �������")]
    public float DecaySpeed = 20f;
    [Range(1f, 20f), Tooltip("Fade-in speed of particles when car lands / �������� �������������� ������, ����� ������ ����� �������� �����")]
    public float RestoreSpeed = 10f;
    [Tooltip("Minimum car speed required to emit particles / ����������� �������� ������, ��� ������� �������� ���������������� �������")]
    public float MinSpeedToEmit = 0.1f;

    [Space, Header("----------------------------"), Space]

    [Space, Header("Engine Sound Settings / ��������� ����� ���������")]
    [Tooltip("���� ��������� / Engine Sound")]
    public AudioClip EngineSoundClip;

    [Header("Pitch Settings / ��������� ���� ���������")]
    [Tooltip("������� ��� ��������� ��� ����������� �������� / Lowest engine pitch when car is idle or very slow")]
    public float MinPitch = 0.5f;
    [Tooltip("������������ ��� ��������� �� ����� (������� �� ��������) / Max engine pitch on ground, scales with speed")]
    public float GroundMaxPitch = 2f;
    [Tooltip("������������ ��� ��������� � ������� (�������� ������������� �����) / Max engine pitch in air when wheels spin freely")]
    public float AirMaxPitch = 2.5f;

    [Header("Smoothing Settings / ��������� ����������� ������")]
    [Range(1f, 15f), Tooltip("�������� ����������� ��������� ����� �� ����� (��� ������ = ��� �������) / Pitch transition smoothness on ground")]
    public float GroundPitchSmooth = 5f;
    [Range(1f, 15f), Tooltip("�������� ����������� ��������� ����� � ������� (��� ������ = ��� �������) / Pitch transition smoothness in air")]
    public float AirPitchSmooth = 10f;
}
