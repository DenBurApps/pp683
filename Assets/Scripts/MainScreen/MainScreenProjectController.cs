using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainScreenProjectController : MonoBehaviour
{
    [SerializeField] private List<ProjectPlane> _planes;
    [SerializeField] private AddProjectScreen _addProject;
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private GameObject _emptyPlane;
    [SerializeField] private HobbyLogoHolder _logoHolder;
    [SerializeField] private EditProject _editProject;
    
    public event Action<ProjectPlane> PlaneOpened;
    
    private void OnEnable()
    {
        foreach (var plane in _planes)
        {
            plane.SetHolder(_logoHolder);
            plane.Opened += OpenPlane;
        }

        _addProject.Saved += EnablePlane;
        _editProject.Deleted += DeletePlane;
    }

    private void OnDisable()
    {
        foreach (var plane in _planes)
        {
            plane.Opened -= OpenPlane;
        }

        _addProject.Saved -= EnablePlane;
        _editProject.Deleted -= DeletePlane;
    }

    private void Start()
    {
        DisableAllPlanes();
        _emptyPlane.gameObject.SetActive(ArePlanesAvailable());
    }

    private void EnablePlane(ProjectData data)
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

    private void DeletePlane(ProjectPlane plane)
    {
        if (_planes.Contains(plane))
        {
            plane.ResetData();
            plane.Disable();
        }
        
        _emptyPlane.gameObject.SetActive(ArePlanesAvailable());
    }

    private void OpenPlane(ProjectPlane plane)
    {
        PlaneOpened?.Invoke(plane);
        _mainScreen.Disable();
    }
}
