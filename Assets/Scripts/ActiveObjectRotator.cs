using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObjectRotator : MonoBehaviour
{
    [SerializeField] float m_Angles = 360;

    void Update()
    {
        RotateObject();
    }

    void RotateObject()
    {
        if (gameObject.activeSelf)
        {
            transform.Rotate(Vector3.back * m_Angles * Time.deltaTime);
        }
    }

    public void ResetRotation()
    {
        if (gameObject.activeSelf)
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
