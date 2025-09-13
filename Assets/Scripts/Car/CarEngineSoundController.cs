using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarEngineSoundController : MonoBehaviour
{
    private AudioClip _engineSoundClip;

    private float _minPitch = 0.5f;
    private float _groundMaxPitch = 2f;
    private float _airMaxPitch = 2.5f;
    private float _groundPitchSmooth = 5f;
    private float _airPitchSmooth = 10f;

    private AudioSource _audioSource;
    private Car _carScript;
    public void Init(Car carScript, AudioClip engineSoundClip, float minPitch, float groundMaxPitch, float airMaxPitch, float groundPitchSmooth, float airPitchSmooth)
    {
        // Инициализация полей / Fields Initialization

        _carScript = carScript;

        _engineSoundClip = engineSoundClip;

        _minPitch = minPitch;
        _groundMaxPitch = groundMaxPitch;
        _airMaxPitch = airMaxPitch;

        _groundPitchSmooth = groundPitchSmooth;
        _airPitchSmooth = airPitchSmooth;

        // Инициализация проигрывателя / Audio Source Initialization

        _audioSource = GetComponent<AudioSource>();

        _audioSource.clip = _engineSoundClip;
        _audioSource.loop = true;
        _audioSource.pitch = _minPitch;

        _audioSource.Play();
    }

    private void Update()
    {
        UpdateEngineSound();
    }

    private void UpdateEngineSound()
    {
        float speed = _carScript.GetCarController.GetRb.linearVelocity.magnitude;
        float maxSpeed = _carScript.GetCarSettings.EngineMaxSpeed;
        float t = Mathf.Clamp01(speed / maxSpeed);

        float targetPitch;

        if (_carScript.GetCarController.GetIsWorkingEngine)
        {
            if (_carScript.GetGroundCheck.IsGround)
            {
                targetPitch = Mathf.Lerp(_minPitch, _groundMaxPitch, t);
            }
            else
            {
                targetPitch = _airMaxPitch;
            }
        }
        else
        {
            targetPitch = _minPitch;
        }

        float smooth = _carScript.GetGroundCheck.IsGround ? _groundPitchSmooth : _airPitchSmooth;

        _audioSource.pitch = Mathf.Lerp(_audioSource.pitch, targetPitch, Time.deltaTime * smooth);
    }



}
