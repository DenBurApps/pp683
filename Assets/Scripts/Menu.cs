using System;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _addButton;
    [SerializeField] private Button _articlesButton;
    [SerializeField] private Button _settingsButton;

    public event Action HomeClicked;
    public event Action AddClicked;
    public event Action ArticlesClicked;
    public event Action SettingsClicked;

    private void OnEnable()
    {
        _homeButton.onClick.AddListener(OnHomeClicked);
        _addButton.onClick.AddListener(OnAddClicked);
        _articlesButton.onClick.AddListener(OnArticlesClicked);
        _settingsButton.onClick.AddListener(OnSettingsClicked);
    }

    private void OnDisable()
    {
        _homeButton.onClick.RemoveListener(OnHomeClicked);
        _addButton.onClick.RemoveListener(OnAddClicked);
        _articlesButton.onClick.RemoveListener(OnArticlesClicked);
        _settingsButton.onClick.RemoveListener(OnSettingsClicked);
    }

    private void OnHomeClicked() => HomeClicked?.Invoke();
    private void OnAddClicked() => AddClicked?.Invoke();
    private void OnArticlesClicked() => ArticlesClicked?.Invoke();
    private void OnSettingsClicked() => SettingsClicked?.Invoke();
}
