using UnityEngine;

public class GameQuit : MonoBehaviour
{
    #region Instant Game Quit Method
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        Debug.Log($"<<< See You Soon, Buddy >>>");
    }
    #endregion
}