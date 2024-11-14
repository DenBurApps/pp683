using System;
using System.Collections;
using System.Collections.Generic;
using TheraBytes.BetterUi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MainScreen : MonoBehaviour
{
    [SerializeField] private Color _selectButtonColor;
    [SerializeField] private Color _unselectButtonColor;
    [SerializeField] private Color _selectTextColor;
    [SerializeField] private Color _unselectTextColor;

    [SerializeField] private Button _hobbyButton;
    [SerializeField] private Button _projectsButton;
    [SerializeField] private MainScreenHobbieController _hobbieController;
    [SerializeField] private Menu _menu;
    [SerializeField] private AddHobbie _addHobbie;
    [SerializeField] private EditHobby _editHobby;

    private Button _currentSelectedButton;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action AddHobbieClicked;
    public event Action AddProjectClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _hobbyButton.onClick.AddListener((() => SetButton(_hobbyButton)));
        _projectsButton.onClick.AddListener((() => SetButton(_projectsButton)));
        _menu.AddClicked += OnAddClicked;
        _addHobbie.BackClicked += Enable;
        _editHobby.BackClicked += Enable;
    }

    private void OnDisable()
    {
        _hobbyButton.onClick.RemoveListener((() => SetButton(_hobbyButton)));
        _projectsButton.onClick.RemoveListener((() => SetButton(_projectsButton)));
        _menu.AddClicked -= OnAddClicked;
        _addHobbie.BackClicked -= Enable;
        _editHobby.BackClicked -= Enable;
    }

    private void Start()
    {
        SetButton(_hobbyButton);
        _hobbieController.gameObject.SetActive(true);
        Enable();
        _projectsButton.GetComponent<BetterImage>().color = _unselectButtonColor;
        _projectsButton.GetComponentInChildren<TMP_Text>().color = _unselectTextColor;
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void SetButton(Button button)
    {
        if (_currentSelectedButton != null)
        {
            _currentSelectedButton.GetComponent<BetterImage>().color = _unselectButtonColor;
            _currentSelectedButton.GetComponentInChildren<TMP_Text>().color = _unselectTextColor;
        }

        _currentSelectedButton = button;
        _currentSelectedButton.GetComponent<BetterImage>().color = _selectButtonColor;
        _currentSelectedButton.GetComponentInChildren<TMP_Text>().color = _selectTextColor;
    }

    private void OnAddClicked()
    {
        if (_currentSelectedButton != null)
        {
            if (_currentSelectedButton == _hobbyButton)
            {
                AddHobbieClicked?.Invoke();
                Disable();
            }
        }
    }
}