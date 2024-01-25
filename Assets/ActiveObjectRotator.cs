using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObjectRotator : MonoBehaviour
{
    [SerializeField] float m_Angles = 360;

    void Update()
    {
        // Example code (for non-blocking UI)
        if (gameObject.activeSelf)
        {
            transform.Rotate(Vector3.right * m_Angles * Time.deltaTime);
        }
    }
}
