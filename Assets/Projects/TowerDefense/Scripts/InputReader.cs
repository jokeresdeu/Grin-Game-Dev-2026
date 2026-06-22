using UnityEngine;
using UnityEngine.EventSystems;

namespace Projects.TowerDefense.Scripts
{
    /// <summary>
    /// Static input helpers. Uses legacy Input (the project's activeInputHandler is Both),
    /// and guards against clicks that land on UI so building doesn't fire through menus.
    /// </summary>
    public static class InputReader
    {
        public static bool PrimaryDown => Input.GetMouseButtonDown(0);

        public static bool ConfirmDown =>
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetMouseButtonDown(0);

        public static bool IsPointerOverUI()
        {
            if (EventSystem.current == null)
                return false;

            if (EventSystem.current.IsPointerOverGameObject())
                return true;

            for (int i = 0; i < Input.touchCount; i++)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                    return true;
            }

            return false;
        }
    }
}
