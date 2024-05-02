using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayDataInUI : MonoBehaviour
{
    public ChangeGame displayData;

    public TMP_Text percentageCompletedText;
    public TMP_Text currentLevelText;
    public int percentage;
    public int level;

    private void Start()
    {
        // Verificar si hay al menos un elemento en el arreglo
        if (displayData.percentageOfGameCompleted.Length > 0)
        {
            // Obtener el valor en la posición 0 del arreglo y convertirlo a cadena de texto
            float percentageValue = displayData.percentageOfGameCompleted[percentage];
            string percentageText = percentageValue.ToString();

            // Asignar la cadena resultante al campo de texto
            percentageCompletedText.text = percentageText;
        }
        else
        {
            // Si no hay elementos en el arreglo, mostrar un mensaje de error en el campo de texto
            percentageCompletedText.text = "No hay datos disponibles";
        }

        // Mostrar el nivel actual
        if (displayData.currentLevel != null)
        {
            int currentLevelValue = displayData.currentLevel[level];
            string currentLevelTextString = currentLevelValue.ToString();
            currentLevelText.text = currentLevelTextString;
        }
        else
        {
            currentLevelText.text = "Nivel no disponible";
        }
    }
}
