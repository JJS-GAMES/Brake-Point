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

    [Tooltip("Front-wheel drive / �������� ������ �����")]
    public bool FrontWheel = false;
    [Tooltip("Back-wheel drive / ������ ������ �����")]
    public bool BackWheel = true;

    [Space, Range(0f, 100f), Tooltip("Traction multiplier in mud (higher = better performance) / ��������� ��������� � ������ ����� (���� = ����� ������������)")]
    public float MudTraction = 95f;

    [Space, Tooltip("Car mass (kg) / ����� ������ (��)")]
    public float Mass = 10f;
    [Tooltip("Engine Max Speed (km/h) / ������������ �������� ��� ���������� ��������� (��/�)")]
    public float MotorMaximumSpeed = 10f;
    [Range(1, 100), Tooltip("Maximum motor torque (affects acceleration) / ������������ �������� ������ ��������� (������ �� ���������)")]
    public float MaximumMotorTorque = 20f;

    [Range(0.1f, 10f), Tooltip("Brake force applied when braking (higher = stops faster) / ���� ���������� ��� ������� �� ������ (��� ����, ��� ������� ��������������� ������)")]
    public float BreakForce = 2f;

    [Space, Header("----------------------------"), Space]

    [Header("Flip Settings / ��������� ����������")]

    [Range(0.1f, 1f), Tooltip("How often to check the flip condition (seconds). / ��� ����� ��������� ������� ���������� (�������).")]
    public float FlipCheckInterval = 0.2f;
    [Range(-0.1f, 5f), Tooltip("The maximum speed below which we consider the car to have stopped. / ������������ ��������, ���� ������� ������� ������ ��������������.")]
    public float SpeedThreshold = 0.1f;
    [Range(-1f, 1f), Tooltip("Threshold for determining that the car is upside down: -1 = completely upside down, 0 = on its side, 1 = on wheels. / ����� ��� �����������, ��� ������ �����������: -1 = ��������� ����� ������, 0 = �� ����, 1 = �� ������.")]
    public float UpDotThreshold = -0.8f;

    [Space, Header("----------------------------"), Space]

    [Header("Physics Settings / ��������� ������")]

    [Tooltip("Physics material applied to the car's body (affects friction and bounciness) / ���������� ��������, ���������� �� ����� ���������� (������ �� ������ � ���������)")]
    public PhysicsMaterial2D CarPhysicsMaterial;
    [Range(0f, 1f), Tooltip("How much the car resists sliding (0 = no friction, 1 = maximum friction) / ��������� ���������� �������������� ���������� (0 = ���������� ������, 1 = ������������ ������)")]
    public float Friction = 0.6f;

    [Header("Air Speed Effect / ��������� �������� � �������")]

    [Range(5f, 100f), Tooltip("The force of rotation of the machine in the air / ���� �������� ������ � �������")]
    public float AirTorque = 20f;

    [Space, Header("----------------------------"), Space]

    [Space, Header("Suspension Settings / ��������� ��������")]
    [Range(0.8f, 1.2f), Tooltip("Front wheel traction multiplier (adjusts grip/slide) / ��������� ��������� ��������� ������ (���������� ����������/����������)")]
    public float FrontWheelTraction = 1f;
    [Range(0.8f, 1.2f), Tooltip("Back wheel traction multiplier (adjusts grip/slide) / ��������� ��������� ������� ������ (���������� ����������/����������)")]
    public float BackWheelTraction = 1f;

    [Space, Range(3f, 20f), Tooltip("Stiffness of the front suspension (range 3-20) / ��������� �������� �������� (�������� 3�20)")]
    public float FrontSuspensionStiffness = 8f;
    [Range(3f, 20f), Tooltip("Stiffness of the back suspension (range 3-12) / ��������� ������ �������� (�������� 3�12)")]
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
