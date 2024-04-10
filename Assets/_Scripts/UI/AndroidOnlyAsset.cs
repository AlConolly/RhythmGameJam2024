using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidOnlyAsset : MonoBehaviour
{
    void Start()
    {
        if (Application.platform != RuntimePlatform.Android)
            Destroy(gameObject);
    }
}
