using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DateElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _dateText;
    [SerializeField] private TMP_Text _dayText;
    
    public void SetDatesText(string date, string day)
    {
        _dateText.text = date;
        _dayText.text = day;
    }
}
