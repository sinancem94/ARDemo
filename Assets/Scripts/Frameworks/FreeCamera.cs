using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
 {
     public float sensitivity = 5f;

     private void Start()
     {
         InputEventManager.inputEvent.onMouseMoved += MouseMove;
         InputEventManager.inputEvent.onRightClick += ChangeCursor;
     }

     private void MouseMove(Vector2 axis)
     {
         transform.Rotate(0, axis.x * sensitivity, 0);
         transform.Rotate(-axis.y * sensitivity, 0, 0);
     }

     private void ChangeCursor(Vector2 clickPos)
     {
         Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
     }
     
 }
