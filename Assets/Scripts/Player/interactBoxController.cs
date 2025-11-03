using UnityEngine;

public class interactBoxController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("interactObject"))
        {
            //TODO
            //other.GetComponent<interactObjectController>().
            Debug.Log("I´m interacting.");
        }
    }
}
