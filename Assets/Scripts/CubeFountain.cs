using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UtmostInput;

public class CubeFountain : MonoBehaviour
{
    [SerializeField]
    private GameObject indicatorObject;

    [SerializeField] 
    private GameObject cubeObject;
    

    private Camera _mainCamera;

    private GameObject _cubeParent;
    private List<Cube> _pooledCubes;
    private GameObject _indicator;
    
    void Start()
    {

        _mainCamera = Camera.main;

        _cubeParent = new GameObject(name: "cubes");
        _pooledCubes = new List<Cube>();
        for (int i = 0; i < 100; i++)
        {
            var cube = GameObject.Instantiate(cubeObject, _cubeParent.transform);
            cube.GetComponent<Cube>().Reset();
            _pooledCubes.Add(cube.GetComponent<Cube>());
        }
        
        _indicator = GameObject.Instantiate(indicatorObject);
        
        InputEventManager.inputEvent.onTouch += OnPress;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFountainPose();
    }

    private void UpdateFountainPose()
    {
        var screenCenter = _mainCamera.ViewportToScreenPoint(Vector3.one * 0.5f);
        RaycastHit hit;
        var camForward = _mainCamera.transform.forward;
        var ray = new Ray(_mainCamera.transform.position, camForward);
        if (Physics.Raycast(ray, out hit,150f) )
        {
            _indicator.SetActive(true);
            _indicator.transform.rotation = Quaternion.LookRotation(new Vector3(camForward.x,0f,camForward.z), hit.normal);
            _indicator.transform.position = hit.point;
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
            
                cube.Go(_indicator.transform.position, _indicator.transform.rotation, Vector3.up);
                break;
            }
        }
        
    }
}
