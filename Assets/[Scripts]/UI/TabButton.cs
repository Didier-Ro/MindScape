using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour
{
    public TabGroup tabGroup = default;
    public Image background;
    [SerializeField]  private GameObject canvasUI = default;
    [SerializeField] private GameObject initialButton;
    private void Awake()
    {
        background = GetComponent<Image>();
    }


    public void ActivateCanvas()
    {
        canvasUI.SetActive(true);
        if (initialButton != null)
        {
            UIManager.GetInstance().ChangeUISelected(initialButton);   
        }
      
    }

    public void DeactivateCanvas()
    {
        canvasUI.SetActive(false);
    }
}