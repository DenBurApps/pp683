using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    private void OpenPlane(HobbyPlane plane)
    {
        PlaneOpened?.Invoke(plane);
        _mainScreen.Disable();
    }
}
