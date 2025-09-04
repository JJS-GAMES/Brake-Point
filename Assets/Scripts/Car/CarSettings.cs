using UnityEngine;

[CreateAssetMenu(fileName = "Car Settings", menuName = "Car/Settings", order = 1)]
public class CarSettings : ScriptableObject
{
    [Header("Camera Settings / ��������� ������")]

    [Range(0.1f, 10),Tooltip("Camera follow smoothness (lower value = smoother movement) / ��������� ���������� ������ �� ������� (������ �������� = ����� ������� ��������)")]
    public float Smooth = 5f;
    [Tooltip("Camera offset relative to the car / �������� ������ ������������ ������")]
    public Vector3 Offset = new Vector3(5, 3, 10);

    [Header("Main Settings / �������� ���������")]

    [Tooltip("Car mass (kg) / ����� ������ (��)")]
    public float Mass = 10f;
    [Tooltip("Engine Max Speed (km/h) / ������������ �������� ��� ���������� ��������� (��/�)")]
    public float EngineMaxSpeed = 10f;
    [Tooltip("Coast Max Speed (km/h) / ������������ �������� ������� �� ������� (��/�)")]
    public float CoastMaxSpeed = 5f;
    [Tooltip("Acceleration (higher value = faster speed gain) / ��������� (��� ���� ��������, ��� ������� ������)")]
    public float Acceleration = 10f;

    [Space, Tooltip("Is engine running from beginning? / �������� �� ��������� � ������ ������?")]
    public bool IsWorkingEngine = true;
    [Space]

    [Header("Physics Settings / ��������� ������")]

    [Tooltip("Physics material applied to the car's body (affects friction and bounciness) / ���������� ��������, ���������� �� ����� ���������� (������ �� ������ � ���������)")]
    public PhysicsMaterial2D CarPhysicsMaterial;
    [Range(0, 1), Tooltip("How much the car resists sliding (0 = no friction, 1 = maximum friction) / ��������� ���������� �������������� ���������� (0 = ���������� ������, 1 = ������������ ������)")]
    public float Friction = 0.6f;

    [Header("Air Speed Effect / ��������� �������� � �������")]

    [Tooltip("Deceleration of speed during ascent (when the car is flying up) / ���������� �������� ��� ������� (����� ������ ����� �����)")]
    public float AirSlowEffect = 3f;
    [Tooltip("Acceleration of speed during descent (when the car is flying down) / ��������� �������� ��� ������ (����� ������ ����� ����)")]
    public float AirBoostEffect = 7f;

    [Space, Header("Suspension Settings / ��������� ��������")]
    [Range(3, 12), Tooltip("Stiffness of the front suspension (range 3-12) / ��������� �������� �������� (�������� 3�12)")]
    public float FrontSuspensionStiffness = 8f;
    [Range(3, 12), Tooltip("Stiffness of the back suspension (range 3-12) / ��������� ������ �������� (�������� 3�12)")]
    public float BackSuspensionStiffness = 8f;

    [Header("Particle Settings / ��������� ���������")]
    [Range(1, 20), Tooltip("Fade-out speed of particles when car is airborne / �������� ������������ ������, ����� ������ � �������")]
    public float DecaySpeed = 20f;

    [Range(1, 20), Tooltip("Fade-in speed of particles when car lands / �������� �������������� ������, ����� ������ ����� �������� �����")]
    public float RestoreSpeed = 10f;

    [Tooltip("Minimum car speed required to emit particles / ����������� �������� ������, ��� ������� �������� ���������������� �������")]
    public float MinSpeedToEmit = 0.1f;
}
