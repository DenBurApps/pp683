using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class ArticleScreen : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Sprite _unselectedSprite;

    [SerializeField] private List<ArticlePlane> _planes;
    [SerializeField] private OpenArticle _openArticle;
    [SerializeField] private Button _favouriteButton;
    [SerializeField] private Menu _menu;
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private Settings _settings;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    private bool _showFavoritesOnly = false;

    public event Action HomeClicked;
    public event Action SettingsClicked;

    private string _savePath;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        _savePath = Path.Combine(Application.persistentDataPath, "Articles.json");
    }

    private void OnEnable()
    {
        _favouriteButton.onClick.AddListener(ToggleFavoriteFilter);
        _openArticle.BackClicked += Enable;

        _menu.HomeClicked += OnHomeClicked;
        _menu.SettingsClicked += OnSettingsClicked;

        _showFavoritesOnly = false;

        foreach (var plane in _planes)
        {
            plane.PlaneOpened += OpenArticle;
            plane.FavouriteClicked += SortAllPlanes;
        }

        _mainScreen.ArticlesClicked += Enable;
        _settings.ArticlesClicked += Enable;
    }

    private void OnDisable()
    {
        _favouriteButton.onClick.RemoveListener(ToggleFavoriteFilter);
        _openArticle.BackClicked -= Enable;

        _menu.HomeClicked -= OnHomeClicked;
        _menu.SettingsClicked -= OnSettingsClicked;

        foreach (var plane in _planes)
        {
            plane.PlaneOpened -= OpenArticle;
            plane.FavouriteClicked -= SortAllPlanes;
        }

        _mainScreen.ArticlesClicked -= Enable;
        _settings.ArticlesClicked -= Enable;
    }

    private void Start()
    {
        Load();
        Disable();
    }

    private void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    private void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void ToggleFavoriteFilter()
    {
        _showFavoritesOnly = !_showFavoritesOnly;

        if (_showFavoritesOnly)
        {
            SortAllPlanes();
        }
        else
        {
            ShowAllPlanes();
        }

        _image.sprite = _showFavoritesOnly ? _selectedSprite : _unselectedSprite;
    }

    private void ShowAllPlanes()
    {
        foreach (var plane in _planes)
        {
            plane.gameObject.SetActive(true);
        }
    }

    private void SortAllPlanes()
    {
        if (!_showFavoritesOnly)
        {
            ShowAllPlanes();
            return;
        }

        foreach (var plane in _planes)
        {
            if (plane.Data.IsFavorite)
            {
                plane.gameObject.SetActive(true);
            }
            else
            {
                plane.gameObject.SetActive(false);
            }
        }

        Save();
    }

    private void OpenArticle(ArticlePlane plane)
    {
        _openArticle.Enable(plane);
        Disable();
    }

    private void OnHomeClicked()
    {
        HomeClicked?.Invoke();
        Disable();
    }

    private void OnSettingsClicked()
    {
        SettingsClicked?.Invoke();
        _settings.ShowSettings();
        Disable();
    }

    private void Save()
    {
        try
        {
            var dataList = new List<ArticleData>();

            foreach (var plane in _planes)
            {
                dataList.Add(new ArticleData
                {
                    IsFavorite = plane.Data.IsFavorite
                });
            }

            string json = JsonUtility.ToJson(new ArticlesFavouriteStatusWrapper(dataList), true);
            File.WriteAllText(_savePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save data: {e}");
        }
    }

    private void Load()
    {
        try
        {
            if (File.Exists(_savePath))
            {
                string json = File.ReadAllText(_savePath);
                var loadedData = JsonUtility.FromJson<ArticlesFavouriteStatusWrapper>(json);

                for (int i = 0; i < loadedData.ArticleDatas.Count && i < _planes.Count; i++)
                {
                    _planes[i].Data.IsFavorite = loadedData.ArticleDatas[i].IsFavorite;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data: {e}");
        }
    }
}

[Serializable]
public class ArticlesFavouriteStatusWrapper
{
    public List<ArticleData> ArticleDatas;

    public ArticlesFavouriteStatusWrapper(List<ArticleData> articleDatas)
    {
        ArticleDatas = articleDatas;
    }
}