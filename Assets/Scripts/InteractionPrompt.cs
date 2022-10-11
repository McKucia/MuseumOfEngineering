using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionPrompt : MonoBehaviour
{
    public bool isDisplayed { get; private set; } = false;

    private Camera playerCamera;
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TextMeshProUGUI promptText;
 
    void Start()
    {
        playerCamera = Camera.main;
        uiPanel.SetActive(false);
    }

    void LateUpdate()
    {
        if(!isDisplayed) return;
        var rotation = Quaternion.AngleAxis(playerCamera.transform.eulerAngles.y, Vector3.up);
        transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
    }

    public void SetUp(string text)
    {
        promptText.text = text;

        uiPanel.SetActive(true);
        isDisplayed = true;
    }

    public void Close()
    {
        uiPanel.SetActive(false);
        isDisplayed = false;
    }
}
