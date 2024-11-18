using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextScript : MonoBehaviour
{
    [SerializeField] private TMP_Text _nativeText;
    [SerializeField] private TMP_Text _textToCopy;

    private void Update()
    {
        _nativeText.text = _textToCopy.text;
    }
}
