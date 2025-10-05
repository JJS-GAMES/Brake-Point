using UnityEngine;

public class CarParticleController : MonoBehaviour
{
    [Header("Particle Systems / Партикл системы")]
    [SerializeField] private ParticleSystem _frontWheelParticleSystem;
    [SerializeField] private ParticleSystem _backWheelParticleSystem;

    private float _decaySpeed = 20f;
    private float _restoreSpeed = 10f;

    private CarController _carController;
    private GroundCheck _groundCheck;

    private ParticleSystem.EmissionModule _frontEmission;
    private ParticleSystem.EmissionModule _backEmission;

    private float _frontDefaultRate;
    private float _backDefaultRate;

    private float _frontCurrentRate = 0f;
    private float _backCurrentRate = 0f;

    private ParticleSystem.MinMaxCurve _reusableCurve = new ParticleSystem.MinMaxCurve(0f);

    [SerializeField, Tooltip("How often (seconds) to update particle emission. Lower = cheaper.")]
    private float _updateInterval = 0.05f; // 20 Hz

    private float _updateTimer = 0f;

    private bool _frontWheel = false;
    private bool _backWheel = true;

    private bool _frontWasPlaying = false;
    private bool _backWasPlaying = false;
    private bool _frontEmissionEnabled = false;
    private bool _backEmissionEnabled = false;

    public void Init(CarController carController, GroundCheck groundCheck, bool frontWheel, bool backWheel, float decaySpeed, float restoreSpeed, float minSpeedToEmit)
    {
        _carController = carController;
        _groundCheck = groundCheck;

        _frontWheel = frontWheel;
        _backWheel = backWheel;

        _decaySpeed = decaySpeed;
        _restoreSpeed = restoreSpeed;

        CacheDefaultRates();
    }

    private void Start()
    {
        CacheDefaultRates();
    }

    private void CacheDefaultRates()
    {
        if (_frontWheelParticleSystem != null && _frontWheel)
        {
            _frontEmission = _frontWheelParticleSystem.emission;
            _frontDefaultRate = _frontEmission.rateOverTime.constant;
            _frontCurrentRate = _frontDefaultRate;
            _frontWasPlaying = _frontWheelParticleSystem.isPlaying;
            _frontEmissionEnabled = _frontEmission.enabled;
        }
        else _frontWheelParticleSystem?.gameObject.SetActive(false);

        if (_backWheelParticleSystem != null && _backWheel)
        {
            _backEmission = _backWheelParticleSystem.emission;
            _backDefaultRate = _backEmission.rateOverTime.constant;
            _backCurrentRate = _backDefaultRate;
            _backWasPlaying = _backWheelParticleSystem.isPlaying;
            _backEmissionEnabled = _backEmission.enabled;
        }
        else _backWheelParticleSystem?.gameObject.SetActive(false);
    }

    private void Update()
    {
        _updateTimer += Time.deltaTime;
        if (_updateTimer < _updateInterval) return;
        _updateTimer = 0f;

        UpdateParticles();
    }

