using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedButton : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem = default;
  
 //if the object is Enabled the button is select
    private void OnEnable()
    {
        if (EventSystem.current == null)
        {
            eventSystem = new GameObject("EventSystem").AddComponent<EventSystem>();
            eventSystem.gameObject.AddComponent<StandaloneInputModule>();
        }
        else
        {
            eventSystem = EventSystem.current;
        }
        ChangeUISelected(gameObject);
    }

    public void ChangeUISelected(GameObject objectToSelect)
    {
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(objectToSelect);
    }
}
