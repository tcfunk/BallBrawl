using UnityEngine;

public class MarbleInner : MonoBehaviour
{
    public Transform parent;
    public Vector3 positionOffset;

    public void SetParent(Transform _parent)
    {
        parent = _parent;
    }

    void LateUpdate()
    {
        if (parent)
        {
            transform.position = parent.position + positionOffset;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
