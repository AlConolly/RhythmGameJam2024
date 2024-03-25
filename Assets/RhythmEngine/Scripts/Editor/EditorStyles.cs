using UnityEditor;
using UnityEngine;

namespace RhythmEngine
{
    public static class EditorStyles
    {
        public static GUIStyle Label
        {
            get
            {
                var style = new GUIStyle(GUI.skin.label)
                {
                    wordWrap = true
                };

                return style;
            }
        }

        public static GUIStyle WarningIcon
        {
            get
            {
                var style = new GUIStyle("CN EntryWarnIconSmall");

                return style;
            }
        }

        public static GUIStyle InfoIcon
        {
            get
            {
                var style = new GUIStyle("CN EntryInfoIconSmall");

                return style;
            }
        }

        public static GUIStyle SequencerToggleOn
        {
            get
            {
                var style = new GUIStyle(GUI.skin.button)
                {
                    normal =
                    {
                        background = AssetDatabase.LoadAssetAtPath("Assets/RhythmEngine/Editor/Icons/SequencerToggleOn.png", typeof(Texture2D)) as Texture2D
                    }
                };

                return style;
            }
        }

        public static GUIStyle SequencerToggleOff
        {
            get
            {
                var style = new GUIStyle(GUI.skin.button)
                {
                    normal =
                    {
                        textColor = Color.clear,
                        background = AssetDatabase.LoadAssetAtPath("Assets/RhythmEngine/Editor/Icons/SequencerToggleOff.png", typeof(Texture2D)) as Texture2D
                    },
                };

                return style;
            }
        }

        public static GUIStyle SequencerButtonOff
        {
            get
            {
                var style = new GUIStyle(UnityEditor.EditorStyles.textField)
                {
                    fixedHeight = 0,
                    fixedWidth = 0,
                    padding = new RectOffset(6, 6, 2, 2),
                    margin = new RectOffset(3, 3, 2, 2),
                    border = new RectOffset(6, 6, 4, 4),
                    overflow = new RectOffset(0, 0, -1, 2),
                    imagePosition = ImagePosition.ImageLeft,
                };

                return style;
            }
        }
    }
}
