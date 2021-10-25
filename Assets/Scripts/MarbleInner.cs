using System;
using UnityEngine;

public class MarbleInner : MonoBehaviour
{
    [SerializeField] private Transform parent;

    private Vector3 positionOffset;

    private void Start()
    {
        positionOffset = transform.localPosition;
        transform.SetParent(null);
    }

    void LateUpdate()
    {
        transform.position = parent.position + positionOffset;
    }
}
