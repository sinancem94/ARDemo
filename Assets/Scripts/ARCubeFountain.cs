using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;
using UtmostInput;

public class ARCubeFountain : MonoBehaviour
{
    [SerializeField]
    private GameObject indicatorObject;

    [SerializeField] 
    private GameObject cubeObject;
    
    private ARSessionOrigin _sessionOrigin;
    private ARRaycastManager _raycastManager;

    private Camera _mainCamera;

    private GameObject _cubeParent;
    private List<Cube> _pooledCubes;
    private GameObject _indicator;

    void Start()
    {
        _sessionOrigin = FindObjectOfType<ARSessionOrigin>();
        _raycastManager = FindObjectOfType<ARRaycastManager>();

        _mainCamera = _sessionOrigin.camera;

        _cubeParent = new GameObject(name: "cubes");
        _pooledCubes = new List<Cube>();
        for (int i = 0; i < 100; i++)
        {
            var cube = GameObject.Instantiate(cubeObject, _cubeParent.transform);
            _pooledCubes.Add(cube.GetComponent<Cube>());
        }
        _indicator = GameObject.Instantiate(indicatorObject);
        
        InputEventManager.inputEvent.onTouchStarted += OnPress;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFountainPose();
    }

    private void UpdateFountainPose()
    {
        var screenCenter = _mainCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f));
        var hits = new List<ARRaycastHit>();
        var camForward = _mainCamera.transform.forward;

        if (_raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            _indicator.SetActive(true);
            _indicator.transform.rotation = Quaternion.LookRotation(new Vector3(camForward.x,0f,camForward.z), hits[0].pose.up);
            _indicator.transform.position = hits[0].pose.position;
        }
        else
        {
            _indicator.SetActive(false);
        }
    }

    private void OnPress(CrossPlatformClick crossPlatformClick)
    {
        if (_indicator.activeSelf)
        {
            foreach (var cube in _pooledCubes)
            {
                if(cube.IsActive)
                    continue;
            
                cube.Go(_indicator.transform.position,  _indicator.transform.rotation, Vector3.up);
                break;
            }
        }
    }
}
