using UnityEngine;

[CreateAssetMenu(fileName = "Car Settings", menuName = "Car/Settings", order = 1)]
public class CarSettings : ScriptableObject
{
    [Header("Camera Settings / Настройки камеры")]

    [Range(0.1f, 10),Tooltip("Camera follow smoothness (lower value = smoother movement) / Плавность следования камеры за машиной (меньше значение = более плавное движение)")]
    public float Smooth = 5f;
    [Tooltip("Camera offset relative to the car / Смещение камеры относительно машины")]
    public Vector3 Offset = new Vector3(5, 3, 10);

    [Header("Main Settings / Основные настройки")]

    [Tooltip("Car mass (kg) / Масса машины (кг)")]
    public float Mass = 10f;
    [Tooltip("Engine Max Speed (km/h) / Максимальная скорость при работающем двигателе (км/ч)")]
    public float EngineMaxSpeed = 10f;
    [Tooltip("Coast Max Speed (km/h) / Максимальная скорость машинки по инерции (км/ч)")]
    public float CoastMaxSpeed = 5f;
    [Tooltip("Acceleration (higher value = faster speed gain) / Ускорение (чем выше значение, тем быстрее разгон)")]
    public float Acceleration = 10f;

    [Space, Tooltip("Is engine running from beginning? / Работает ли двигатель с самого начала?")]
    public bool IsWorkingEngine = true;
    [Space]

    [Header("Physics Settings / Настройки физики")]

    [Tooltip("Physics material applied to the car's body (affects friction and bounciness) / Физический материал, нанесенный на кузов автомобиля (влияет на трение и упругость)")]
    public PhysicsMaterial2D CarPhysicsMaterial;
    [Range(0, 1), Tooltip("How much the car resists sliding (0 = no friction, 1 = maximum friction) / Насколько автомобиль сопротивляется скольжению (0 = отсутствие трения, 1 = максимальное трение)")]
    public float Friction = 0.6f;

    [Header("Air Speed Effect / Настройки скорости в воздухе")]

    [Tooltip("Deceleration of speed during ascent (when the car is flying up) / Замедление скорости при подъёме (когда машина летит вверх)")]
    public float AirSlowEffect = 3f;
    [Tooltip("Acceleration of speed during descent (when the car is flying down) / Ускорение скорости при спуске (когда машина летит вниз)")]
    public float AirBoostEffect = 7f;

    [Space, Header("Suspension Settings / Настройки подвески")]
    [Range(3, 12), Tooltip("Stiffness of the front suspension (range 3-12) / Жесткость передней подвески (диапазон 3–12)")]
    public float FrontSuspensionStiffness = 8f;
    [Range(3, 12), Tooltip("Stiffness of the back suspension (range 3-12) / Жесткость задней подвески (диапазон 3–12)")]
    public float BackSuspensionStiffness = 8f;

    [Header("Particle Settings / Настройки партиклов")]
    [Range(1, 20), Tooltip("Fade-out speed of particles when car is airborne / Скорость исчезновения частиц, когда машина в воздухе")]
    public float DecaySpeed = 20f;

    [Range(1, 20), Tooltip("Fade-in speed of particles when car lands / Скорость восстановления частиц, когда машина снова касается земли")]
    public float RestoreSpeed = 10f;

    [Tooltip("Minimum car speed required to emit particles / Минимальная скорость машины, при которой начинают воспроизводиться частицы")]
    public float MinSpeedToEmit = 0.1f;
}
