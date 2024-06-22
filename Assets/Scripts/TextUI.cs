using TMPro;
using UnityEngine;

public class TextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textTotal;
    [SerializeField] private TextMeshProUGUI _textActive;
    [SerializeField] private TextMeshProUGUI _textName;
    [SerializeField] private MonoBehaviour _spawner;

    private ICounter _counter;

    private void OnValidate()
    {
        if (_spawner is ICounter)
        {
            return;
        }
    }

    private void Awake()
    {
        _counter = _spawner as ICounter;
    }

    private void OnEnable()
    {
        _counter.ChangedActive += OnChangedCurrent;
        _counter.ChangedTotal += OnChangedTotal;
    }

    private void Start()
    {
        _textName.text = _counter.Name;
    }

    private void OnChangedTotal(int value)
    {
        _textTotal.text = value.ToString();
    }

    private void OnChangedCurrent(int value)
    {
        _textActive.text = value.ToString();
    }
}
