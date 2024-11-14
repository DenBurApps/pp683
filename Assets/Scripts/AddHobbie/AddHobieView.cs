using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class AddHobieView : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private TMP_InputField _descriptionInput;
    [SerializeField] private TMP_InputField _goalInput;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action<string> NameInputed;
    public event Action<string> DescriptionInputed;
    public event Action<string> GoalInputed;
    public event Action BackClicked;
    public event Action SaveClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _nameInput.onValueChanged.AddListener(OnNameInputed);
        _descriptionInput.onValueChanged.AddListener(OnDescriptionInputed);
        _goalInput.onValueChanged.AddListener(OnGoaldInputed);
        _saveButton.onClick.AddListener(OnSaveClicked);
        _backButton.onClick.AddListener(OnBackClicked);
    }

    private void OnDisable()
    {
        _nameInput.onValueChanged.RemoveListener(OnNameInputed);
        _descriptionInput.onValueChanged.RemoveListener(OnDescriptionInputed);
        _goalInput.onValueChanged.RemoveListener(OnGoaldInputed);
        _saveButton.onClick.RemoveListener(OnSaveClicked);
        _backButton.onClick.RemoveListener(OnBackClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void ToggleSaveButton(bool status)
    {
        _saveButton.interactable = status;
    }

    public void SetName(string name)
    {
        _nameInput.text = name;
    }

    public void SetDescript(string text)
    {
        _descriptionInput.text = text;
    }

    public void SetGoal(string text)
    {
        _goalInput.text = text;
    }

    private void OnBackClicked() => BackClicked?.Invoke();
    private void OnSaveClicked() => SaveClicked?.Invoke();
    private void OnNameInputed(string text) => NameInputed?.Invoke(text);
    private void OnDescriptionInputed(string text) => DescriptionInputed?.Invoke(text);
    private void OnGoaldInputed(string text) => GoalInputed?.Invoke(text);
}