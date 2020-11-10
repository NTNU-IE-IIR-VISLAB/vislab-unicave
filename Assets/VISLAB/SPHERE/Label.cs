using System;
using TMPro;
using UnityEngine;
public class Label : MonoBehaviour {
    [SerializeField]
    private TMP_Text label;

    private float MinFontSize { get; set; } = 0.10f;

    private float MaxFontSize { get; set; } = 2;

    private float CurrentFontSize { get; set; } = 0.10f;

    void Awake() {
        if (label is null) throw new NullReferenceException("Add text mesh pro reference to Label");
        this.CurrentFontSize = label.fontSize;
    }

    private void OnEnable() {
        label.fontSize = CurrentFontSize;
    }

    public void SetTextOnLabel(string text) {
        if (text is null) label.SetText("");
        label.SetText(text);
    }

    public void SetFontSize(float size) {
        label.fontSize = size;
        CurrentFontSize = size;
    }

    public void IncreaseFontSize(float size) {
        float increasedFontSize = CurrentFontSize + size;
        if (increasedFontSize <= MaxFontSize) {
            SetFontSize(increasedFontSize);
        }
    }

    public void DecreaseFontSize(float size) {
        float decreasedFontSize = CurrentFontSize - size;
        if (decreasedFontSize >= MinFontSize) {
            SetFontSize(decreasedFontSize);
        }
    }

}