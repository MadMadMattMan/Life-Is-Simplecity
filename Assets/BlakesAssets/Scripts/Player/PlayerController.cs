using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    public float MoveSpeed = 10f, RotationSpeed = 5f;
    private float yRotation = 0f;
    private float xRotation = 0f;
    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }
    public void Move(Vector2 dir)
    {
        Vector3 move = transform.forward * dir.y + transform.right * dir.x;
        move = move * MoveSpeed * Time.deltaTime;
        Vector3 moveDirection = new Vector3(move.x, 0f, move.z);
        cc.Move(moveDirection);
    }
    public void Rotate(Vector2 dir)
    {
        yRotation += dir.x * RotationSpeed * Time.deltaTime;
        xRotation -= dir.y * RotationSpeed * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