    private void UpdateParticles()
    {
        if (_carController == null || _groundCheck == null)
        {
            if (_frontWheelParticleSystem != null && _frontWheel)
            {
                StopAndDisable(_frontWheelParticleSystem, ref _frontEmission, ref _frontCurrentRate, ref _frontWasPlaying, ref _frontEmissionEnabled);
            }
            if (_backWheelParticleSystem != null && _backWheel)
            {
                StopAndDisable(_backWheelParticleSystem, ref _backEmission, ref _backCurrentRate, ref _backWasPlaying, ref _backEmissionEnabled);
            }
            return;
        }

        float carSpeed = 0f;
        var rb = _carController.GetRb;
        if (rb != null)
        {
            carSpeed = Mathf.Abs(rb.linearVelocity.x);
        }

        bool grounded = _groundCheck.IsGround;

        if (grounded && _carController.IsWorkingEngine)
        {
            if (_frontWheelParticleSystem != null && _frontWheel)
            {
                float targetFront = _frontDefaultRate;
                float nextFront = Mathf.MoveTowards(_frontCurrentRate, targetFront, _restoreSpeed * Time.deltaTime / _updateInterval);

                ApplyEmission(_frontWheelParticleSystem, ref _frontEmission, ref _frontCurrentRate, nextFront, ref _frontWasPlaying, ref _frontEmissionEnabled);
            }
            if (_backWheelParticleSystem != null && _backWheel)
            {
                float targetBack = _backDefaultRate;
                float nextBack = Mathf.MoveTowards(_backCurrentRate, targetBack, _restoreSpeed * Time.deltaTime / _updateInterval);

                ApplyEmission(_backWheelParticleSystem, ref _backEmission, ref _backCurrentRate, nextBack, ref _backWasPlaying, ref _backEmissionEnabled);
            }
        }
        else if (!grounded)
        {
            if (_frontWheelParticleSystem != null && _frontWheel)
            {
                float nextFront = Mathf.MoveTowards(_frontCurrentRate, 0f, _decaySpeed * Time.deltaTime / _updateInterval);
                ApplyEmission(_frontWheelParticleSystem, ref _frontEmission, ref _frontCurrentRate, nextFront, ref _frontWasPlaying, ref _frontEmissionEnabled);
            }
            if (_backWheelParticleSystem != null && _backWheel)
            {
                float nextBack = Mathf.MoveTowards(_backCurrentRate, 0f, _decaySpeed * Time.deltaTime / _updateInterval);
                ApplyEmission(_backWheelParticleSystem, ref _backEmission, ref _backCurrentRate, nextBack, ref _backWasPlaying, ref _backEmissionEnabled);
            }
        }
        else
        {
            if (_frontWheelParticleSystem != null && _frontWheel)
            {
                StopAndDisable(_frontWheelParticleSystem, ref _frontEmission, ref _frontCurrentRate, ref _frontWasPlaying, ref _frontEmissionEnabled);
            }
            if (_backWheelParticleSystem != null && _backWheel)
            {
                StopAndDisable(_backWheelParticleSystem, ref _backEmission, ref _backCurrentRate, ref _backWasPlaying, ref _backEmissionEnabled);
            }
        }
    }

    private void ApplyEmission(ParticleSystem ps, ref ParticleSystem.EmissionModule emission, ref float currentRate, float targetRate, ref bool wasPlaying, ref bool emissionEnabled)
    {
        if (ps == null) return;

        if (!Mathf.Approximately(currentRate, targetRate))
        {
            currentRate = targetRate;
            _reusableCurve.constant = currentRate;
            emission.rateOverTime = _reusableCurve;
        }

        bool shouldEmit = currentRate > 0.01f;

        if (shouldEmit != emissionEnabled)
        {
            emission.enabled = shouldEmit;
            emissionEnabled = shouldEmit;
        }

        if (shouldEmit)
        {
            if (!ps.isPlaying)
            {
                ps.Play();
                wasPlaying = true;
            }
        }
        else
        {
            if (ps.isPlaying)
            {
                ps.Stop();
                wasPlaying = false;
            }
        }
    }

    private void StopAndDisable(ParticleSystem ps, ref ParticleSystem.EmissionModule emission, ref float currentRate, ref bool wasPlaying, ref bool emissionEnabled)
    {
        if (ps == null) return;

        if (!Mathf.Approximately(currentRate, 0f))
        {
            currentRate = 0f;
            _reusableCurve.constant = 0f;
            emission.rateOverTime = _reusableCurve;
        }

        if (emissionEnabled)
        {
            emission.enabled = false;
            emissionEnabled = false;
        }

        if (ps.isPlaying)
        {
            ps.Stop();
            wasPlaying = false;
        }
    }

    private void OnDisable()
    {
        StopAndDisable(_frontWheelParticleSystem, ref _frontEmission, ref _frontCurrentRate, ref _frontWasPlaying, ref _frontEmissionEnabled);
        StopAndDisable(_backWheelParticleSystem, ref _backEmission, ref _backCurrentRate, ref _backWasPlaying, ref _backEmissionEnabled);
    }
}
