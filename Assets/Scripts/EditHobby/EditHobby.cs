using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditHobby : MonoBehaviour
{
    [SerializeField] private EditHobbyView _view;
    [SerializeField] private MainScreenHobbieController _hobbieController;
    [SerializeField] private PhotosController _photosController;
    
    private string _name;
    private string _description;
    private string _goal;
    private byte[] _imagePath;

    private HobbyPlane _currentPlane;
    private GoalTypes _type;

    public event Action BackClicked;
    public event Action<HobbyPlane> Deleted; 

    private void OnEnable()
    {
        _photosController.SetPhoto += SetImage;
        _view.NameInputed += SetName;
        _view.DescriptionInputed += SetDescription;
        _view.GoalInputed += SetGoal;
        _view.BackClicked += OnBackClicked;
        _view.SaveClicked += Save;
        _view.DeleteClicked += OnDeletePlane;

        _hobbieController.PlaneOpened += OpenScreen;
    }

    private void OnDisable()
    {
        _photosController.SetPhoto -= SetImage;
        _view.NameInputed -= SetName;
        _view.DescriptionInputed -= SetDescription;
        _view.GoalInputed -= SetGoal;
        _view.BackClicked -= OnBackClicked;
        _view.SaveClicked -= Save;
        
        _hobbieController.PlaneOpened -= OpenScreen;
        _view.DeleteClicked -= OnDeletePlane;
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

    private void OpenScreen(HobbyPlane plane)
    {
        ResetValues();
        _currentPlane = plane;

        _name = _currentPlane.Data.Name;
        _description = _currentPlane.Data.Description;
        _goal = _currentPlane.Data.Goal;
        _imagePath = _currentPlane.Data.ImagePath;
        _photosController.SetPhotos(_imagePath);
        _view.SetName(_name);
        _view.SetDescript(_description);
        _view.SetGoal(_goal);
        
        _view.Enable();
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

    private void SetImage()
    {
        _imagePath = _photosController.GetPhoto();
        Debug.Log(_imagePath);
        ValidateInput();
    }

    private void ValidateInput()
    {
        bool isValid = !string.IsNullOrEmpty(_name) && !string.IsNullOrEmpty(_description) &&
                       !string.IsNullOrEmpty(_goal) && _imagePath != null &&  _type != GoalTypes.None;
        
        _view.ToggleSaveButton(isValid);
    }

    private void ResetValues()
    {
        _name = string.Empty;
        _description = string.Empty;
        _goal = string.Empty;
        _imagePath = null;
        _photosController.ResetPhotos();
        _view.SetName(_name);
        _view.SetDescript(_description);
        _view.SetGoal(_goal);
    }

    private void Save()
    {
        var data = new HobbyData(_name, _description, _goal, _imagePath);
        data.Type = _type;
        
        _currentPlane.UpdateGoalData(data);
        
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
