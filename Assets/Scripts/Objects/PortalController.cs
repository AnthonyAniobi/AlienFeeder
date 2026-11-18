using UnityEngine;

public class PortalController : MonoBehaviour
{

    void Update()
    {
        transform.Rotate()
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<GameLevelManager>().OnPlayerEnterPortal();
        }
    }
}
