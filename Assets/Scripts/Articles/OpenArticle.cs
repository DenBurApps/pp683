using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class OpenArticle : MonoBehaviour
{
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Sprite _unselectedSprite;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _favButton;
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _textText;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    private ArticlePlane _articlePlane;
    
    public event Action BackClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _backButton.onClick.AddListener(OnBackClicked);
        _favButton.onClick.AddListener(OnFavoriteClicked);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(OnBackClicked);
        _favButton.onClick.RemoveListener(OnFavoriteClicked);
    }

    private void Start()
    {
        Disable();
    }

    public void Enable(ArticlePlane plane)
    {
        _screenVisabilityHandler.EnableScreen();

        _titleText.text = plane.Data.Title;
        _image.sprite = plane.Image;
        _textText.text = plane.Data.Text;

        _favButton.image.sprite = plane.Data.IsFavorite ? _selectedSprite : _unselectedSprite;

        _articlePlane = plane;
    }

    private void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnBackClicked()
    {
        BackClicked?.Invoke();
        Disable();
    }

    private void OnFavoriteClicked()
    {
        if (_articlePlane.Data.IsFavorite)
        {
            _articlePlane.ChangeFavouriteStatus(false);
        }
        else
        {
            _articlePlane.ChangeFavouriteStatus(true);
        }
        
        _favButton.image.sprite = _articlePlane.Data.IsFavorite ? _selectedSprite : _unselectedSprite;
    }
}