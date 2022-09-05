using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField] bool isOrbital;
    float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if (isOrbital)
        {
            resetOrbitalSpeed();
        }
        else
        {
            rotateSpeed = 0.05f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3 (0, rotateSpeed * CameraManager.speedMult, 0));
    }

    // Sets orbital speed based off distance to orbital object

    // Might consider putting in line breaks for readability
    void resetOrbitalSpeed() { 
        rotateSpeed = Mathf.Sqrt(transform.parent.transform.localScale.x / 
            transform.GetChild(0).transform.position.x / 100); }
}
