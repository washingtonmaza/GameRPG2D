using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public float damTime = 0.15f; // valor da suavidade da camera
    private Vector3 velocity = Vector3.zero; // velocidade  
    public Transform target; //ponto personagem que vai ser seguido
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
        Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        Vector3 destination = transform.position+delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, damTime);

    }
}
 