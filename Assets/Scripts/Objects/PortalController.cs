using UnityEngine;

public class PortalController : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("PortalController: OnTriggerEnter called with " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the portal!");
            FindObjectOfType<GameLevelManager>().OnPlayerEnterPortal();
        }
    }
}
