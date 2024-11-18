using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    
    private string _savePath => Path.Combine(Application.persistentDataPath, "projects.json");
    
    public event Action<ProjectPlane> PlaneOpened;
    
    private void OnEnable()
    {
        foreach (var plane in _planes)
        {
            plane.SetHolder(_logoHolder);
            plane.Opened += OpenPlane;
            plane.UpdatedData += SaveProjects;
        }

        _addProject.Saved += EnablePlane;
        _editProject.Deleted += DeletePlane;
    }

    private void OnDisable()
    {
        foreach (var plane in _planes)
        {
            plane.Opened -= OpenPlane;
            plane.UpdatedData -= SaveProjects;
        }

        _addProject.Saved -= EnablePlane;
        _editProject.Deleted -= DeletePlane;
    }

    private void Start()
    {
        DisableAllPlanes();
        LoadProjects();
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
        SaveProjects();
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
        SaveProjects();
    }

    private void OpenPlane(ProjectPlane plane)
    {
        PlaneOpened?.Invoke(plane);
        _mainScreen.Disable();
    }
    
    private void SaveProjects()
    {
        try
        {
            List<ProjectData> activeProjects = new List<ProjectData>();

            foreach (var plane in _planes)
            {
                if (plane.IsActive)
                {
                    activeProjects.Add(plane.Data);
                }
            }

            var projectDataWrapper = new ProjectDataWrapper(activeProjects);
            string json = JsonUtility.ToJson(projectDataWrapper, true);
            File.WriteAllText(_savePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving project data: {e.Message}");
        }
    }
    
    private void LoadProjects()
    {
        try
        {
            if (File.Exists(_savePath))
            {
                string json = File.ReadAllText(_savePath);
                var loadedProjects = JsonUtility.FromJson<ProjectDataWrapper>(json);

                for (int i = 0; i < loadedProjects.Projects.Count; i++)
                {
                    if (i < _planes.Count)
                    {
                        _planes[i].Enable(loadedProjects.Projects[i]);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading project data: {e.Message}");
        }
    }
}

[Serializable]
public class ProjectDataWrapper
{
    public List<ProjectData> Projects;

    public ProjectDataWrapper(List<ProjectData> projects)
    {
        Projects = projects;
    }
}
