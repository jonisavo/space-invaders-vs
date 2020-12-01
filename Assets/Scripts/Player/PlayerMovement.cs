using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 10;
    public float RotateSpeed = 250;

    private void Update()
    {
        transform.Translate(
            Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime,
            0,
            Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime);

        transform.Rotate(
            new Vector3(0, Input.GetAxis("Mouse X") * RotateSpeed * Time.deltaTime, 0));
    }
}
