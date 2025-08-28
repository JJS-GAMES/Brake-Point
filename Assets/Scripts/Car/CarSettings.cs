using UnityEngine;

[CreateAssetMenu(fileName = "Car Settings", menuName = "Car/Settings", order = 1)]
public class CarSettings : ScriptableObject
{
    [Header("Main Settings / �������� ���������")]

    [Tooltip("Car mass (kg) / ����� ������ (��)")]
    public float Mass = 10f;
    [Tooltip("Car Speed (km/h) / ������������ c������� ������� (��/�)")]
    public float MaxSpeed = 10f;
    [Tooltip("Acceleration (higher value = faster speed gain) / ��������� (��� ���� ��������, ��� ������� ������)")]
    public float Acceleration = 10f;
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
    [Space]

    [Header("Suspension Settings / ��������� ��������")]
    [Range(3, 12), Tooltip("Stiffness of the front suspension (range 3-12) / ��������� �������� �������� (�������� 3�12)")]
    public float FrontSuspensionStiffness = 8f;
    [Range(3, 12), Tooltip("Stiffness of the back suspension (range 3-12) / ��������� ������ �������� (�������� 3�12)")]
    public float BackSuspensionStiffness = 8f;
}
