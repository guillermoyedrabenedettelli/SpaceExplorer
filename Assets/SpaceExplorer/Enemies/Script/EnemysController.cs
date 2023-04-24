using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysController : MonoBehaviour
{
    [SerializeField]
    public string NameTarget = "";

    public float velocidadAvance = 5.0f; // Velocidad de movimiento
    public float velocidadRotacion = 5.0f; // Velocidad de rotación
    public float distanciaMaxima = 5.0f; // Distancia máxima para avanzar

    //Instanciar Proyectil
    public GameObject proyectil; // Prefab del proyectil
    public Transform puntoDisparo; // Punto de origen del proyectil
    public float fuerzaDisparo = 1000.0f; // Fuerza de disparo
    public Vector3 direccionDisparo = Vector3.forward; // Dirección de disparo
    public KeyCode teclaDisparo = KeyCode.Space; // Tecla para disparar

    private GameObject Target;

    // Start is called before the first frame update
    void Start()
    {
        GameObject objetoBuscado = GameObject.Find(NameTarget);
        if (objetoBuscado != null)
        {
            // El objeto fue encontrado, haz algo con él
            Target = objetoBuscado;
        }
        else
        {
            // El objeto no fue encontrado
            Debug.Log("No se encontró ningún objeto con el nombre especificado.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Calcular la rotación necesaria para mirar hacia el target
        Vector3 direccion = Target.transform.position - transform.position;
        Quaternion rotacion = Quaternion.LookRotation(direccion);

        // Rotar el objeto hacia el target
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, velocidadRotacion * Time.deltaTime);

        // Avanzar hacia el target si la distancia es mayor a distanciaMaxima
        float distancia = Vector3.Distance(transform.position, Target.transform.position);
        if (distancia > distanciaMaxima)
        {
            transform.Translate(Vector3.forward * velocidadAvance * Time.deltaTime);
        }
        if (Input.GetKeyDown(teclaDisparo))
        {
            GameObject nuevoProyectil = Instantiate(proyectil, puntoDisparo.position, puntoDisparo.rotation);

            // Obtener el componente Rigidbody del proyectil
            Rigidbody rigidbodyProyectil = nuevoProyectil.GetComponent<Rigidbody>();

            // Aplicar una fuerza al proyectil en la dirección y magnitud deseadas
            rigidbodyProyectil.AddForce(direccionDisparo * fuerzaDisparo);
        }

    }
}
