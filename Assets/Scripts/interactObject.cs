using UnityEngine;

public class interactObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("interactBox"))
        {
            Debug.Log("I was interacted.");
        }
    }
}
