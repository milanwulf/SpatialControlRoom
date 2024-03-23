using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiButtonTester : MonoBehaviour
{
    [SerializeField] private GameObject buttonToTest;
    private Button button;

    private void Awake()
    {
        button = buttonToTest.GetComponent<Button>();
    }
    private void OnEnable()
    {
        button.onClick.AddListener(ButtonTest);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(ButtonTest);    
    }

    public void ButtonTest()
    {
        Debug.Log("This Button is working: " + buttonToTest.name);
    }
}
