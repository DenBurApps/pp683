using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class MainScreenHobbieController : MonoBehaviour
{
    [SerializeField] private List<HobbyPlane> _planes;
    [SerializeField] private AddHobbie _addHobbie;
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private GameObject _emptyPlane;
    [SerializeField] private HobbyLogoHolder _logoHolder;
    [SerializeField] private EditHobby _editHobby;
    
    private string _savePath => Path.Combine(Application.persistentDataPath, "hobbies.json");
    
    public event Action<HobbyPlane> PlaneOpened;
    
    private void OnEnable()
    {
        foreach (var plane in _planes)
        {
            plane.SetHolder(_logoHolder);
            plane.Opened += OpenPlane;
        }

        _addHobbie.Saved += EnablePlane;
        _editHobby.Deleted += DeletePlane;
    }

    private void OnDisable()
    {
        foreach (var plane in _planes)
        {
            plane.Opened -= OpenPlane;
        }

        _addHobbie.Saved -= EnablePlane;
        _editHobby.Deleted -= DeletePlane;
    }

    private void Start()
    {
        DisableAllPlanes();
        LoadHobbies();
        _emptyPlane.gameObject.SetActive(ArePlanesAvailable());
    }

    private void EnablePlane(HobbyData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        var availablePlane = _planes.FirstOrDefault(plane => !plane.IsActive);

        if (availablePlane != null)
        {
            availablePlane.Enable(data);
        }
        
        _emptyPlane.gameObject.SetActive(ArePlanesAvailable());
        SaveHobbies();
    }

    private void DisableAllPlanes()
    {
        foreach (var plane in _planes)
        {
            plane.Disable();
        }
    }

    private bool ArePlanesAvailable()
    {
        return _planes.All(plane => !plane.IsActive);
    }

    private void DeletePlane(HobbyPlane plane)
    {
        if (_planes.Contains(plane))
        {
            plane.ResetData();
            plane.Disable();
        }
        
        _emptyPlane.gameObject.SetActive(ArePlanesAvailable());
        SaveHobbies();
    }

    private void OpenPlane(HobbyPlane plane)
    {
        PlaneOpened?.Invoke(plane);
        _mainScreen.Disable();
    }
    
    private void SaveHobbies()
    {
        try
        {
            List<HobbyData> activeHobbies = new List<HobbyData>();

            foreach (var plane in _planes)
            {
                if (plane.IsActive)
                {
                    activeHobbies.Add(plane.Data);
                }
            }

            var hobbyDataWrapper = new HobbyDataWrapper(activeHobbies);
            string json = JsonUtility.ToJson(hobbyDataWrapper, true);
            File.WriteAllText(_savePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving hobby data: {e.Message}");
        }
    }
    
    private void LoadHobbies()
    {
        try
        {
            if (File.Exists(_savePath))
            {
                string json = File.ReadAllText(_savePath);
                var loadedHobbies = JsonUtility.FromJson<HobbyDataWrapper>(json);

                for (int i = 0; i < loadedHobbies.Hobbies.Count; i++)
                {
                    if (i < _planes.Count)
                    {
                        _planes[i].Enable(loadedHobbies.Hobbies[i]);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading hobby data: {e.Message}");
        }
    }
}

[Serializable]
public class HobbyDataWrapper
{
    public List<HobbyData> Hobbies;

    public HobbyDataWrapper(List<HobbyData> hobbies)
    {
        Hobbies = hobbies;
    }
}
