using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProjectPlane : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _startDate;
    [SerializeField] private TMP_Text _endDate;
    [SerializeField] private Button _openButton;
    [SerializeField] private Image _logo;

    private HobbyLogoHolder _logoHolder;

    public event Action<ProjectPlane> Opened;
    public event Action UpdatedData;

    public ProjectData Data { get; private set; }
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

    public void Enable(ProjectData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        Data = data;

        _nameText.text = Data.Name;
        _descriptionText.text = Data.Description;
        _startDate.text = Data.StartDate;
        _endDate.text = Data.EndDate;
        _logo.enabled = false;
        gameObject.SetActive(true);
        IsActive = true;
        
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

    public void UpdateData(ProjectData data)
    {
        if (data == null)
            return;

        Data = data;
        _nameText.text = Data.Name;
        _descriptionText.text = Data.Description;

        if (Data.Type is GoalTypes.Yes or GoalTypes.AlmostThere)
        {
            _logo.enabled = true;
            _logo.sprite = _logoHolder.GetLogoSprite(Data.Type);
        }
        else
        {
            _logo.enabled = false;
        }
        
        UpdatedData?.Invoke();
    }

    public void Disable()
    {
        ResetData();
        gameObject.SetActive(false);
        IsActive = false;
    }

    public void ResetData()
    {
        if (Data != null)
            Data = null;

        _descriptionText.text = string.Empty;
        _nameText.text = string.Empty;
        IsActive = false;
    }

    private void OnOpen() => Opened?.Invoke(this);
}