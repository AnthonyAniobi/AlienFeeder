using TMPro;
using UnityEngine;

public class FloatingPowerupText : MonoBehaviour
{
    [SerializeField] private float targetHeight = 1f;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CanvasGroup canvasGroup;
    
    public void SetText(string message)
    {
        text.text = message;
        // reset the position and scale of the text
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        transform.localScale = Vector3.one;
        canvasGroup.alpha = 1f; // Reset alpha to fully visible
    }

    // Update is called once per frame
    public void ShowAnimation()
    {

        System.Collections.IEnumerator AnimateText()
        {
            Vector3 startPos = transform.position;
            Vector3 targetPos = startPos + new Vector3(0f, targetHeight, 0f);
            Vector3 targetScale = new Vector3(1.5f, 1.5f, 1f); // Target scale for the text
            float duration = 1f; // Duration of the animation in seconds
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                transform.position = Vector3.Lerp(startPos, targetPos, t);
                transform.localScale = Vector3.Lerp(Vector3.one, targetScale, t); // Scale up the text
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, t); // Fade out the text
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the final position is set
            transform.position = targetPos;

            // Destroy the text object after the animation is complete
            Destroy(gameObject);
        }

        StartCoroutine(AnimateText());
        

    }
}
