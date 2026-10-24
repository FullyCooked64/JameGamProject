using UnityEngine;

public class PhysicsWatcher : MonoBehaviour
{
    private Rigidbody testBody;

    void Start()
    {
        Physics.simulationMode = SimulationMode.Script;

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0, 10, 0);

        Rigidbody rb = cube.AddComponent<Rigidbody>();
        rb.useGravity = true;

        for (int i = 0; i < 100; i++)
        {
            Physics.Simulate(0.02f);
        }

        Debug.Log("Cube height after manual simulation: " + rb.position.y);
    }

    void FixedUpdate()
    {
        if (testBody != null)
        {
            Debug.Log(
                "Height: " + testBody.position.y +
                " | Velocity: " + testBody.linearVelocity.y
            );
        }
    }
}
