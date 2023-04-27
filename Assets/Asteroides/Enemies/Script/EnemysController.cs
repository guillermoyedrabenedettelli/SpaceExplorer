using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysController : MonoBehaviour
{
    [SerializeField]
    public string NameTarget = "";
    //Deteccion del objetivo por nombre
    private GameObject Target;

    // Configuraci�n de velocidades de nave 
    public float velocidadAvance = 5.0f; // Velocidad de movimiento
    public float velocidadRotacion = 5.0f; // Velocidad de rotaci�n
    public float distanciaMaxima = 5.0f; // Distancia m�xima para avanzar

    //Instanciar Proyectil
    [SerializeField] float cadense = 5f;
    public GameObject proyectil; // Prefab del proyectil
    public Transform puntoDisparo; // Punto de origen del proyectil
    public float fuerzaDisparo = 1000.0f; // Fuerza de disparo
    public float lifeTime = 5f; // Tiempo de destruccion de misil
    public Vector3 direccionDisparo = Vector3.forward; // Direcci�n de disparo
    float timeSinceLastShot = 0f;
    public KeyCode teclaDisparo = KeyCode.Space; // Tecla para disparar



    void Start()
    {
        GameObject objetoBuscado = GameObject.Find(NameTarget);
        if (objetoBuscado != null)
        {
            // El objeto fue encontrado, haz algo con �l
            Target = objetoBuscado;
        }
        else
        {
            // El objeto no fue encontrado
            Debug.Log("No se encontr� ning�n objeto con el nombre especificado.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Calcular la rotaci�n necesaria para mirar hacia el target
        Vector3 direccion = Target.transform.position - transform.position;
        Quaternion rotacion = Quaternion.LookRotation(direccion);

        // Rotar el objeto hacia el target
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, velocidadRotacion * Time.deltaTime);

        // Avanzar hacia el target si la distancia es mayor a distanciaMaxima
        float distancia = Vector3.Distance(transform.position, Target.transform.position);
        //Debug.Log(distancia);
        if (distancia >= distanciaMaxima)
        {
            transform.Translate(Vector3.forward * velocidadAvance * Time.deltaTime);
            if (distanciaMaxima == 0)
            {
                if (distancia < 15)
                {
                    if ((Time.time - timeSinceLastShot) > (1f / cadense))
                    {

                        //Disparar continuamente
                        timeSinceLastShot = Time.time;
                        Shoot();
                    }
                }

            }
        }
        else
        {
            if (distancia <= distanciaMaxima || Input.GetKeyDown(teclaDisparo))
            {
                if ((Time.time - timeSinceLastShot) > (1f / cadense))
                {

                    //Disparar continuamente
                    timeSinceLastShot = Time.time;
                    Shoot();
                }
            }

        }


    }
    private void Shoot()
    {
        GameObject nuevoProyectil = Instantiate(proyectil, puntoDisparo.position, puntoDisparo.rotation);
        Rigidbody rigidbodyProyectil = nuevoProyectil.GetComponent<Rigidbody>();
        if (rigidbodyProyectil)
        {
            rigidbodyProyectil.velocity = nuevoProyectil.transform.forward * fuerzaDisparo;
            Destroy(nuevoProyectil, lifeTime);
        }

    }
}
