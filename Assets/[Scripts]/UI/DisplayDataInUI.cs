using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayDataInUI : MonoBehaviour
{
    public ChangeGame displayData;

    public TMP_Text percentageCompletedText;
    public TMP_Text currentLevelText;
    public TMP_Text currentTimePlayedText;
    public int percentage;
    public int level;

    private void Start()
    {
        // Verificar si hay al menos un elemento en el arreglo
        if (displayData.percentageOfGameCompleted.Count > 0)
        {
            // Obtener el valor en la posici�n 0 del arreglo y convertirlo a cadena de texto
            float percentageValue = displayData.percentageOfGameCompleted[percentage];
            string percentageText = percentageValue.ToString();

            // Asignar la cadena resultante al campo de texto
            percentageCompletedText.text = "Completion: " + percentageText;
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
            currentLevelText.text = "Current Level: " + currentLevelTextString;
        }
        else
        {
            currentLevelText.text = "Nivel no disponible";
        }
        
        if (displayData.gamesTimePlayed != null)
        {
            int time = displayData.gamesTimePlayed[level];
            int minutes = time / 60;
            int seconds = time % 60;
            currentTimePlayedText.text = minutes + "m " + seconds + "s";
        }
    }
}