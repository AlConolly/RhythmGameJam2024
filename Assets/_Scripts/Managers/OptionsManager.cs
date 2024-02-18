using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{


    public Text returnText;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneSwitcher.optsWithPause)
        {
            returnText.text = "Return to game";    
        } else {
            returnText.text = "Return to menu";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
