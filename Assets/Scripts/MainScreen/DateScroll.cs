using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DateScroll : MonoBehaviour
{
    [SerializeField] private List<DateElement> _dateElements;
 
    private int _currentMonth;
    private int _currentYear;

    private void Awake()
    {
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
    }

    private void Start()
    {
        _currentMonth = DateTime.Now.Month;
        _currentYear = DateTime.Now.Year;
        DisableAllWindows();
        PopulateDays(_currentYear, _currentMonth);
    }

    public void NextMonth()
    {
        _currentMonth++;
        if (_currentMonth > 12)
        {
            _currentMonth = 1;
            _currentYear++;
        }

        PopulateDays(_currentYear, _currentMonth);
    }

    public void PreviousMonth()
    {
        _currentMonth--;
        if (_currentMonth < 1)
        {
            _currentMonth = 12;
            _currentYear--;
        }

        PopulateDays(_currentYear, _currentMonth);
    }

    private void PopulateDays(int year, int month)
    {
        DisableAllWindows();
        int daysInMonth = DateTime.DaysInMonth(year, month);
        DateTime today = DateTime.Now;

        int startDay = (year == today.Year && month == today.Month) ? today.Day : 1;

        for (int i = 0; i < _dateElements.Count; i++)
        {
            int day = startDay + i;
            
            if (day > daysInMonth)
            {
                _dateElements[i].gameObject.SetActive(false);
                continue;
            }

            DateTime date = new DateTime(year, month, day);
            _dateElements[i].SetDatesText(date.ToString("ddd"), day.ToString());
            _dateElements[i].gameObject.SetActive(true);
        }
    }

    private void DisableAllWindows()
    {
        foreach (var element in _dateElements)
        {
            element.gameObject.SetActive(false);
        }
    }
}
