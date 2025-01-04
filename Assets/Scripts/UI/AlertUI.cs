using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertUI : MonoBehaviour
{
    [SerializeField] private Image alertImage;
    [SerializeField] private float maxOpacity = 0.4f;
    [SerializeField] private float animationSpeed = 0.5f;

    private bool isActive;

    public void Activate()
    {
        isActive = true;
        alertImage.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        isActive = false;
        alertImage.gameObject.SetActive(false);

        // Resetting the alpha (opacity)
        //Color newColor = alertImage.color;
        //newColor.a = 0f;
        //alertImage.color = newColor;
    }

    private void Awake()
    {
        Deactivate();
    }

    private void Update()
    {
        if (isActive)
        {
            Color newColor = alertImage.color;
            newColor.a = Mathf.PingPong(animationSpeed * Time.time, maxOpacity);
            alertImage.color = newColor;
        }
    }
}
