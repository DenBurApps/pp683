using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HobbyPlane : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private ImagePlacer _imagePlacer;
    [SerializeField] private Image _logo;
    [SerializeField] private Button _openButton;
    [SerializeField] private TMP_Text _description;

    private HobbyLogoHolder _logoHolder;

    public event Action<HobbyPlane> Opened;
    
    public HobbyData Data { get; private set; }
    public bool IsActive { get; private set; }

    private void OnEnable()
    {
        _openButton.onClick.AddListener(OnOpen);
    }

    private void OnDisable()
    {
        _openButton.onClick.RemoveListener(OnOpen);
    }

    public void SetHolder(HobbyLogoHolder logo)
    {
        _logoHolder = logo;
    }
    
    public void Enable(HobbyData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        gameObject.SetActive(true);
        IsActive = true;
        
        Data = data;

        _name.text = Data.Name;
        _description.text = Data.Description;
        _imagePlacer.SetImage(Data.ImagePath);
        _logo.enabled = false;
    }

    public void UpdateGoalData(HobbyData data)
    {
        if(data == null)
            return;

        Data = data;
        _name.text = Data.Name;
        _description.text = Data.Description;
        _imagePlacer.SetImage(Data.ImagePath);

        if (Data.Type is GoalTypes.Yes or GoalTypes.AlmostThere)
        {
            _logo.enabled = true;
            _logo.sprite = _logoHolder.GetLogoSprite(Data.Type);
        }
        else
        {
            _logo.enabled = false;
        }
    }

    public void Disable()
    {
        ResetData();
        gameObject.SetActive(false);
    }

    public void ResetData()
    {
        if (Data != null)
            Data = null;

        _description.text = string.Empty;
        _name.text = string.Empty;
        _imagePlacer.SetImage(null);
        IsActive = false;
    }

    private void OnOpen() => Opened?.Invoke(this);
}
