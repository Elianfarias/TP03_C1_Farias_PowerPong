using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ColorPaletteController : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IInitializePotentialDragHandler
{
    [Header("Refs")]
    [SerializeField] RectTransform picker;
    [SerializeField] Image pickedColorImage;
    [SerializeField] Material colorWheelMat;          // Se instancia en Awake (no shared)

    [Header("Wheel")]
    [SerializeField] int totalNumberofColors = 24;
    [SerializeField] int wheelsCount = 2;
    [SerializeField, Range(0, 360), Tooltip("clockwise angle of the beginning point starting from +X")]
    float startingAngle = 0;
    [SerializeField, InspectorName("Control Sat & Val")] bool controlSV = false;
    [SerializeField] bool inertia = true;
    [SerializeField] float decelerationRate = 0.135f;
    [SerializeField] bool wholeSegment = false;

    [Header("Limits")]
    [SerializeField, Range(0.5f, 0.001f)] float minimumSatValStep = 0.01f;
    [SerializeField, Range(0, 1)] float minimumSaturation = 0.25f;
    [SerializeField, Range(0, 1)] float maximumSaturation = 1f;
    [SerializeField, Range(0, 1)] float minimumValue = 0.25f;
    [SerializeField, Range(0, 1)] float maximumValue = 1f;

    // dragging
    bool dragging = false;
    float satValAmount = 1f;
    float omega = 0f;               // velocidad angular (deg/s)
    float previousTheta = 0f;       // acumulador angular previo (deg)
    float theta = 0f;               // acumulador angular actual (deg)

    float previousDiscretedH;
    float sat = 1f, val = 1f;

    // estado público
    Color selectedColor;
    public Color SelectedColor
    {
        get => selectedColor;
        private set
        {
            if (value != selectedColor)
            {
                selectedColor = value;
                OnColorChange?.Invoke(SelectedColor);
            }
        }
    }

    /// <summary>Hue en [0..wheelsCount)</summary>
    public float Hue { get; private set; } = 0f;

    public float Value
    {
        get => val;
        set
        {
            float newVal = Mathf.Clamp(value, minimumValue, maximumValue);
            if (Mathf.Abs(val - newVal) > minimumSatValStep)
            {
                val = newVal;
                UpdateMaterial();
                UpdateColor();
            }
        }
    }

    public float Saturation
    {
        get => sat;
        set
        {
            float newSat = Mathf.Clamp(value, minimumSaturation, maximumSaturation);
            if (Mathf.Abs(sat - newSat) > minimumSatValStep)
            {
                sat = newSat;
                UpdateMaterial();
                UpdateColor();
            }
        }
    }

    public ColorChangeEvent OnColorChange;
    public HueChangeEvent OnHueChange;

    // --- presets/geom ---
    Vector2 centerPoint;      // en coords de pantalla
    float paletteRadius;      // en unidades UI
    float pickerHueOffset;    // [0..1] por la posición inicial del picker
    RectTransform rt;
    Camera uiCam;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        // Cámara del Canvas si no es Overlay
        var canvas = GetComponentInParent<Canvas>();
        if (canvas && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            uiCam = canvas.worldCamera;

        CalculatePresets();
        UpdateMaterialInitialValues();
        UpdateMaterial();
        UpdateColor();
    }

    void OnEnable()
    {
        rt = GetComponent<RectTransform>();
        CalculatePresets();
    }

    void OnRectTransformDimensionsChange()
    {
        rt = GetComponent<RectTransform>();
        // si cambia tamaño/posición recalculamos
        CalculatePresets();
    }

    void UpdateMaterialInitialValues()
    {
        SafeSet("_StartingAngle", startingAngle);
        SafeSetInt("_ColorsCount", totalNumberofColors);
        SafeSetInt("_WheelsCount", wheelsCount);
    }

    // ---- presets ----
    void CalculatePresets()
    {
        // centro del rect en pantalla (funciona en Overlay y Camera/World)
        Vector3 worldCenter = rt.TransformPoint(rt.rect.center);
        centerPoint = RectTransformUtility.WorldToScreenPoint(uiCam, worldCenter);

        // radio útil
        var size = rt.rect.size;
        paletteRadius = Mathf.Max(Mathf.Min(size.x, size.y) * 0.5f - 1f, 1e-4f);

        // offset inicial del picker -> hue
        if (picker)
        {
            Vector3 lp = picker.localPosition;
            float ang = Mathf.Atan2(lp.y, lp.x);      // [-π..π]
            if (ang < 0f) ang += 2f * Mathf.PI;
            pickerHueOffset = ang / (2f * Mathf.PI);  // [0..1]
        }
        else pickerHueOffset = 0f;
    }

    /// <summary>
    /// amount en [0..2]: [0..1] varía V (S=1), [1..2] varía S (V=1)
    /// </summary>
    public void CalculateSaturationAndValue(float amount)
    {
        if (amount > 1f) { val = 1f; sat = 2f - amount; }
        else { sat = 1f; val = amount; }

        sat = Mathf.Clamp(sat, minimumSaturation, maximumSaturation);
        val = Mathf.Clamp(val, minimumValue, maximumValue);
        UpdateMaterial();
        UpdateColor();
    }

    public void UpdateHue()
    {
        // hue siempre finito y dentro de [0..wheelsCount)
        if (float.IsNaN(Hue) || float.IsInfinity(Hue)) Hue = 0f;
        Hue = Mathf.Repeat(Hue, Mathf.Max(1, wheelsCount));

        UpdateMaterial();
        UpdateColor();
    }

    public void UpdateMaterial()
    {
        // Hue normalizado 0..1 (mismo criterio que UpdateColor)
        float h01 = (pickerHueOffset + startingAngle / 360f + (Hue % wheelsCount)) / wheelsCount;
        h01 = Mathf.Repeat(h01, 1f);

        if (wholeSegment)
        {
            float step = 1f / Mathf.Max(1, totalNumberofColors);
            h01 = Mathf.Floor(h01 / step) * step;
        }

        if (float.IsNaN(h01) || float.IsInfinity(h01)) h01 = 0f;

        colorWheelMat.SetFloat("_Hue", h01);

        if (controlSV)
        {
            colorWheelMat.SetFloat("_Sat", Mathf.Clamp01(sat));
            colorWheelMat.SetFloat("_Val", Mathf.Clamp01(val));
        }

        // Por si la UI usa máscaras/material modificado
        var img = GetComponent<Image>();
        if (img) img.SetMaterialDirty();
    }

    public void UpdateColor()
    {
        // shift según picker + startingAngle y ruedas
        float shiftedH = (pickerHueOffset + startingAngle / 360.0f + (Hue % wheelsCount)) / wheelsCount;
        shiftedH = shiftedH % 1.0f;

        // discretización de vista (tu lógica original)
        float discretedH = ((int)(shiftedH * totalNumberofColors)) / (1.0f * (totalNumberofColors - 1));

        Color color;
        if (shiftedH > 1 - 1.0f / totalNumberofColors && shiftedH <= 1)
            color = Color.HSVToRGB(0, 0, (val - sat + 0.75f) / 1.5f);    // gris
        else
            color = Color.HSVToRGB(discretedH, sat, val);

        if (previousDiscretedH != discretedH)
            OnHueChange?.Invoke(discretedH);

        if (pickedColorImage) pickedColorImage.color = color;
        SelectedColor = color;
        previousDiscretedH = discretedH;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging || eventData.button != PointerEventData.InputButton.Left) return;

        Vector2 curr = eventData.position;
        Vector2 prev = curr - eventData.delta;

        // Control S/V radial (si está activo)
        if (controlSV)
        {
            float r1 = Vector2.Distance(centerPoint, prev);
            float r2 = Vector2.Distance(centerPoint, curr);
            float dr = r2 - r1;
            satValAmount = Mathf.Clamp(satValAmount + dr / Mathf.Max(paletteRadius, 1e-4f), 0f, 2f);
            CalculateSaturationAndValue(satValAmount);
        }

        // Hue por ángulo (protegido contra centro)
        Vector2 vPrev = prev - centerPoint;
        Vector2 vCurr = curr - centerPoint;

        float dtheta = 0f; // en grados
        if (vPrev.sqrMagnitude > 1e-6f && vCurr.sqrMagnitude > 1e-6f)
        {
            dtheta = Vector2.SignedAngle(vCurr, vPrev); // igual que tu código (sentido horario)
            theta += dtheta;
            Hue += dtheta / 360f;
        }

        UpdateHue();
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        omega = 0f;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        dragging = true;
        omega = 0f;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        dragging = false;
    }

    void Update()
    {
        float dt = Time.unscaledDeltaTime;                 // usa SIEMPRE el mismo dt
        if (dt <= 1e-6f) return;                           // evita /0

        if (dragging && inertia)
        {
            float newOmega = (theta - previousTheta) / dt; // deg/s
            if (!float.IsNaN(newOmega) && !float.IsInfinity(newOmega))
                omega = Mathf.Lerp(omega, newOmega, dt * 10f);
            previousTheta = theta;
        }

        if (!dragging && Mathf.Abs(omega) > 0f)
        {
            omega *= Mathf.Pow(decelerationRate, dt);
            if (Mathf.Abs(omega) < 1f) omega = 0f;

            float dtheta = omega * dt;        // deg
            Hue += dtheta / 360f;
            UpdateHue();
        }
    }

    // ---------- helpers ----------
    void SafeSet(string name, float v)
    {
        if (!colorWheelMat || string.IsNullOrEmpty(name)) return;
        if (float.IsNaN(v) || float.IsInfinity(v)) return;
        int id = Shader.PropertyToID(name);
        if (colorWheelMat.HasProperty(id)) colorWheelMat.SetFloat(id, v);
    }
    void SafeSetInt(string name, int v)
    {
        if (!colorWheelMat || string.IsNullOrEmpty(name)) return;
        int id = Shader.PropertyToID(name);
        if (colorWheelMat.HasProperty(id)) colorWheelMat.SetInt(id, v);
    }
}

[System.Serializable] public class ColorChangeEvent : UnityEvent<Color> { }
[System.Serializable] public class HueChangeEvent : UnityEvent<float> { }
