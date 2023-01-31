using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;

public class ActionEffect : MonoBehaviour
{
    public static ActionEffect instance;

    [HideInInspector] public bool play = false;
    public bool recovery;

    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] CinemachineBasicMultiChannelPerlin shakeChannel;
    [SerializeField] private Volume volume;
    [SerializeField] private Vignette vignette;
    [SerializeField] private ChromaticAberration chromaticAberration;
    [SerializeField] private ColorAdjustments colorAdjustments;

    [Header("Zoom")]
    private float effectDuration = 0.5f;
    [SerializeField] private float recoveryDuration = 0.5f;
    private float cameraSize = 3f;
    [SerializeField] [Range(0f, 1f)] private float zoomDuration = 0.5f;
    private float _currentTime = 0f;
    [SerializeField] private AnimationCurve zoomInCurve;
    [SerializeField] private AnimationCurve zoomOutCurve;

    [Header("Shake")]
    private float shakeIntensity = 0.01f;
    private float shakeDuration = 0.05f;

    [Header("Vignette")]
    [SerializeField] [Range(0f, 1f)] private float vignetteIntensity = 0.5f;

    [Header("Chromatic aberration")]
    [SerializeField] [Range(0f, 1f)] private float chromaticAberrationIntensity = 0.5f;

    [Header("Color adjustments")]
    [SerializeField] private float colorAdjustmentsSaturation = 50f;

    // Original parameter values
    private float originalCameraSize;
    private float originalVignetteIntensity;
    private float originalChromaticAberrationIntensity;
    private float originalColorAdjustmentsSaturation;



    [SerializeField] Camera secondCamera;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Get references
            // Object references
        cinemachineCamera = FindObjectOfType<CinemachineVirtualCamera>();
        volume = FindObjectOfType<Volume>();

            // Individual post-processing effect references
                // Vignette
        Vignette v;
        if (volume.profile.TryGet<Vignette>(out v))
            vignette = v;
                // Chromatic aberration
        ChromaticAberration ca;
        if (volume.profile.TryGet<ChromaticAberration>(out ca))
            chromaticAberration = ca;
                // Color adjustments
        ColorAdjustments cad;
        if (volume.profile.TryGet<ColorAdjustments>(out cad))
            colorAdjustments = cad;

        // Save up some of the original parameters of these components
        originalCameraSize = cinemachineCamera.m_Lens.OrthographicSize; // Camera size
        originalVignetteIntensity = vignette.intensity.value;
        originalChromaticAberrationIntensity = chromaticAberration.intensity.value;
        originalColorAdjustmentsSaturation = colorAdjustments.saturation.value;
    }

    private void Update()
    {
        /// FOR DEBUG /// FOR DEBUG /// FOR DEBUG /// FOR DEBUG /// FOR DEBUG /// FOR DEBUG /// FOR DEBUG ///
        //if (Input.GetKeyDown(KeyCode.Space) && !_play && !_recovery)
        //    Play(3f, 0.5f, 0.01f, 0.05f);
        /// FOR DEBUG /// FOR DEBUG /// FOR DEBUG /// FOR DEBUG /// FOR DEBUG /// FOR DEBUG /// FOR DEBUG ///
        
        // Using a bool we can expect just one "Play" call to trigger the effect
        if (play)
            Effect();

        // Same thing with the "recovery" effect of the effect itself
        if (recovery)
            Recovery();
    }



    public void Play(float _cameraSize, float _effectDuration, float _shakeIntensity, float _shakeDuration)
    {
        // Set effect parameters as arguments
        cameraSize = _cameraSize;
        effectDuration = _effectDuration;
        shakeIntensity = _shakeIntensity;
        shakeDuration = _shakeDuration;
        shakeChannel = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();


        play = true; // Sets the variable to true
    }
    public void Play(ActionEffectParameters parameters)
    {
        // Set effect parameters as arguments
        cameraSize = parameters.cameraSize;
        effectDuration = parameters.effectDuration;
        shakeIntensity = parameters.shakeIntensity;
        shakeDuration = parameters.shakeDuration;

        play = true; // Sets the variable to true
    }
    private void Effect()
    {
        _currentTime += zoomDuration * Time.deltaTime; // A variable that adds over time
        
        #region Zoom
        cinemachineCamera.m_Lens.OrthographicSize = Mathf.Lerp(originalCameraSize, cameraSize, zoomInCurve.Evaluate(_currentTime)); // Lerp the Size of the camera with an animation curve
        #endregion

        #region Vignette
        vignette.intensity.value = Mathf.Lerp(originalVignetteIntensity, vignetteIntensity, zoomInCurve.Evaluate(_currentTime));
        #endregion

        #region Chromatic aberration
        chromaticAberration.intensity.value = Mathf.Lerp(originalChromaticAberrationIntensity, chromaticAberrationIntensity, zoomInCurve.Evaluate(_currentTime));
        #endregion

        #region Color adjustments
        colorAdjustments.saturation.value = Mathf.Lerp(originalColorAdjustmentsSaturation, colorAdjustmentsSaturation, zoomInCurve.Evaluate(_currentTime));
        #endregion

        shakeChannel.m_AmplitudeGain = shakeIntensity;
        shakeChannel.m_FrequencyGain = shakeDuration;
        // Condition on the effect "play" duration (the recovery duration is not included in this time interval)
        if (_currentTime >= effectDuration)
        {
            _currentTime = 0f;
            shakeChannel.m_AmplitudeGain = 0f;
            shakeChannel.m_FrequencyGain = 1f;

            play = false;
            recovery = true;
        }
    }

    private void Recovery()
    {
        _currentTime += zoomDuration * Time.deltaTime;

        
        #region Zoom
        cinemachineCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.m_Lens.OrthographicSize, originalCameraSize, zoomOutCurve.Evaluate(_currentTime));
        #endregion

        #region Vignette
        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, originalVignetteIntensity, zoomOutCurve.Evaluate(_currentTime));
        #endregion

        #region Chromatic aberration
        chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, originalChromaticAberrationIntensity, zoomOutCurve.Evaluate(_currentTime));
        #endregion

        #region Color adjustments
        colorAdjustments.saturation.value = Mathf.Lerp(colorAdjustments.saturation.value, originalColorAdjustmentsSaturation, zoomOutCurve.Evaluate(_currentTime));
        #endregion

        if (_currentTime >= recoveryDuration)
        {
            _currentTime = 0f;
            recovery = false;
        }
    }


    public bool CheckActionEffectState()
    {
        if(play || recovery)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
