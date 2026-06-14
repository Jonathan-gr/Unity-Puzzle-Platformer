using UnityEngine;
public interface IButtonListener
{
    void OnButtonPressed(MonoBehaviour sender);
    void OnButtonReleased(MonoBehaviour sender);
}