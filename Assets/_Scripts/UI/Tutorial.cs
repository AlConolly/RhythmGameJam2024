using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour
{
    public Sprite MobileTutorial;
    public Sprite WindowsTutorial;
    private void OnEnable()
    {
        if (Application.platform == RuntimePlatform.Android || EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            GetComponent<Image>().sprite = MobileTutorial;
        }
        else if(Application.platform == RuntimePlatform.WindowsPlayer || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows)
        {
            GetComponent<Image>().sprite = WindowsTutorial;
        }
        print("Platform: " + Application.platform);
    }
}
