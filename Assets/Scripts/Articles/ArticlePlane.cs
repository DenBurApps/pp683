using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ArticlePlane : MonoBehaviour
{
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Sprite _unselectedSprite;
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private Button _favButton;
    [SerializeField] private Button _openButton;

    public event Action<ArticlePlane> PlaneOpened;
    public event Action FavouriteClicked; 

    [field: SerializeField] public ArticleData Data { get; private set; }
    public bool IsActive { get; private set; }
    public Sprite Image => _image.sprite;
    
    private void OnEnable()
    {
        gameObject.SetActive(true);
        IsActive = true;
        _text.text = Data.Text;
        _title.text = Data.Title;

        _favButton.image.sprite = Data.IsFavorite ? _selectedSprite : _unselectedSprite;
        
        _favButton.onClick.AddListener(OnFavouriteClicked);
        _openButton.onClick.AddListener(OpenClicked);
    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
        IsActive = false;
        _favButton.onClick.RemoveListener(OnFavouriteClicked);
        _openButton.onClick.RemoveListener(OpenClicked);
    }

    public void ChangeFavouriteStatus(bool status)
    {
        Data.IsFavorite = status;
        _favButton.image.sprite = Data.IsFavorite ? _selectedSprite : _unselectedSprite;
    }

    private void OpenClicked()
    {
        PlaneOpened?.Invoke(this);
    }

    private void OnFavouriteClicked()
    {
        Data.IsFavorite = !Data.IsFavorite;
        ChangeFavouriteStatus(Data.IsFavorite);
        FavouriteClicked?.Invoke();
    }
}

[Serializable]
public class ArticleData
{
    public bool IsFavorite;
    public string Title;
    public string Text;
}