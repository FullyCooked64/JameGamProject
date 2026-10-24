using UnityEngine;

public class ParticlesStickObject : MonoBehaviour
{
    public Transform objectToStick;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = objectToStick.transform.position;
        transform.rotation = objectToStick.transform.rotation;
    }
}
