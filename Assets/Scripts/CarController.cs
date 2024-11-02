using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(Rigidbody) )]
public class CarController : MonoBehaviour
{
    public float acceleration = 50; // velocidad a la que aceleramos si nos movemos
    public float MaxSpeed = 15;


    // --> private Vector3 currentSpeed; // (en dirección movimiento)
    private float movement; // indicará si debemos movernos, para iniciar acción en FixedUpdate 
    private Rigidbody rb;



    // Start is called before the first frame update
    void Start()
    {
        // --> currentSpeed = new Vector3 (0, 0, 0); ????
        movement = 0;
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        // A REVISAR: Prueba de ver si avanzamos o retrocedemos
        if (Input.GetKey(KeyCode.Z) )
           movement = 1; // avanzar
        
        else if (Input.GetKey(KeyCode.X) )
            movement = -1; // retroceder
        
        else
            movement = 0; // nada


        //--> transform.position += currentSpeed * Time.deltaTime;
    }

    // ¡¡Para las físicas!!
    void FixedUpdate(){

        if (movement != 0)
            Move (movement);

    }

    
    // Mueve coche adelante o atrás en función del valor value [-1, 1] un máximo de acceleration
    void Move (float value){

        float currentSpeed = Vector3.Dot(transform.forward, rb.velocity);
        float addedSpeed = acceleration * value;
        float finalSpeed = currentSpeed + addedSpeed;

        if (finalSpeed >= -MaxSpeed && finalSpeed <= MaxSpeed) // Si no vamos a acelerar más del tope, aplica la fuerza correspondiente
           rb.AddForce (transform.forward * addedSpeed, ForceMode.Acceleration);

        // En caso contrario, en función de su signo, capamos aumento al máximo negativo o positivo
        else if (finalSpeed < 0)
            rb.AddForce (transform.forward * - (currentSpeed + MaxSpeed), ForceMode.Acceleration);
        
        else
            rb.AddForce (transform.forward * (MaxSpeed - currentSpeed), ForceMode.Acceleration);

        //currentSpeed += transform.forward * acceleration * value * Time.deltaTime;

        // OJO --> currentSpeed = Vector3.ClampMagnitude (currentSpeed, MaxSpeed); // capar velocidad máxima, para que no vaya más allá de MaxSpeed
    }
}
