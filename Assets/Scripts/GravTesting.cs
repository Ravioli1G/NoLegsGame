using UnityEngine;

public class GravTesting : MonoBehaviour
{
    public float x = 0f;
    public float y = -9.81f;
    public float z = 0f;

    // Update is called once per frame
    void Update()
    {
        Physics.gravity = new Vector3(x, y, z);
    }
}
