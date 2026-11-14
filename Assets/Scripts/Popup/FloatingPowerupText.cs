using TMPro;
using UnityEngine;

public class FloatingPowerupText : MonoBehaviour
{
    [SerializeField] private float targetHeight = 1f;
    [SerializeField] private TextMeshProUGUI text;
    
    void SetText(string message)
    {
        text.text = message;
    }

    // Update is called once per frame
    void ShowAnimation()
    {

        System.Collections.IEnumerator AnimateText()
        {
            Vector3 startPos = transform.position;
            Vector3 targetPos = startPos + new Vector3(0f, targetHeight, 0f);
            float duration = 1f; // Duration of the animation in seconds
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                transform.position = Vector3.Lerp(startPos, targetPos, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the final position is set
            transform.position = targetPos;

            // Destroy the text object after the animation is complete
            Destroy(gameObject);
        }

        Vector3 currentPos = transform.position;
        float targetPos = currentPos.y+targetHeight;
        

    }
}
