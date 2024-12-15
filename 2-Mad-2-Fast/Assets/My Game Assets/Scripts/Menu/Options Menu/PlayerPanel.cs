using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Panel where player can select their input preferences
/// </summary>
public class PlayerPanel : MonoBehaviour
{
    public GameObject leftHandToggle;
    public GameObject controlSchemeDropdown;
    public GameObject inputDeviceDropdown;

    /// <summary>
    /// Swap to the shared layout, where toggle is disabled and dropdowns are larger
    /// </summary>
    public void sharedLayout()
    {
        leftHandToggle.SetActive(false);
        resizeButton(controlSchemeDropdown, 435);
        resizeButton(inputDeviceDropdown, 435);
    }

    /// <summary>
    /// Swap to the default layout, where toggle is enabled and dropdowns are smaller
    /// </summary>
    public void defaultLayout()
    {
        leftHandToggle.SetActive(true);
        resizeButton(controlSchemeDropdown, 160);
        resizeButton(inputDeviceDropdown, 160);
    }

    /// <summary>
    /// Change the size of a dropdown element
    /// </summary>
    /// <param name="button">Element to be resized</param>
    /// <param name="width">Width of resize</param>
    private void resizeButton(GameObject button, int width)
    {
        RectTransform rectTrans = button.GetComponent<RectTransform>();

        // Change size https://discussions.unity.com/t/modify-the-width-and-height-of-recttransform/551868
        Vector2 size = rectTrans.sizeDelta;
        size.x = width;

        rectTrans.sizeDelta = size;
    }

    /// <summary>
    /// Get the boolean stored in left hand toggle
    /// </summary>
    /// <returns>Boolean stored in left hand toggle</returns>
    public bool getLeftHandToggle()
    {
        return leftHandToggle.GetComponent<Toggle>().isOn;
    }

    /// <summary>
    /// Get the controlscheme enum of the dropdown
    /// </summary>
    /// <returns>Controlscheme enum of the dropdown</returns>
    public ControlScheme getControlScheme()
    {
        return (ControlScheme) controlSchemeDropdown.GetComponent<TMP_Dropdown>().value;
    }

    /// <summary>
    /// Change the controlscheme selected on the menu
    /// </summary>
    /// <param name="scheme">desired controlscheme to switch to</param>
    public void setControlScheme(ControlScheme scheme)
    {
        controlSchemeDropdown.GetComponent<TMP_Dropdown>().value = (int) scheme;
    }

    /// <summary>
    /// Get the InputMethod enum of the dropdown
    /// </summary>
    /// <returns>InputMethod enum of the dropdown</returns>
    public InputMethod getInputMethod()
    {
        return (InputMethod) inputDeviceDropdown.GetComponent<TMP_Dropdown>().value;
    }
}
