using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvent : MonoBehaviour
{
    public UnityEvent onEnter;
    public string hitTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == hitTag)
        {
            onEnter.Invoke();
        }
    }
}