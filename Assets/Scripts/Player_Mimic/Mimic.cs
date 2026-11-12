using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mimic : MonoBehaviour
{
    [Header("Animation")]
    public GameObject legPrefab;

    [Range(2, 20)]
    public int numberOfLegs = 5;
    [Tooltip("The number of splines per leg")]
    [Range(1, 10)]
    public int partsPerLeg = 4;
    int maxLegs;

    public int legCount;
    public int deployedLegs;
    [Range(0, 19)]
    public int minimumAnchoredLegs = 2;
    public int minimumAnchoredParts;

    [Tooltip("Minimum duration before leg is replaced")]
    public float minLegLifetime = 5;
    [Tooltip("Maximum duration before leg is replaced")]
    public float maxLegLifetime = 15;

    public Vector3 legPlacerOrigin = Vector3.zero;
    [Tooltip("Leg placement radius offset")]
    public float newLegRadius = 3;

    public float minLegDistance = 4.5f;
    public float maxLegDistance = 6.3f;

    [Range(2, 50)]
    [Tooltip("Number of spline samples per legpart")]
    public int legResolution = 40;

    [Tooltip("Minimum lerp coeficient for leg growth smoothing")]
    public float minGrowCoef = 4.5f;
    [Tooltip("MAximum lerp coeficient for leg growth smoothing")]
    public float maxGrowCoef = 6.5f;

    [Tooltip("Minimum duration before a new leg can be placed")]
    public float newLegCooldown = 0.3f;

    [Header("Surface and Food")]
    [SerializeField] private string groundTag = "Ground";
    [SerializeField] private string edibleTag = "Edible";
    [SerializeField] private string wallTag = "Wall";
    [SerializeField] private float edibleDetectionRadius = 1f;
    [SerializeField] private float ediblePullDuration = 0.35f;
    [SerializeField] private float wallCollisionRadius = 0.35f;
    [SerializeField] private float wallCollisionHeight = 0.8f;
    [SerializeField] private float wallCollisionPadding = 0.02f;

    [Header("Mimic Levels")]
    [SerializeField] private Vector3 childScale = Vector3.one;
    [SerializeField] private Vector3 teenScale = new Vector3(1.25f, 1.25f, 1.25f);
    [SerializeField] private Vector3 adultScale = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] private Color childColor = Color.white;
    [SerializeField] private Color teenColor = Color.yellow;
    [SerializeField] private Color adultColor = Color.red;
    [SerializeField] private Color bossColor = Color.black;
    [Tooltip("Visual body root to scale. Leave empty to scale the mimic renderers without moving camera targets.")]
    [SerializeField] private Transform levelVisualRoot;
    [SerializeField] private Material childLegMaterial;
    [SerializeField] private Material teenLegMaterial;
    [SerializeField] private Material adultLegMaterial;
    [SerializeField] private Material bossLegMaterial;

    public GameObject AttachedEdible { get; private set; }

    bool canCreateLeg = true;
    bool isEating;

    List<GameObject> availableLegPool = new List<GameObject>();
    Vitality vitality;
    Renderer[] bodyRenderers;
    Vector3 levelVisualBaseScale = Vector3.one;

    [Tooltip("This must be updates as the Mimin moves to assure great leg placement")]
    public Vector3 velocity;

    void Start()
    {
        vitality = GetComponent<Vitality>();
        bodyRenderers = GetComponentsInChildren<Renderer>();
        ResolveLevelVisualRoot();
        ResetMimic();
        if (vitality != null)
            ApplyMimicLevel(vitality.MimicLevel);
    }

    private void OnValidate()
    {
        ResetMimic();
    }

    private void ResetMimic()
    {
        foreach (Leg g in GameObject.FindObjectsOfType<Leg>())
        {
            Destroy(g.gameObject);
        }
        legCount = 0;
        deployedLegs = 0;

        maxLegs = numberOfLegs * partsPerLeg;
        float rot = 360f / maxLegs;
        Vector2 randV = Random.insideUnitCircle;
        velocity = new Vector3(randV.x, 0, randV.y);
        minimumAnchoredParts = minimumAnchoredLegs * partsPerLeg;
        maxLegDistance = newLegRadius * 2.1f;

    }

    IEnumerator NewLegCooldown()
    {
        canCreateLeg = false;
        yield return new WaitForSeconds(newLegCooldown);
        canCreateLeg = true;
    }

    // Update is called once per frame
    void Update()
    {
        TryAttachNearbyEdible();

        if (!canCreateLeg)
            return;

        // New leg origin is placed in front of the mimic
        legPlacerOrigin = transform.position + velocity.normalized * newLegRadius;

        if (legCount <= maxLegs - partsPerLeg)
        {
            // Offset The leg origin by a random vector
            Vector2 offset = Random.insideUnitCircle * newLegRadius;
            Vector3 newLegPosition = legPlacerOrigin + new Vector3(offset.x, 0, offset.y);

            // If the mimic is moving and the new leg position is behind it, mirror it to make
            // it reach in front of the mimic.
            if (velocity.magnitude > 1f)
            {
                float newLegAngle = Vector3.Angle(velocity, newLegPosition - transform.position);

                if (Mathf.Abs(newLegAngle) > 90)
                {
                    newLegPosition = transform.position - (newLegPosition - transform.position);
                }
            }

            if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(legPlacerOrigin.x, 0, legPlacerOrigin.z)) < minLegDistance)
                newLegPosition = ((newLegPosition - transform.position).normalized * minLegDistance) + transform.position;

            // if the angle is too big, adjust the new leg position towards the velocity vector
            if (Vector3.Angle(velocity, newLegPosition - transform.position) > 45)
                newLegPosition = transform.position + ((newLegPosition - transform.position) + velocity.normalized * (newLegPosition - transform.position).magnitude) / 2f;

            if (!TryGetGroundPoint(newLegPosition, out Vector3 myHit))
                return;

            float lifeTime = Random.Range(minLegLifetime, maxLegLifetime);

            StartCoroutine("NewLegCooldown");
            for (int i = 0; i < partsPerLeg; i++)
            {
                RequestLeg(myHit, legResolution, maxLegDistance, Random.Range(minGrowCoef, maxGrowCoef), this, lifeTime);
                if (legCount >= maxLegs)
                    return;
            }
        }
    }

    // object pooling to limit leg instantiation
    void RequestLeg(Vector3 footPosition, int legResolution, float maxLegDistance, float growCoef, Mimic myMimic, float lifeTime)
    {
        GameObject newLeg;
        if (availableLegPool.Count > 0)
        {
            newLeg = availableLegPool[availableLegPool.Count - 1];
            availableLegPool.RemoveAt(availableLegPool.Count - 1);
        }
        else
        {
            newLeg = Instantiate(legPrefab, transform.position, Quaternion.identity);
        }
        newLeg.SetActive(true);
        Leg leg = newLeg.GetComponent<Leg>();
        leg.Initialize(footPosition, legResolution, maxLegDistance, growCoef, myMimic, lifeTime);
        MimicLevelEnum level = vitality != null ? vitality.MimicLevel : MimicLevelEnum.child;
        leg.SetMaterial(GetLegMaterial(level));
        leg.SetColor(GetLevelColor(level));
        newLeg.transform.SetParent(myMimic.transform);
    }

    public bool TryGetGroundPoint(Vector3 position, out Vector3 groundPoint)
    {
        RaycastHit[] hits = Physics.RaycastAll(position + Vector3.up * 10f, Vector3.down, 20f);
        float closestDistance = float.MaxValue;
        groundPoint = default;

        foreach (RaycastHit hit in hits)
        {
            if (!hit.collider.CompareTag(groundTag) || hit.distance >= closestDistance)
                continue;

            closestDistance = hit.distance;
            groundPoint = hit.point;
        }

        return closestDistance < float.MaxValue;
    }

    public bool IsWallBlocking(Vector3 movement)
    {
        Vector3 horizontalMovement = new Vector3(movement.x, 0, movement.z);
        float distance = horizontalMovement.magnitude;
        if (distance <= 0f)
            return false;

        float radius = Mathf.Max(0.01f, wallCollisionRadius);
        float height = Mathf.Max(radius * 2f, wallCollisionHeight);
        Vector3 capsuleBottom = transform.position + Vector3.up * radius;
        Vector3 capsuleTop = transform.position + Vector3.up * (height - radius);
        RaycastHit[] hits = Physics.CapsuleCastAll(
            capsuleBottom,
            capsuleTop,
            radius,
            horizontalMovement / distance,
            distance + wallCollisionPadding);

        foreach (RaycastHit hit in hits)
        {
            Transform hitTransform = hit.collider.transform;
            if (hitTransform == transform || hitTransform.IsChildOf(transform))
                continue;

            if (hit.collider.CompareTag(wallTag))
                return true;
        }

        return false;
    }

    public bool TryAttachEdible(GameObject edible)
    {
        EdibleController edibleController = edible != null
            ? edible.GetComponentInParent<EdibleController>()
            : null;

        if (edibleController == null || !edibleController.CompareTag(edibleTag) || isEating)
            return false;

        StartCoroutine(ConsumeEdible(edibleController));
        return true;
    }

    public GameObject DetachEdible()
    {
        GameObject edible = AttachedEdible;
        AttachedEdible = null;

        if (edible != null)
            edible.transform.SetParent(null, true);

        return edible;
    }

    private void TryAttachNearbyEdible()
    {
        if (AttachedEdible != null || isEating)
            return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, edibleDetectionRadius);
        foreach (Collider candidate in colliders)
        {
            EdibleController edible = candidate.GetComponentInParent<EdibleController>();
            if (edible != null && TryAttachEdible(edible.gameObject))
            {
                return;
            }
        }
    }

    private IEnumerator ConsumeEdible(EdibleController edible)
    {
        isEating = true;
        AttachedEdible = edible.gameObject;
        Transform edibleTransform = edible.transform;
        Vector3 startPosition = edibleTransform.position;
        float elapsed = 0f;

        while (elapsed < ediblePullDuration && edibleTransform != null)
        {
            elapsed += Time.deltaTime;
            float progress = ediblePullDuration <= 0f ? 1f : elapsed / ediblePullDuration;
            edibleTransform.position = Vector3.Lerp(startPosition, transform.position, progress);
            yield return null;
        }
        // yield return null;
        if (edible != null)
        {
            vitality ??= GetComponent<Vitality>();
            vitality?.AddNutrition(edible.edibleValue);
            Destroy(edible.gameObject);
        }

        AttachedEdible = null;
        isEating = false;
    }

    public void ApplyMimicLevel(MimicLevelEnum level)
    {
        Vector3 levelScale = level switch
        {
            MimicLevelEnum.teen => teenScale,
            MimicLevelEnum.adult => adultScale,
            _ => childScale
        };

        if (levelVisualRoot != null)
            levelVisualRoot.localScale = Vector3.Scale(levelVisualBaseScale, levelScale);

        Color levelColor = GetLevelColor(level);

        foreach (Renderer renderer in bodyRenderers ?? GetComponentsInChildren<Renderer>())
        {
            if (renderer.GetComponent<Leg>() == null)
                renderer.material.color = levelColor;
        }

        Material legMaterial = GetLegMaterial(level);

        foreach (Leg leg in GetComponentsInChildren<Leg>(true))
        {
            leg.SetMaterial(legMaterial);
            leg.SetColor(levelColor);
        }
    }

    private void ResolveLevelVisualRoot()
    {
        if (levelVisualRoot != null)
        {
            levelVisualBaseScale = levelVisualRoot.localScale;
            return;
        }

        foreach (Renderer renderer in bodyRenderers)
        {
            if (renderer.GetComponent<Leg>() == null)
            {
                levelVisualRoot = renderer.transform;
                levelVisualBaseScale = levelVisualRoot.localScale;
                return;
            }
        }
    }

    private Color GetLevelColor(MimicLevelEnum level)
    {
        return level switch
        {
            MimicLevelEnum.teen => teenColor,
            MimicLevelEnum.adult => adultColor,
            MimicLevelEnum.boss => bossColor,
            _ => childColor
        };
    }

    private Material GetLegMaterial(MimicLevelEnum level)
    {
        return level switch
        {
            MimicLevelEnum.teen => teenLegMaterial,
            MimicLevelEnum.adult => adultLegMaterial,
            MimicLevelEnum.boss => bossLegMaterial,
            _ => childLegMaterial
        };
    }

    public void RecycleLeg(GameObject leg)
    {
        availableLegPool.Add(leg);
        leg.SetActive(false);
    }
}