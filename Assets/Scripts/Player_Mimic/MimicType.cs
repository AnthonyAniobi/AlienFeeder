using UnityEngine;

public class MimicType : MonoBehaviour
{

    [SerializeField] private Material whiteLegMaterial;
    [SerializeField] private Material yellowLegMaterial;
    [SerializeField] private Material redLegMaterial;
    [SerializeField] private Material blackLegMaterial;
    
    void Start()
    {
        Initialize(MimicTypeEnum.red, 10f);
    }

    public void Initialize(MimicTypeEnum mimicType, float totalBatteriesToCollect)
    {

        GetComponent<Mimic>().Initialize(GetLegMaterial(mimicType));
        GetComponent<Vitality>().Initialize(
            mimicType.HealthValue(),
            totalBatteriesToCollect
        );
    }

    Material GetLegMaterial(MimicTypeEnum mimicType)
    {
        switch (mimicType)
        {
            case MimicTypeEnum.white:
                return whiteLegMaterial;
            case MimicTypeEnum.yellow:
                return yellowLegMaterial;
            case MimicTypeEnum.red:
                return redLegMaterial;
            case MimicTypeEnum.black:
                return blackLegMaterial;
            default:
                return whiteLegMaterial;
        }
    }
}


public enum MimicTypeEnum
{
    white,
    yellow,
    red,
    black
}

public static class MimicTypeEnumExtensions
{
    public static Color GetColor(this MimicTypeEnum mimicType)
    {
        switch (mimicType)
        {
            case MimicTypeEnum.white:
                return Color.white;
            case MimicTypeEnum.yellow:
                return Color.yellow;
            case MimicTypeEnum.red:
                return Color.red;
            case MimicTypeEnum.black:
                return Color.black;
            default:
                return Color.white;
        }
    }

    public static float HealthValue(this MimicTypeEnum mimicType)
    {
        switch (mimicType)
        {
            case MimicTypeEnum.white:
                return 100f;
            case MimicTypeEnum.yellow:
                return 110f;
            case MimicTypeEnum.red:
                return 120f;
            case MimicTypeEnum.black:
                return 150f;
            default:
                return 100f;
        }
    }
}