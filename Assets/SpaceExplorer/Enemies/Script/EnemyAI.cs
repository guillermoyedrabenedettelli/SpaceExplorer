using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform PatrolPoint; // El objetivo a llegar de patruya
    private Transform target;    // El objetivo a seguir
    public float speed = 100f;    // La velocidad a la que se moverá el enemigo
    public float detectRadius = 10f; // El radio de detección para buscar enemigos o jugadores
    public float additionalDetectDistance = 10f; // Distancia adicional para la detección
    public float rotationSpeed = 5f; // Velocidad de rotacion


    private bool isTarget = false;

    //

    //Instanciar Proyectil
    [SerializeField] float cadense = 5f;
    public GameObject proyectil; // Prefab del proyectil
    public Transform puntoDisparo; // Punto de origen del proyectil
    public float fuerzaDisparo = 1000.0f; // Fuerza de disparo
    public float lifeTime = 5f; // Tiempo de destruccion de misil
    public Vector3 direccionDisparo = Vector3.forward; // Dirección de disparo

    public float shootDistance = 50.0f; // Distancia máxima para avanzar
    float timeSinceLastShot = 1f;
    [Header("Distancia segura")]
    [SerializeField] float safeShoot = 20f;
    //


    void Update()
    {
        Vector3 detectPosition = transform.position + transform.forward * additionalDetectDistance;
        // Detección de enemigos o jugadores
        Collider[] detectedColliders = Physics.OverlapSphere(detectPosition, detectRadius);
        foreach (Collider collider in detectedColliders)
        {
            // Verificar si el objeto detectado es un enemigo o un jugador
            if (collider.GetComponent<AlingIA>()|| collider.CompareTag("Player") || collider.CompareTag("Aling"))
            {
                // Pasar el objeto detectado al código que lo maneja
                // Esto podría ser una función separada que toma el objeto detectado como un parámetro
                isTarget = true;
                TargetEncontrado(collider.gameObject);
            }
        }
        if (!isTarget)
        {
            deteccionObstaculo(PatrolPoint);
        }
        if (isTarget)
        {
            if (target != null)
            {
                deteccionObstaculo(target);
                float distancia = Vector3.Distance(transform.position, target.transform.position);
                if (distancia >= detectRadius) target = null;
            }
            else
            {
                isTarget = false;
            }
        }

        
    }

    // Función para hacer algo con el objeto detectado
    void TargetEncontrado(GameObject obj)
    {
        if (obj == null) return;
        target = obj.transform;
        //print("EncontradoPlayer");


        // Obtenemos el centro del Mesh o del Collider del objetivo
        Vector3 targetCenter = GetTargetCenter(obj.transform);

        // Calculamos la dirección en la que apuntar
        Vector3 targetDirection = targetCenter - transform.position;

        // Calculamos la rotación para apuntar hacia el objetivo
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Avanzar hacia el target si la distancia es mayor a distanciaMaxima
        float distancia = Vector3.Distance(transform.position, obj.transform.position); 

        // Aplicamos la rotación gradualmente Lerp
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        if (distancia <= shootDistance)
        {

            if ((Time.time - timeSinceLastShot) > (1f / cadense))
            {

                //Disparar continuamente
                timeSinceLastShot = Time.time;
                Shoot();
            }
        }
    }


    private Vector3 GetTargetCenter(Transform target)
    {
        Vector3 targetCenter;

        // Si el objetivo tiene un Renderer, usamos su centro
        Renderer targetRenderer = target.GetComponent<Renderer>();
        if (targetRenderer != null)
        {
            targetCenter = targetRenderer.bounds.center;
        }
        // Si no tiene Renderer, usamos el centro de su Collider
        else
        {
            Collider targetCollider = target.GetComponent<Collider>();
            if (targetCollider != null)
            {
                targetCenter = targetCollider.bounds.center;
            }
            // Si tampoco tiene Collider, usamos su posición
            else
            {
                targetCenter = target.position;
            }
        }

        return targetCenter;
    }

    void deteccionObstaculo(Transform obj)
    {
        // Obtener la dirección hacia el objetivo
        Vector3 direction = obj.position - transform.position;
        direction.Normalize();

        // Comprobar si hay obstáculos en el camino
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit))
        {
            // Si hay un obstáculo, calcular una dirección alternativa
            Vector3 obstacleAvoidanceDirection = Vector3.zero;

            // Calcular varias direcciones alternativas
            for (int i = -45; i <= 45; i += 10)
            {
                Vector3 directionToCheck = Quaternion.AngleAxis(i, transform.up) * direction;
                if (!Physics.Raycast(transform.position, directionToCheck, out hit))
                {
                    obstacleAvoidanceDirection = directionToCheck;
                    break;
                }
            }

            // Si no hay direcciones alternativas, no hacer nada
            if (obstacleAvoidanceDirection == Vector3.zero)
            {
                return;
            }

            // Usar la dirección alternativa para evitar el obstáculo
            direction = obstacleAvoidanceDirection;
        }

        // Rotar al enemigo hacia el objetivo
        transform.LookAt(target);
        // Calcular la rotación necesaria para mirar hacia el target
        Vector3 direccion = obj.transform.position - transform.position;
        Quaternion rotacion = Quaternion.LookRotation(direccion);
        // Rotar el objeto hacia el target
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, rotationSpeed * Time.deltaTime);
        // Mover al enemigo en la dirección del objetivo
        if (Vector3.Distance(transform.position, obj.position) > safeShoot)
        {
            transform.position += direction * speed * Time.deltaTime;
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


