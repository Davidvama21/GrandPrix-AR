using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    // Propiedades para considerar en el script CarController
    public bool forwardSteerable; // indica que la rueda puede girar si vamos hacia adelante
    public bool motorized;

    [HideInInspector] public WheelCollider WheelCollider; // para facilitar acceso en el CarController
    
    // Start is called before the first frame update
    void Start()
    {
        WheelCollider = GetComponent<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
