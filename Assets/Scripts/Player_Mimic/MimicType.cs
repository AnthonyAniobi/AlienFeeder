using UnityEngine;

public class MimicType : MonoBehaviour
{

    [SerializeField] private Material whiteLegMaterial;
    [SerializeField] private Material yellowLegMaterial;
    [SerializeField] private Material redLegMaterial;
    [SerializeField] private Material blackLegMaterial;
    
    void Start()
    {
        Initialize(MimicTypeEnum.red);
    }

    public void Initialize(MimicTypeEnum mimicType)
    {

        GetComponent<Mimic>().Initialize(GetLegMaterial(mimicType));
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