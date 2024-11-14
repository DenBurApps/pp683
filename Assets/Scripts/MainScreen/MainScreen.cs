using System;
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
    [SerializeField] private MainScreenProjectController _projectController;
    [SerializeField] private Menu _menu;
    [SerializeField] private AddHobbie _addHobbie;
    [SerializeField] private EditHobby _editHobby;
    [SerializeField] private AddProjectScreen _addProjectScreen;
    [SerializeField] private EditProject _editProject;

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
        _addProjectScreen.BackClicked += Enable;
        _editProject.BackClicked += Enable;
    }

    private void OnDisable()
    {
        _hobbyButton.onClick.RemoveListener((() => SetButton(_hobbyButton)));
        _projectsButton.onClick.RemoveListener((() => SetButton(_projectsButton)));
        _menu.AddClicked -= OnAddClicked;
        _addHobbie.BackClicked -= Enable;
        _editHobby.BackClicked -= Enable;
        _addProjectScreen.BackClicked -= Enable;
        _editProject.BackClicked -= Enable;
    }

    private void Start()
    {
        SetButton(_hobbyButton);
        _hobbieController.gameObject.SetActive(true);
        _projectController.gameObject.SetActive(false);
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

        if (_currentSelectedButton == _hobbyButton)
        {
            _hobbieController.gameObject.SetActive(true);
            _projectController.gameObject.SetActive(false);
        }
        else
        {
            _hobbieController.gameObject.SetActive(false);
            _projectController.gameObject.SetActive(true);
        }
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
            else
            {
                AddProjectClicked?.Invoke();
                Disable();
            }
        }
    }
}