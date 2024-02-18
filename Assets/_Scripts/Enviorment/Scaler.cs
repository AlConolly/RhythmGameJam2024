using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    public void ScaleObject(float scale)
    {
        transform.localScale = transform.localScale * scale;
    }

}
