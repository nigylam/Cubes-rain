using System;
using TMPro;
using UnityEngine;

public class TextBar : MonoBehaviour
{

    [SerializeField] private string _description;

    private TextMeshProUGUI _text;

    private Func<int> _valueGetter;
    private Action<Action<int>> _subscribeAction;
    private Action<Action<int>> _unsubscribeAction;
    private Action<int> UpdateCallback;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        UpdateCallback = UpdateText;
    }

    private void OnEnable()
    {
        if (_valueGetter != null)
            UpdateText(_valueGetter());

        _subscribeAction?.Invoke(UpdateCallback);
    }

    private void OnDisable()
    {
        _unsubscribeAction?.Invoke(UpdateCallback);
    }

    public void Bind(Func<int> getter, Action<Action<int>> subscribe, Action<Action<int>> unsubscribe)
    {
        _valueGetter = getter;
        _subscribeAction = subscribe;
        _unsubscribeAction = unsubscribe;

        if (isActiveAndEnabled)
        {
            _subscribeAction?.Invoke(UpdateCallback);
            UpdateText(_valueGetter());
        }
    }

    private void UpdateText(int value)
    {
        _text.text = _description + value;
    }
}