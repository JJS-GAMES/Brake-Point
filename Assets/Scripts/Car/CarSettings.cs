using UnityEngine;

[CreateAssetMenu(fileName = "Car Settings", menuName = "Car/Settings", order = 1)]
public class CarSettings : ScriptableObject
{
    [Header("Main Settings / Основные настройки")]

    [Tooltip("Car mass (kg) / Масса машины (кг)")]
    public float Mass = 10f;
    [Tooltip("Car Speed (km/h) / Максимальная cкорость машинки (км/ч)")]
    public float MaxSpeed = 10f;
    [Tooltip("Acceleration (higher value = faster speed gain) / Ускорение (чем выше значение, тем быстрее разгон)")]
    public float Acceleration = 10f;
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
    [Space]

    [Header("Suspension Settings / Настройки подвески")]
    [Range(3, 12), Tooltip("Stiffness of the front suspension (range 3-12) / Жесткость передней подвески (диапазон 3–12)")]
    public float FrontSuspensionStiffness = 8f;
    [Range(3, 12), Tooltip("Stiffness of the back suspension (range 3-12) / Жесткость задней подвески (диапазон 3–12)")]
    public float BackSuspensionStiffness = 8f;
}
