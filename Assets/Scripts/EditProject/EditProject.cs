using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EditProject : MonoBehaviour
{
    [SerializeField] private EditProjectView _view;
    [SerializeField] private MainScreenProjectController _projectController;
   
    private string _name;
    private string _description;
    private string _goal;
    private string _startTime;
    private string _endTime;
    private string _startDate;
    private string _endDate;
    
    private GoalTypes _type;
    private ProjectPlane _currentPlane;

    public event Action BackClicked;
    public event Action<ProjectPlane> Deleted;

    private void OnEnable()
    {
        _view.NameInputed += SetName;
        _view.DescriptionInputed += SetDescription;
        _view.GoalInputed += SetGoal;
        _view.BackClicked += OnBackClicked;
        _view.SaveClicked += Save;
        _view.StartDateInputed += SetStartDate;
        _view.EndDateInputed += SetEndDate;
        _view.StartTimeInputed += SetStartTime;
        _view.EndTimeInputed += SetEndTime;
        _view.DeleteClicked += OnDeletePlane;

        _projectController.PlaneOpened += OpenScreen;
    }

    private void OnDisable()
    {
        _view.NameInputed -= SetName;
        _view.DescriptionInputed -= SetDescription;
        _view.GoalInputed -= SetGoal;
        _view.BackClicked -= OnBackClicked;
        _view.SaveClicked -= Save;
        _view.StartDateInputed -= SetStartDate;
        _view.EndDateInputed -= SetEndDate;
        _view.StartTimeInputed -= SetStartTime;
        _view.EndTimeInputed -= SetEndTime;
        _view.DeleteClicked -= OnDeletePlane;

        _projectController.PlaneOpened -= OpenScreen;
    }

    private void OpenScreen(ProjectPlane plane)
    {
        ResetValues();

        _currentPlane = plane;

        _name = plane.Data.Name;
        _description = plane.Data.Description;
        _goal = plane.Data.Goal;
        _startDate = plane.Data.StartDate;
        _startTime = plane.Data.StartTime;
        _endDate = plane.Data.EndDate;
        _endTime = plane.Data.EndTime;
        
        _view.SetName(_name);
        _view.SetDescript(_description);
        _view.SetGoal(_goal);
        _view.SetStartDate(_startDate);
        _view.SetEndTime(_endTime);
        _view.SetEndDate(_endDate);
        _view.SetStartTime(_startTime);
        _view.Enable();
        ValidateInput();
    }
    
    private void Start()
    {
        _view.Disable();
        ResetValues();
    }

    public void SetYes()
    {
        _type = GoalTypes.Yes;
        ValidateInput();
    }
    
    public void SetAlmost()
    {
        _type = GoalTypes.AlmostThere;
        ValidateInput();
    }

    public void SetNo()
    {
        _type = GoalTypes.No;
        ValidateInput();
    }
    
    private void SetName(string name)
    {
        _name = name;
        ValidateInput();
    }

    private void SetDescription(string text)
    {
        _description = text;
        ValidateInput();
    }

    private void SetGoal(string text)
    {
        _goal = text;
        ValidateInput();
    }

    private void SetStartDate(string date)
    {
        _startDate = date;
        ValidateInput();
    }

    private void SetEndDate(string date)
    {
        _endDate = date;
        ValidateInput();
    }

    private void SetStartTime(string time)
    {
        _startTime = time;
        ValidateInput();
    }

    private void SetEndTime(string time)
    {
        _endTime = time;
        ValidateInput();
    }
    
    private void ValidateInput()
    {
        bool isValid = !string.IsNullOrEmpty(_name) && !string.IsNullOrEmpty(_description) &&
                       !string.IsNullOrEmpty(_goal) && !string.IsNullOrEmpty(_startDate) &&
                       !string.IsNullOrEmpty(_startTime)
                       && !string.IsNullOrEmpty(_endDate) && !string.IsNullOrEmpty(_startDate);
        
        _view.ToggleSaveButton(isValid);
    }
    
    private void ResetValues()
    {
        _name = string.Empty;
        _description = string.Empty;
        _goal = string.Empty;
        _startDate = string.Empty;
        _endDate = string.Empty;
        _startTime = string.Empty;
        _endTime = string.Empty;
        _view.SetName(_name);
        _view.SetDescript(_description);
        _view.SetGoal(_goal);
        _view.SetStartDate(_startDate);
        _view.SetEndTime(_endTime);
        _view.SetEndDate(_endDate);
        _view.SetStartTime(_startTime);
    }
    
    private void Save()
    {
        var data = new ProjectData(_name, _description, _goal, _startTime, _endTime, _startDate, _endDate);
        data.Type = _type;
        
        _currentPlane.UpdateData(data);
        
        OnBackClicked();
    }
    
    private void OnBackClicked()
    {
        ResetValues();
        _view.Disable();
        BackClicked?.Invoke();
    }
    
    private void OnDeletePlane()
    {
        Deleted?.Invoke(_currentPlane);
        OnBackClicked();
    }
}
