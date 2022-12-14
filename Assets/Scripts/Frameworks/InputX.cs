using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtmostInput
{
    public class InputX
    {
        List<CrossPlatformClick> generalMOInputs;

        float height = Screen.height;
        float width = Screen.width;

        Vector2 screenMiddle;

        private KeyCode leftClick = KeyCode.Mouse0;
        private KeyCode rightClick = KeyCode.Mouse1;
        private KeyCode mouseMiddleClick = KeyCode.Mouse2;
        
        private string mouseWheelAxis = "Mouse ScrollWheel";
        private string verticalLookAxis = "Mouse Y";
        private string horizontalLookAxis = "Mouse X";
        private string verticalAxis = "Vertical";
        private string horizontalAxis = "Horizontal";

        public InputX()
        {
            generalMOInputs = new List<CrossPlatformClick>();

            screenMiddle = new Vector2(width / 2f, height / 2f);
        }

        public bool SetInputs()
        {
            ResetEndedInput();

            if (MOExtendedInputs.isInputEntered() && generalMOInputs.Count == 0) // for now works with only one input
            {
                CrossPlatformClick tmpGi = new CrossPlatformClick();

                tmpGi.phase = IPhase.Began;

                tmpGi.startPosition = MOExtendedInputs.GetPoint();
                tmpGi.currentPosition = tmpGi.startPosition;
                tmpGi.lastFramePosition = tmpGi.startPosition;
                tmpGi.delta = Vector2.zero;

                tmpGi.index = generalMOInputs.Count + 1;

                generalMOInputs.Add(tmpGi);

                return true;
            }
            else if (MOExtendedInputs.isInput())
            {
                CrossPlatformClick tmpGi = generalMOInputs[0];

                tmpGi.lastFramePosition = tmpGi.currentPosition; // Before changing current position update last frame poisition

                tmpGi.currentPosition = MOExtendedInputs.GetPoint();
                tmpGi.delta = tmpGi.currentPosition - tmpGi.lastFramePosition;

                if (tmpGi.delta.magnitude < 0.2f && tmpGi.delta.magnitude > -0.2f)
                {
                    tmpGi.phase = IPhase.Stationary;
                }
                else
                {
                    tmpGi.phase = IPhase.Moved;
                }

                generalMOInputs[0] = tmpGi;

                return true;
            }
            else if (generalMOInputs.Count > 0)// && isinput değilse 
            {
                CrossPlatformClick tmpGi = generalMOInputs[0];

                tmpGi.phase = IPhase.Ended;
                tmpGi.lastFramePosition = tmpGi.currentPosition; // Before changing current position update last frame poisition
                tmpGi.currentPosition = MOExtendedInputs.GetPoint();
                tmpGi.delta = tmpGi.currentPosition - tmpGi.lastFramePosition;

                generalMOInputs[tmpGi.index - 1] = tmpGi;

                return true;
            }

            return false;
        }

        void ResetEndedInput()
        {
            int removedCount = 0;

            List<CrossPlatformClick> tmpInputs = new List<CrossPlatformClick>(generalMOInputs);
            foreach(CrossPlatformClick gmoi in generalMOInputs)
            {
                //first check whether any input is ended
                if (gmoi.phase == IPhase.Ended)
                {
                    //if input is ended remove it from list
                    tmpInputs.Remove(gmoi);

                    //Since list has changed decrease index of remaining general input`s 
                    removedCount++;
                }
                else //if input is not ended decrease inputs index by removedCount
                {
                    CrossPlatformClick indexChangedInput = tmpInputs.Find(x => x.index == gmoi.index);// -= removedCount;
                    indexChangedInput.index -= removedCount;

                    tmpInputs[gmoi.index - 1] = indexChangedInput;
                }
            }

            generalMOInputs = tmpInputs;

            /*if (generalMOInputs.Count > 0 && generalMOInputs[0].phase == IPhase.Ended)
            {
                generalMOInputs.RemoveAt(0);
            }*/
        }

        public CrossPlatformClick GetInput(int index)
        {
            return generalMOInputs[index];
        }

        /// <summary>
        /// 
        /// Returns position of mouse in screen pos which assumes
        ///  middle of screen is point (0,0), left top is (-1,1), left bottom is (-1,-1), right bottom is (1,-1), right top is (1,1)
        /// </summary>
        /// <returns></returns>
        public Vector2 GetMousePositionInCoordinates(Vector2 mousePos)
        {
            //Map mousePosition to middle coordinate system

            //subtract half of width/height from width/height
            //same calculation could be efficient without if check
            mousePos.x = (mousePos.x > screenMiddle.x) ? mousePos.x - screenMiddle.x : /*if small */ (mousePos.x - screenMiddle.x);
            mousePos.y = (mousePos.y > screenMiddle.y) ? mousePos.y - screenMiddle.y : /*if small */ (mousePos.y - screenMiddle.y);

            //now map this coordinate to unit coordinate system
            mousePos.x = (mousePos.x > 0f) ? mousePos.x / screenMiddle.x : /*if small*/ mousePos.x / screenMiddle.x;
            mousePos.y = (mousePos.y > 0f) ? mousePos.y / screenMiddle.y : /*if small*/ mousePos.y / screenMiddle.y;
            
            return mousePos;
        }

        public Vector2 GetMouseCurserPosition()
        {
            return Input.mousePosition;
        }

        #region Keyboard
        public Vector2 MouseAxis()
        {
            return new Vector2(Input.GetAxis(horizontalLookAxis), Input.GetAxis(verticalLookAxis));
        }

        public float Vertical()
        {
            return Input.GetAxis(verticalAxis);
        }

        public float VerticalMouse()
        {
            return Input.GetAxis(verticalLookAxis);
        }

        public float Horizontal()
        {
            return Input.GetAxis(horizontalAxis);
        }

        public float HorizontalMouse()
        {
            return Input.GetAxis(horizontalLookAxis);
        }

        public bool MouseRightPressing()
        {
            return Input.GetKey(rightClick);
        }

        public bool MouseRightClick()
        {
            return Input.GetKeyDown(rightClick);
        }
        
        public bool MouseLeftClick()
        {
            return Input.GetKeyDown(leftClick);
        }

        public bool MouseLeftRelease()
        {
            return Input.GetKeyUp(leftClick);
        }

        
        public float MouseWheelAxis()
        {
            return Input.GetAxis(mouseWheelAxis);
        }

        public bool MouseMiddleClick()
        {
            return Input.GetKeyDown(mouseMiddleClick);
        }
        
        public bool MouseMiddleRelease()
        {
            return Input.GetKeyUp(mouseMiddleClick);

        }
        
        public bool isSpacePressed()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }

        public bool isSpaceReleased()
        {
            return Input.GetKeyUp(KeyCode.Space);
        }

        public bool isSpace()
        {
            return Input.GetKey(KeyCode.Space);
        }

        public bool isShiftPressed()
        {
            return Input.GetKeyDown(KeyCode.LeftShift);
        }

        public bool isShiftReleased()
        {
            return Input.GetKeyUp(KeyCode.LeftShift);
        }

        public bool isShift()
        {
            return Input.GetKey(KeyCode.LeftShift);
        }

        public bool isPressedKey(KeyCode keyCode)
        {
            return Input.GetKeyDown(keyCode);
        }
        
        public bool isReleasedKey(KeyCode keyCode)
        {
            return Input.GetKeyUp(keyCode);
        }
        
        public bool isCtrl()
        {
            return Input.GetKey(KeyCode.LeftControl);
        }

       
        #endregion
    }


    public enum IPhase
    {
        Began,
        Moved,
        Stationary,
        Ended,
        Canceled
    }

    /// <summary>
    ///   <para>General input state  and position container for mouse position and mobile touch position</para>
    /// </summary>
    [System.Serializable]
    public struct CrossPlatformClick
    {
        public int index; //index of current touch. Could be used with multiTouch
        public IPhase phase;

        public Vector2 lastFramePosition
        {
            get;
            set;
        }

        public Vector2 currentPosition
        {
            get;
            set;
        }

        public Vector2 startPosition
        {
            get;
            set;
        }

        public Vector2 delta
        {
            get;
            set;
        }
    }
    

    /// <summary>
    /// MO stands for MOBILE and MOUSE
    /// </summary>
    class MOExtendedInputs
    {

#if !UNITY_EDITOR && UNITY_IOS || UNITY_ANDROID
        public static bool isInputEntered()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                return true;

            return false;
        }

        public static bool isInput()
        {
            if (Input.touchCount > 0)
                return true;

            return false;
        }

        public static bool isInputExited()
        {
            if (Input.touchCount <= 0)
                return true;

            return false;
        }

        public static Vector2 GetPoint()
        {
            if(Input.touchCount > 0)
                return Input.touches[0].position;
            
            return Vector2.zero;
        }

        
#elif UNITY_EDITOR
        public static bool isInputEntered()
        {
            if (Input.GetMouseButtonDown(0))
                return true;

            return false;
        }

        public static bool isInput()
        {
            if (Input.GetMouseButton(0))
                return true;

            return false;
        }

        public static bool isInputExited()
        {
            if (Input.GetMouseButtonUp(0))
                return true;

            return false;
        }

        public static Vector2 GetPoint()
        {
            return Input.mousePosition; // new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
        
        public static Vector2 GetAxis()
        {
            return  new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
    #endif
    }





    #region UnusedFuncs
    //Gives 1 for maximum left and minus -1 for maximum right

    /*  public Vector2 GetDirection(GeneralMOInput gmINput)
      {
          Vector2 direction = Vector2.up;

          float diff = gmINput.startPosition.x - gmINput.lastFramePosition.x;

          float directionLimit = 10f;

          if (Mathf.Abs(diff) > directionLimit / 2f)
          {
              direction.x = diff / Mathf.Abs(diff);
          }
          else
          {
              direction.x = diff / (directionLimit / 2f);
          }

          return direction;
      }*/

    #endregion
}
