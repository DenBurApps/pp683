using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHobbie : MonoBehaviour
{
    [SerializeField] private AddHobieView _view;
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private PhotosController _photosController;
    
    private string _name;
    private string _description;
    private string _goal;
    private byte[] _imagePath;

    public event Action BackClicked;
    public event Action<HobbyData> Saved;

    private void OnEnable()
    {
        _photosController.SetPhoto += SetImage;
        _view.NameInputed += SetName;
        _view.DescriptionInputed += SetDescription;
        _view.GoalInputed += SetGoal;
        _view.BackClicked += OnBackClicked;
        _view.SaveClicked += Save;

        _mainScreen.AddHobbieClicked += OpenScreen;
    }

    private void OnDisable()
    {
        _photosController.SetPhoto -= SetImage;
        _view.NameInputed -= SetName;
        _view.DescriptionInputed -= SetDescription;
        _view.GoalInputed -= SetGoal;
        _view.BackClicked -= OnBackClicked;
        _view.SaveClicked -= Save;
        
        _mainScreen.AddHobbieClicked -= OpenScreen;
    }

    private void Start()
    {
        _view.Disable();
        ResetValues();
    }

    private void OpenScreen()
    {
        _view.Enable();
        ResetValues();
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
                       !string.IsNullOrEmpty(_goal) && _imagePath != null;
        
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
        
        Saved?.Invoke(data);
        OnBackClicked();
    }

    private void OnBackClicked()
    {
        ResetValues();
        _view.Disable();
        BackClicked?.Invoke();
    }
}
