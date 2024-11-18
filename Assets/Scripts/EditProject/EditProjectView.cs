using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class EditProjectView : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _deleteButton;
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private TMP_InputField _descriptionInput;
    [SerializeField] private TMP_InputField _goalInput;
    [SerializeField] private TMP_InputField _startTimeInput;
    [SerializeField] private TMP_InputField _endTimeInput;
    [SerializeField] private TMP_InputField _startDateInput;
    [SerializeField] private TMP_InputField _endDateInput;
    [SerializeField] private GameObject[] _disableObject;

    [SerializeField] private GameObject _first;
    [SerializeField] private GameObject _second;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action<string> NameInputed;
    public event Action<string> DescriptionInputed;
    public event Action<string> GoalInputed;
    public event Action<string> StartTimeInputed;
    public event Action<string> StartDateInputed;
    public event Action<string> EndTimeInputed;
    public event Action<string> EndDateInputed;
    public event Action BackClicked;
    public event Action SaveClicked;
    public event Action DeleteClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _nameInput.onValueChanged.AddListener(OnNameInputed);
        _descriptionInput.onValueChanged.AddListener(OnDescriptionInputed);
        _goalInput.onSelect.AddListener(GoalStartInputed);
        _goalInput.onValueChanged.AddListener(OnGoaldInputed);
        _goalInput.onDeselect.AddListener(GoladEndInputed);
        _startTimeInput.onSelect.AddListener(HideInput);
        _startTimeInput.onEndEdit.AddListener(OnStartTimeChanged);
        _startTimeInput.onDeselect.AddListener(ActivateInput);

        _endTimeInput.onSelect.AddListener(HideInput);
        _endTimeInput.onEndEdit.AddListener(OnEndTimeChanged);
        _endTimeInput.onDeselect.AddListener(ActivateInput);

        _startDateInput.onSelect.AddListener(HideInput);
        _startDateInput.onEndEdit.AddListener(OnStartDateChanged);
        _startDateInput.onDeselect.AddListener(ActivateInput);

        _endDateInput.onSelect.AddListener(HideInput);
        _endDateInput.onEndEdit.AddListener(OnEndDateChanged);
        _endDateInput.onDeselect.AddListener(ActivateInput);

        _saveButton.onClick.AddListener(OnSaveClicked);
        _backButton.onClick.AddListener(OnBackClicked);
        _deleteButton.onClick.AddListener(OnDeleteClicked);
    }

    private void OnDisable()
    {
        _nameInput.onValueChanged.RemoveListener(OnNameInputed);
        _descriptionInput.onValueChanged.RemoveListener(OnDescriptionInputed);
        _goalInput.onSelect.RemoveListener(GoalStartInputed);
        _goalInput.onValueChanged.RemoveListener(OnGoaldInputed);
        _goalInput.onDeselect.RemoveListener(GoladEndInputed);
        _startTimeInput.onEndEdit.RemoveListener(OnStartTimeChanged);
        _endTimeInput.onEndEdit.RemoveListener(OnEndTimeChanged);
        _startDateInput.onEndEdit.RemoveListener(OnStartDateChanged);
        _endDateInput.onEndEdit.RemoveListener(OnEndDateChanged);

        _startTimeInput.onDeselect.RemoveListener(ActivateInput);
        _endTimeInput.onDeselect.RemoveListener(ActivateInput);
        _startDateInput.onDeselect.RemoveListener(ActivateInput);
        _endDateInput.onDeselect.RemoveListener(ActivateInput);

        _saveButton.onClick.RemoveListener(OnSaveClicked);
        _backButton.onClick.RemoveListener(OnBackClicked);
        _deleteButton.onClick.RemoveListener(OnDeleteClicked);
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

    public void SetStartTime(string time)
    {
        _startTimeInput.text = time;
    }

    public void SetStartDate(string date)
    {
        _startDateInput.text = date;
    }

    public void SetEndTime(string text)
    {
        _endTimeInput.text = text;
    }

    public void SetEndDate(string date)
    {
        _endDateInput.text = date;
    }

    private void ValidateAndFormatTimeInput(TMP_InputField inputField, Action<string> eventAction)
    {
        string input = inputField.text;
        
        if (input.Length == 4)
        {
            try
            {
                string formattedTime = $"{input.Substring(0, 2)}:{input.Substring(2, 2)}";
                inputField.text = formattedTime;
                eventAction?.Invoke(formattedTime);
            }
            catch (Exception ex)
            {
                Debug.Log($"Error formatting time input: {ex.Message}");
            }
        }
        else if (input.Length > 4)
        {
            inputField.text = input.Substring(0, 4);
        }
    }

    private void ValidateAndFormatDateInput(TMP_InputField inputField, Action<string> eventAction)
    {
        string input = inputField.text;
        
        if (input.Length == 8)
        {
            try
            {
                string formattedDate = $"{input.Substring(0, 2)}.{input.Substring(2, 2)}.{input.Substring(4, 4)}";
                inputField.text = formattedDate;
                eventAction?.Invoke(formattedDate);
            }
            catch (Exception ex)
            {
                Debug.Log($"Error formatting date input: {ex.Message}");
            }
        }
        else if (input.Length > 8)
        {
            inputField.text = input.Substring(0, 8);
        }
    }

    private void HideInput(string text)
    {
        foreach (var @object in _disableObject)
        {
            @object.gameObject.SetActive(false);
        }
    }

    private void ActivateInput(string text)
    {
        foreach (var @object in _disableObject)
        {
            @object.gameObject.SetActive(true);
        }
    }

    private void GoalStartInputed(string text)
    {
        _first.gameObject.SetActive(false);
        _second.gameObject.SetActive(false);
    }

    private void GoladEndInputed(string text)
    {
        _first.gameObject.SetActive(true);
        _second.gameObject.SetActive(true);
    }

    private void OnBackClicked() => BackClicked?.Invoke();
    private void OnSaveClicked() => SaveClicked?.Invoke();
    private void OnNameInputed(string text) => NameInputed?.Invoke(text);
    private void OnDescriptionInputed(string text) => DescriptionInputed?.Invoke(text);
    private void OnGoaldInputed(string text) => GoalInputed?.Invoke(text);
    private void OnDeleteClicked() => DeleteClicked?.Invoke();
    private void OnStartTimeChanged(string time) => ValidateAndFormatTimeInput(_startTimeInput, StartTimeInputed);
    private void OnEndTimeChanged(string time) => ValidateAndFormatTimeInput(_endTimeInput, EndTimeInputed);
    private void OnStartDateChanged(string date) => ValidateAndFormatDateInput(_startDateInput, StartDateInputed);
    private void OnEndDateChanged(string date) => ValidateAndFormatDateInput(_endDateInput, EndDateInputed);
}