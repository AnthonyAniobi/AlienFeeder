using UnityEngine;

public class PortalController : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<GameLevelManager>().OnPlayerEnterPortal();
        }
    }
}
