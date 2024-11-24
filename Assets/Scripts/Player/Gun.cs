using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //line renderer debuggin
    public LineRenderer lineRenderer;

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 1000, Color.green);
        /*
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.forward * 1000);
        */    
     }
}
