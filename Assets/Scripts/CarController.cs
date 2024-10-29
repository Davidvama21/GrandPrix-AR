using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float acceleration = 50; // velocidad a la que aceleramos si nos movemos
    public float MaxSpeed = 15;

    private Vector3 currentSpeed; // (en dirección movimiento)


    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = new Vector3 (0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // A REVISAR: Prueba de ver si avanzamos o retrocedemos
        if (Input.GetKey(KeyCode.Z) )
            Move (1); // avanzar
        
        else if (Input.GetKey(KeyCode.X) )
            Move (-1); // retroceder


        transform.position += currentSpeed * Time.deltaTime;
    }

    
    // Mueve coche adelante o atrás en función del valor value [-1, 1] un máximo de acceleration
    void Move (float value){
        currentSpeed += transform.forward * acceleration * value * Time.deltaTime;

        currentSpeed = Vector3.ClampMagnitude (currentSpeed, MaxSpeed); // capar velocidad máxima, para que no vaya más allá de MaxSpeed
    }
}
