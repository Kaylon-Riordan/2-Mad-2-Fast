using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    public GameObject leftHandToggle;
    public GameObject controlSchemeDropdown;
    public GameObject inputDeviceDropdown;

    public void sharedLayout()
    {
        leftHandToggle.SetActive(false);
        resizeButton(controlSchemeDropdown, 435);
        resizeButton(inputDeviceDropdown, 435);
    }
    public void defaultLayout()
    {
        leftHandToggle.SetActive(true);
        resizeButton(controlSchemeDropdown, 160);
        resizeButton(inputDeviceDropdown, 160);
    }

    private void resizeButton(GameObject button, int width)
    {
        RectTransform rectTrans = button.GetComponent<RectTransform>();

        // Change size https://discussions.unity.com/t/modify-the-width-and-height-of-recttransform/551868
        Vector2 size = rectTrans.sizeDelta;
        size.x = width;

        rectTrans.sizeDelta = size;
    }

    public bool getLeftHandToggle()
    {
        return leftHandToggle.GetComponent<Toggle>().isOn;
    }

    public ControlScheme getControlScheme()
    {
        return (ControlScheme) controlSchemeDropdown.GetComponent<TMP_Dropdown>().value;
    }

    public void setControlScheme(ControlScheme setting)
    {
        controlSchemeDropdown.GetComponent<TMP_Dropdown>().value = (int) setting;
    }

    public InputMethod getInputMethod()
    {
        return (InputMethod) inputDeviceDropdown.GetComponent<TMP_Dropdown>().value;
    }
}
