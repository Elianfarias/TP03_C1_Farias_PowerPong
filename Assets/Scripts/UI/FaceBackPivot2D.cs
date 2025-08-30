using UnityEngine;

public class FaceBackPivot2D : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;     // arrastrá el RB de la pelota (o se toma del padre)
    [SerializeField] float smooth = 0f;  // 0 = instantáneo, >0 = suavizado opcional
    [SerializeField] bool spriteMiraUp = false; // marca true si tu arte “mira” +Y en reposo

    private void Awake()
    {
        if (!rb) rb = GetComponentInParent<Rigidbody2D>();
    }

    private void LateUpdate() // después de la física (rebotes ya aplicados)
    {
        Vector2 v = rb.velocity;
        if (v.sqrMagnitude < 1e-6f) return;

        Vector2 back = -v.normalized; // “atrás” = opuesto a la velocidad

        if (smooth <= 0f)
        {
            if (spriteMiraUp) transform.up = back;   // si tu sprite apunta con +Y
            else transform.right = back; // si apunta con +X (lo usual)
        }
        else
        {
            // suavizado exponencial
            Vector3 cur = spriteMiraUp ? (Vector3)transform.up : (Vector3)transform.right;
            Vector3 tgt = new Vector3(back.x, back.y, 0f);
            Vector3 lerp = Vector3.Lerp(cur, tgt, 1f - Mathf.Exp(-smooth * Time.deltaTime));
            if (spriteMiraUp) transform.up = lerp;
            else transform.right = lerp;
        }
    }
}
