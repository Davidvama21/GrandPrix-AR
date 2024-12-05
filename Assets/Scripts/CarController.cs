using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(Rigidbody) )]
public class CarController : MonoBehaviour
{
    //public float acceleration = 50; // velocidad a la que aceleramos si nos movemos
   
    public float brakeTorque = 2000; // para frenar
    public float brakeScale = 0.5f;  // nivel de frenado si no hay input (>= 0!)
    
    public float motorTorque = 2000;
    public float maxSpeed = 15;
    public float turnAngle = 30; // parámetro que indica máximo giro
    public float turnAngleAtMaxSpeed = 10;

    public float centreOfGravityOffset = -1f;


    // --> private Vector3 currentSpeed; // (en dirección movimiento)
    private float movement; // indicará si debemos movernos, para iniciar acción en FixedUpdate
    private float turning; // indicará si debemos girar, para hacer acción tambien en FixedUpdate 
    private Rigidbody rb;
    WheelController[] wheels; // para ref. WheelColliders a los que aplicamos movimiento (+ propiedades script para tomar decisiones)
    

    // Start is called before the first frame update
    void Start()
    {
        movement = turning = 0;
        rb = GetComponent<Rigidbody>(); // ??
        wheels = GetComponentsInChildren<WheelController>();

        // Ajustar centro de masa verticalmente, para evitar que el coche ruede
        rb.centerOfMass += Vector3.up * centreOfGravityOffset;
    }

    // Update is called once per frame
    void Update()
    {
        // A REVISAR: Prueba de ver si avanzamos o retrocedemos
        /*
        if (Input.GetKey(KeyCode.Z) )
           movement = 1; // avanzar
        
        else if (Input.GetKey(KeyCode.X) )
            movement = -1; // retroceder
        
        else
            movement = 0; // nada

        turning = Input.GetAxis("Horizontal");
        */        
    }

    // ¡¡Para las físicas!!
    void FixedUpdate(){

        float currentSpeed = Vector3.Dot(transform.forward, rb.velocity); // velocidad hacia adelante actual

        // Da cuán de cerca estamos del máximo, como valor entre 0 y 1
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, currentSpeed);

        // Use that to calculate how much torque is available 
        // (zero torque at top speed)
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

        // …and to calculate how much to steer 
        // (the car steers more gently at top speed)
        float currentTurnRange = Mathf.Lerp(turnAngle, turnAngleAtMaxSpeed, speedFactor);

        bool isAccelerating = (movement != 0f) && (Mathf.Sign(movement) == Mathf.Sign(currentSpeed) ); // si movimiento va a favor de la velocidad (!= 0f porque el sign coge 0 como positivo)

        // Vamos a tratar el giro de las ruedas de manera diferente en función del sentido de desplazamiento; las que marcamos como forwardSteerable girarán
        // si nos movemos hacia adelante (>= 0), y las otras en el caso de movernos hacia atrás.
        // En cualquiera de los dos casos, trataremos igual la aceleración de las ruedas.

        if (currentSpeed >= 0){ // hacia adelante
            
            foreach (var wheel in wheels){

                if (isAccelerating){

                    if (wheel.motorized)
                        Move (movement, wheel, currentMotorTorque);
                    
                    wheel.WheelCollider.brakeTorque = 0; // necesario por si partimos de estado de frenado

                }else if (movement != 0f){
                    Brake ( Mathf.Abs(movement), wheel);

                }else {
                    Brake (brakeScale, wheel);
                }


                if (wheel.forwardSteerable){
                    Turn (turning, wheel, currentTurnRange);
                }else
                    wheel.WheelCollider.steerAngle = 0; // por si venimos de ir hacia atrás, para que esta rueda no interfiera con el desplazamiento
            }

        }else{
            
            foreach (var wheel in wheels){

                if (isAccelerating){

                    if (wheel.motorized)
                        Move (movement, wheel, currentMotorTorque);
                    
                    wheel.WheelCollider.brakeTorque = 0; // necesario por si partimos de estado de frenado

                }else if (movement != 0f){
                    Brake ( Mathf.Abs(movement), wheel);

                }else {
                    Brake (brakeScale, wheel);
                }


                if (!wheel.forwardSteerable){
                    Turn (turning, wheel, currentTurnRange);
                }else
                    wheel.WheelCollider.steerAngle = 0; // por si venimos de ir hacia adelante, para que esta rueda no interfiera con el desplazamiento
            }
        }

    }

    
    // Mueve rueda coche adelante o atrás en función del valor value [-1, 1] un máximo de acceleration y hasta una velocidad MaxSpeed
    void Move (float value, WheelController wheel, float currentMotorTorque){

        wheel.WheelCollider.motorTorque = value * currentMotorTorque;


        /*
        float addedSpeed = acceleration * value;
        float finalSpeed = currentSpeed + addedSpeed;

        if (finalSpeed >= -MaxSpeed && finalSpeed <= MaxSpeed) // Si no vamos a acelerar más del tope, aplica la fuerza correspondiente
           rb.AddForce (transform.forward * addedSpeed, ForceMode.Acceleration);

        // En caso contrario, en función de su signo, capamos aumento al máximo negativo o positivo
        else if (finalSpeed < 0)
            rb.AddForce (transform.forward * - (currentSpeed + MaxSpeed), ForceMode.Acceleration);
        
        else
            rb.AddForce (transform.forward * (MaxSpeed - currentSpeed), ForceMode.Acceleration);

        */

        //currentSpeed += transform.forward * acceleration * value * Time.deltaTime;
        

        // OJO --> currentSpeed = Vector3.ClampMagnitude (currentSpeed, MaxSpeed); // capar velocidad máxima, para que no vaya más allá de MaxSpeed
    }

    // Hace que la rueda frene, el grado que indique value (¡debe ser positivo!)
    void Brake (float value, WheelController wheel) {

        wheel.WheelCollider.brakeTorque = value * brakeTorque;
        wheel.WheelCollider.motorTorque = 0; // para que no acelere a la vez
    }

    // Hace girar rueda del coche en función del valor value [-1, 1] y otros parámetros
    void Turn (float value, WheelController wheel, float currentTurnRange){

        wheel.WheelCollider.steerAngle = value * currentTurnRange;
        //float currentSpeed = Vector3.Dot(transform.forward, rb.velocity);

        //rb.AddTorque (Vector3.up * value * currentSpeed * steerAngle); // giramos en función de magnitud de avance y parámetro steerAngle

    }

    // Funciones para modificar movement, turning desde fuera //
    public void setMovement (float value){
        movement = value;
    }

    public void setTurning (float value){
        turning = value;
    }
}
