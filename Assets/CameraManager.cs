using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * "E" and "D" cycle camera position
 * "Escape" brings camera back to original view
 * "F" makes the planets Faster
 * "S" makes the planets Slower
 * "R" makes the planet the camera is on bigger
 * "W" makes the planet the camera is on smaller
 */


public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject mainCam;
    [SerializeField] List<GameObject> CameraLocations;
    public static float speedMult = 1;
    private int location = 0;
    private GameObject currentPlanet;
    private GameObject[] correctOrder;
    private float[] distances;
    private bool orderingBySize;

    // Start is called before the first frame update
    void Start()
    {
        setCam();
        correctOrder = new GameObject[8];
        distances = new float[8];
        for (int i = 1; i < CameraLocations.Count; i++)
        {
            correctOrder[i - 1] = CameraLocations[i].transform.parent.gameObject;
            distances[i - 1] = correctOrder[i - 1].transform.position.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            location++;
            if (location >= CameraLocations.Count)
                location = 0;
            setCam();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            location--;
            if (location < 0)
                location = CameraLocations.Count - 1;
            setCam();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            location = 0;
            setCam();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (speedMult > 5f || speedMult < -5f)
                speedMult += 1f;
            else if (speedMult > 1 || speedMult < -1f)
                speedMult += 0.5f;
            else
                speedMult += 0.25f;
            speedMult = Mathf.Clamp(speedMult, -10f, 10f);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (speedMult > 5f || speedMult < -5f)
                speedMult -= 1f;
            else if (speedMult > 1 || speedMult < -1f)
                speedMult -= 0.5f;
            else
                speedMult -= 0.25f;
            speedMult = Mathf.Clamp(speedMult, -10f, 10f);
        }

        if (location != 0)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                float nxtSize = currentPlanet.transform.localScale.x + 0.5f;
                nxtSize = Mathf.Clamp(nxtSize, 0.5f, 50f);
                currentPlanet.transform.localScale = new Vector3(nxtSize, nxtSize, nxtSize);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                float nxtSize = currentPlanet.transform.localScale.x - 0.5f;
                nxtSize = Mathf.Clamp(nxtSize, 0.5f, 50f);
                currentPlanet.transform.localScale = new Vector3(nxtSize, nxtSize, nxtSize);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            orderingBySize = !orderingBySize;
            if (orderingBySize)
            {
                orderBySize();
            }
            else
            {
                orderByOrder(correctOrder);
            }
        }
    }
    
    void setCam()
    {
        currentPlanet = CameraLocations[location];
        Transform tf = currentPlanet.transform;
        mainCam.transform.position = tf.position;
        mainCam.transform.rotation = tf.rotation;
        mainCam.transform.parent = tf;
        if(location != 0)
            currentPlanet = tf.parent.gameObject;
    }

    void orderBySize()
    {
        GameObject[] newOrder = new GameObject[8];
        for(int i = 0; i < correctOrder.Length; i++)
        {
            newOrder[i] = correctOrder[i];
        }

        for(int i = 1; i < newOrder.Length; i++)
        {
            for(int j = i; j > 0; j--)
            {
                if(newOrder[j].transform.localScale.x > newOrder[j - 1].transform.localScale.x)
                {
                    GameObject tmp = newOrder[j - 1];
                    newOrder[j - 1] = newOrder[j];
                    newOrder[j] = tmp;
                }
            }
        }

        orderByOrder(newOrder);
    }

    void orderByOrder(GameObject[] order)
    {
        for(int i = 0; i < order.Length; i++)
        {
            order[i].transform.position = new Vector3(distances[i], 0, 0);
            order[i].transform.parent.gameObject.SendMessage("resetOrbitalSpeed");
        }
    }
}
