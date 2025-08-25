using UnityEngine;

public class CarParticleController : MonoBehaviour
{
    [Header("Particle Panels / Панели частиц")]
    [SerializeField] private ParticleSystem _frontWheelParticleSystem;
    [SerializeField] private ParticleSystem _backWheelParticleSystem;

    [Header("Behavior Settings / Настройки поведения")]
    [SerializeField, Range(1, 20)] private float _decaySpeed = 10f;
    [SerializeField, Range(1, 20)] private float _restoreSpeed = 20f;
    [SerializeField] private float _minSpeedToEmit = 0.1f;

    private CarController _carController;
    private GroundCheck _groundCheck;

    private float _frontDefaultRate;
    private float _backDefaultRate;

    public void Init(CarController carController, GroundCheck groundCheck)
    {
        _carController = carController;
        _groundCheck = groundCheck;
        CacheDefaultRates();
    }

    private void Start()
    {
        CacheDefaultRates();
    }

    private void CacheDefaultRates()
    {
        if (_frontWheelParticleSystem != null)
            _frontDefaultRate = _frontWheelParticleSystem.emission.rateOverTime.constant;

        if (_backWheelParticleSystem != null)
            _backDefaultRate = _backWheelParticleSystem.emission.rateOverTime.constant;
    }

    private void Update()
    {
        UpdateParticles();
    }

    private void UpdateParticles()
    {
        if (_carController == null || _groundCheck == null)
        {
            StopAndDisable(_frontWheelParticleSystem);
            StopAndDisable(_backWheelParticleSystem);
            return;
        }

        float carSpeed = 0f;
        var rb = _carController.GetRb;
        if (rb != null) carSpeed = Mathf.Abs(rb.linearVelocity.x);

        bool grounded = _groundCheck.IsGround;

        if (grounded && carSpeed > _minSpeedToEmit)
        {
            float nextFront = Mathf.MoveTowards(_frontWheelParticleSystem?.emission.rateOverTime.constant ?? 0f, _frontDefaultRate, _restoreSpeed * Time.deltaTime);
            float nextBack = Mathf.MoveTowards(_backWheelParticleSystem?.emission.rateOverTime.constant ?? 0f, _backDefaultRate, _restoreSpeed * Time.deltaTime);

            SetEmissionRateAndPlayIfNeeded(_frontWheelParticleSystem, nextFront);
            SetEmissionRateAndPlayIfNeeded(_backWheelParticleSystem, nextBack);
        }
        else if (!grounded && carSpeed > _minSpeedToEmit)
        {
            float nextFront = Mathf.MoveTowards(_frontWheelParticleSystem?.emission.rateOverTime.constant ?? 0f, 0f, _decaySpeed * Time.deltaTime);
            float nextBack = Mathf.MoveTowards(_backWheelParticleSystem?.emission.rateOverTime.constant ?? 0f, 0f, _decaySpeed * Time.deltaTime);

            SetEmissionRateAndPlayIfNeeded(_frontWheelParticleSystem, nextFront);
            SetEmissionRateAndPlayIfNeeded(_backWheelParticleSystem, nextBack);
        }
        else
        {
            StopAndDisable(_frontWheelParticleSystem);
            StopAndDisable(_backWheelParticleSystem);
        }
    }

    private void SetEmissionRateAndPlayIfNeeded(ParticleSystem ps, float rate)
    {
        if (ps == null) return;

        var emission = ps.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(rate);

        if (rate > 0.01f)
        {
            emission.enabled = true;
            if (!ps.isPlaying) ps.Play();
        }
        else
        {
            emission.enabled = false;
            if (ps.isPlaying) ps.Stop();
        }
    }

    private void StopAndDisable(ParticleSystem ps)
    {
        if (ps == null) return;
        var emission = ps.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(0f);
        emission.enabled = false;
        if (ps.isPlaying) ps.Stop();
    }
}
