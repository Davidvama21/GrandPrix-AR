using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{
    public int totalLaps = 3; // vueltas a dar por el circuito
    
    public GameObject carPrefab;
    public Vector3 startingCarPosition; // posición (relativa al track) y
    public Vector3 startingCarRotation; // rotación del coche carPrefab al empezar


    // Start is called before the first frame update
    void Start()
    {
        Instantiate (carPrefab, transform.position + startingCarPosition, Quaternion.Euler(startingCarRotation), transform); // crea el coche COMO HIJO DEL TRACK con posición relativa al track y su rotación
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
