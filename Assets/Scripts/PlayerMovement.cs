using Assets.Scripts.Enums;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] PlayerSettings playerSettings;
    [SerializeField] private AxisEnum axisRotation = AxisEnum.Z;

    [Header("Keys Movement Configuration")]
    [SerializeField] private KeyCode keyUp = KeyCode.W;
    [SerializeField] private KeyCode keyLeft = KeyCode.A;
    [SerializeField] private KeyCode keyDown = KeyCode.S;
    [SerializeField] private KeyCode keyRight = KeyCode.D;

    [Header("Keys Rotation Configuration")]
    [SerializeField] private KeyCode rotationLeft = KeyCode.Q;
    [SerializeField] private KeyCode rotationRight = KeyCode.E;

    [Header("Keys Rotation Configuration")]
    [SerializeField] private KeyCode colorChange = KeyCode.R;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!UIMainMenu.Instance.isPause)
        {
            PlayerRotation();
            ChangeRandomColor();
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        if (Input.GetKey(keyUp))
            MoveAxisY(playerSettings.SpeedMovement);
        else if (Input.GetKey(keyDown))
            MoveAxisY(-playerSettings.SpeedMovement);
        else if (Input.GetKey(keyLeft))
            MoveAxisX(-playerSettings.SpeedMovement);
        else if (Input.GetKey(keyRight))
            MoveAxisX(playerSettings.SpeedMovement);
    }

    private void MoveAxisY(float speed)
    {
        transform.position = transform.position + new Vector3(0, speed, 0);
    }

    private void MoveAxisX(float speed)
    {
        transform.position = transform.position + new Vector3(speed, 0, 0);
    }

    private void PlayerRotation()
    {
        if (Input.GetKeyDown(rotationRight))
            Rotate(-playerSettings.SpeedRotation);
        else if (Input.GetKeyDown(rotationLeft))
            Rotate(playerSettings.SpeedRotation);
    }

    private void ChangeRandomColor()
    {
        if (Input.GetKeyUp(colorChange))
            spriteRenderer.color = new Color(Random.value, Random.value, Random.value, 1f);
    }

    private void Rotate(int eulerRotation)
    {


        switch (axisRotation)
        {
            case AxisEnum.X:
                transform.Rotate(eulerRotation, 0, 0);
                break;
            case AxisEnum.Y:
                transform.Rotate(0, eulerRotation, 0);
                break;
            case AxisEnum.Z:
                transform.Rotate(0, 0, eulerRotation);
                break;
        }
    }
}
