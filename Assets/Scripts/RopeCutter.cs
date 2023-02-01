using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeCutter : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D m_CutterObject;

    private void Update()
    {
        if(m_CutterObject)
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_CutterObject.gameObject.SetActive(true);
                m_CutterObject.WakeUp();
            }

            if (Input.GetMouseButton(0))
            {
                m_CutterObject.MovePosition((Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }

            if (Input.GetMouseButtonUp(0))
            {
                m_CutterObject.Sleep();
                m_CutterObject.gameObject.SetActive(false);
            }
        }
    }

}
