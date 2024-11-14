using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class AddProjectScreenView : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private TMP_InputField _descriptionInput;
    [SerializeField] private TMP_InputField _goalInput;
    [SerializeField] private TMP_InputField _startTimeInput;
    [SerializeField] private TMP_InputField _endTimeInput;
    [SerializeField] private TMP_InputField _startDateInput;
    [SerializeField] private TMP_InputField _endDateInput;

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
        
        _startTimeInput.onValueChanged.AddListener(OnStartTimeChanged);
        _endTimeInput.onValueChanged.AddListener(OnEndTimeChanged);
        _startDateInput.onValueChanged.AddListener(OnStartDateChanged);
        _endDateInput.onValueChanged.AddListener(OnEndDateChanged);
    }

    private void OnDisable()
    {
        _nameInput.onValueChanged.RemoveListener(OnNameInputed);
        _descriptionInput.onValueChanged.RemoveListener(OnDescriptionInputed);
        _goalInput.onValueChanged.RemoveListener(OnGoaldInputed);
        _saveButton.onClick.RemoveListener(OnSaveClicked);
        _backButton.onClick.RemoveListener(OnBackClicked);
        
        _startTimeInput.onValueChanged.RemoveListener(OnStartTimeChanged);
        _endTimeInput.onValueChanged.RemoveListener(OnEndTimeChanged);
        _startDateInput.onValueChanged.RemoveListener(OnStartDateChanged);
        _endDateInput.onValueChanged.RemoveListener(OnEndDateChanged);
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

        if (Regex.IsMatch(input, @"^\d{4}$"))
        {
            int hours = int.Parse(input.Substring(0, 2));
            int minutes = int.Parse(input.Substring(2, 2));

            if (hours >= 0 && hours <= 23 && minutes >= 0 && minutes <= 59)
            {
                string formattedTime = $"{hours:D2}:{minutes:D2}";
                inputField.text = formattedTime;
                eventAction?.Invoke(formattedTime);
            }
        }
        else
        {
            string timePattern = @"^([01]?[0-9]|2[0-3]):[0-5][0-9]$";
            if (Regex.IsMatch(input, timePattern))
            {
                eventAction?.Invoke(input);
            }
        }
    }

    private void ValidateAndFormatDateInput(TMP_InputField inputField, Action<string> eventAction)
    {
        if (Regex.IsMatch(inputField.text, @"^\d{6}$"))
        {
            string day = inputField.text.Substring(0, 2);
            string month = inputField.text.Substring(2, 2);
            string year = "20" + inputField.text.Substring(4, 2);
            string formattedDate = $"{day}.{month}.{year}";

            inputField.text = formattedDate;
            eventAction?.Invoke(formattedDate);
        }
        else if (Regex.IsMatch(inputField.text, @"^\d{2}\.\d{2}\.\d{4}$"))
        {
            eventAction?.Invoke(inputField.text);
        }
    }

    private void OnBackClicked() => BackClicked?.Invoke();
    private void OnSaveClicked() => SaveClicked?.Invoke();
    private void OnNameInputed(string text) => NameInputed?.Invoke(text);
    private void OnDescriptionInputed(string text) => DescriptionInputed?.Invoke(text);
    private void OnGoaldInputed(string text) => GoalInputed?.Invoke(text);
    private void OnStartTimeChanged(string time) => ValidateAndFormatTimeInput(_startTimeInput, StartTimeInputed);
    private void OnEndTimeChanged(string time) => ValidateAndFormatTimeInput(_endTimeInput, EndTimeInputed);
    private void OnStartDateChanged(string date) => ValidateAndFormatDateInput(_startDateInput, StartDateInputed);
    private void OnEndDateChanged(string date) => ValidateAndFormatDateInput(_endDateInput, EndDateInputed);
}