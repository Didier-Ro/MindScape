using UnityEngine;

public class ObjectType: MonoBehaviour
{
    [SerializeField] private OBJECT_TYPE _objectType;

    public OBJECT_TYPE GetObjectType()
    {
        return _objectType;
    }
}