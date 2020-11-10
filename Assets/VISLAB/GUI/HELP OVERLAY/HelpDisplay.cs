using UnityEngine;
using System;
using TMPro;


/// <summary>
/// Data class for holding the name and keycode name
/// </summary>
[Serializable]
public class KeyCodeMap
{
    public string name;
    public string key;
}

/// <summary>
/// Provides functionality for generating TextMeshPro text objcets
/// containing descriptions for keycodes used in the application.
/// Keycodes and descriptions are added in the inspector on the object the script
/// is attached to.
/// It will append the text object onto two different objects, one for game related and one for 
/// utility key codes.
/// </summary>
public class HelpDisplay : MonoBehaviour
{

    /// <summary>
    /// Must be true if object is to be displayed in world space. 
    /// </summary>
    [SerializeField]
    bool isWorldSpace = false;

    /// <summary>
    /// Holds all keycodes generated for utility 
    /// </summary>
    /// <value></value>
    [SerializeField]
    private KeyCodeMap[] utilityKeyCodes = {
        new KeyCodeMap{name="Open help", key="F1"},
        new KeyCodeMap{name="Toggle sphere", key="F2"},
        new KeyCodeMap{name="- Inc sphere radius", key="Numpad 9"},
        new KeyCodeMap{name="- Dec sphere radius", key="Numpad 7"},
        new KeyCodeMap{name="- Inc sphere resolution", key="Numpad 6"},
        new KeyCodeMap{name="- Dec sphere resolution", key="Numpad 4"},
        new KeyCodeMap{name="Toggle FPS counter", key="F3"},
        };

    /// <summary>
    /// Holds all keycodes generated for game
    /// key codes
    /// </summary>
    [SerializeField]
    private KeyCodeMap[] gameKeyCodes;

    /// <summary>
    /// The game object acting as a parent for game keycode texts
    /// </summary>
    [SerializeField]
    private GameObject gameKeyCodeGroup;

    /// <summary>
    /// The game object acting as a parent for utility keycode texts
    /// </summary>
    [SerializeField]
    private GameObject utilityKeyCodeGroup;

    /// <summary>
    /// The text mesh pro prefab to be instantiate and supply the
    /// keycode name and key to, which will be appended to the key code groups.
    /// </summary>
    [SerializeField]
    private TMP_Text textPrefab;

    private void Start()
    {
        CreateUtilityKeycodeText();
        CreateGameKeycodeText();
    }

    /// <summary>
    /// Generate all the utility key code text elements from utility key codes list, appends them to the
    /// group parent object and resets the world position of the object if the display is
    /// to be placed in world space instead of screen space.
    /// </summary>
    private void CreateUtilityKeycodeText()
    {
        foreach (var keyCode in this.utilityKeyCodes)
        {
            var textelement = Instantiate(textPrefab);
            textelement.SetText($"{keyCode.name}: {keyCode.key}");
            textelement.transform.SetParent(utilityKeyCodeGroup.transform);
            if (isWorldSpace)
            {
                WorldSpaceReset(textelement);
            }
        }
    }

    /// <summary>
    /// 
    /// Generate all the game key code text elements from game key codes list, appends them to the
    /// group parent object and resets the world position of the object if the display is
    /// to be placed in world space instead of screen space.
    /// </summary>
    private void CreateGameKeycodeText()
    {
        foreach (var keyCode in this.gameKeyCodes)
        {
            var textelement = Instantiate(textPrefab);
            textelement.SetText($"{keyCode.name}: {keyCode.key}");
            textelement.transform.SetParent(gameKeyCodeGroup.transform);
            if (isWorldSpace)
            {
                WorldSpaceReset(textelement);
            }
        }
    }

    private void WorldSpaceReset(TMP_Text element)
    {
        element.gameObject.layer = 0;
        var p = element.transform.position;
        var lp = element.transform.localPosition;
        p.z = 0f;
        lp.z = 0f;
        element.transform.position = p;
        element.transform.localPosition = lp;
    }

}
