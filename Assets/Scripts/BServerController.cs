using System.Collections;
using System.Collections.Generic;
using UnityAndroidBluetooth;
using UnityEngine;

[RequireComponent( typeof(CarController) )]
public class BServerController : MonoBehaviour
{
    public string clientAddress; // dirección Bluetooth del cliente

    private CarController car; // para tocar inputs
    BluetoothServer server; // para comunicación

    // Start is called before the first frame update
    void Start()
    {
        car = GetComponent<CarController>();

        // inicio comunicación //
        server = new BluetoothServer();
        server.Start();

        server.MessageReceived += loadInputs; // asignamos función al evento de recepción de mensaje para poder tratarlo
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnApplicationQuit(){

        // Paramos el servidor por si acaso
        server.Stop();
    }

    // Funciones que responden a evento MessageReceived //
    private void loadInputs (object sender, MessageReceivedEventArgs args){

        // Posiblemente se pueda hacer más eficiente...
        string[] numbersAsString = args.Message.Split(' ');
        car.setMovement(float.Parse (numbersAsString[0]) );
        car.setTurning(float.Parse (numbersAsString[1]) );
    }  
}
