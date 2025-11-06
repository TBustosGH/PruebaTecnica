using UnityEngine;

public class interactBoxController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("interactObject"))
        {
            Debug.Log("I´m interacting.");
        }
    }
}
