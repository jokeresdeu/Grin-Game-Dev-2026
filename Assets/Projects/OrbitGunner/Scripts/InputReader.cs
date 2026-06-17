using UnityEngine;
using UnityEngine.EventSystems;

namespace Projects.OrbitGunner.Scripts
{

    public static class InputReader
    {

        public static bool FireHeld
        {
            get
            {
                if (Input.GetKey(KeyCode.Space))
                    return true;

                if (Input.GetMouseButton(0) && !IsPointerOverUI(-1))
                    return true;

                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    bool held = touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled;
                    if (held && !IsPointerOverUI(touch.fingerId))
                        return true;
                }

                return false;
            }
        }

        public static bool FireDown
        {
            get
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    return true;

                if (Input.GetMouseButtonDown(0) && !IsPointerOverUI(-1))
                    return true;

                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began && !IsPointerOverUI(touch.fingerId))
                        return true;
                }

                return false;
            }
        }

        public static bool ConfirmDown
        {
            get
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                    return true;

                return FireDown;
            }
        }

        private static bool IsPointerOverUI(int fingerId)
        {
            if (EventSystem.current == null)
                return false;

            return fingerId < 0
                ? EventSystem.current.IsPointerOverGameObject()
                : EventSystem.current.IsPointerOverGameObject(fingerId);
        }
    }
}
