using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum TypeMode
{
    Typing, Done
}

public class TypewriterUI : MonoBehaviour
{
    [SerializeField] float _delayBeforeStart = .1f;
    [SerializeField] float _timeBtwChars = .1f;
    [SerializeField] string _leadingChar = "";
    [SerializeField] bool _leadingCharBeforeDelay = false;


    private TypeMode _typeMode = TypeMode.Done;

    public IEnumerator TypeWriterText(Text _text, string writer)
    {
        _text.text = _leadingCharBeforeDelay ? _leadingChar : "";

        yield return new WaitForSeconds(_delayBeforeStart);

        foreach (char c in writer)
        {
            if (_text.text.Length > 0)
            {
                _text.text = _text.text.Substring(0, _text.text.Length - _leadingChar.Length);
            }
            _text.text += c;
            _text.text += _leadingChar;
            yield return new WaitForSeconds(_timeBtwChars);
        }

        if (_leadingChar != "")
        {
            _text.text = _text.text.Substring(0, _text.text.Length - _leadingChar.Length);
        }
    }

    public IEnumerator TypeWriterTMP(TextMeshProUGUI _tmpProText, string writer)
    {
        _typeMode = TypeMode.Typing;
        _tmpProText.text = _leadingCharBeforeDelay ? _leadingChar : "";

        yield return new WaitForSeconds(_delayBeforeStart);

        foreach (char c in writer)
        {
            if (_tmpProText.text.Length > 0)
            {
                _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - _leadingChar.Length);
            }
            _tmpProText.text += c;
            _tmpProText.text += _leadingChar;
            yield return new WaitForSeconds(_timeBtwChars);
        }

        _typeMode = TypeMode.Done;
        if (_leadingChar != "")
        {
            _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - _leadingChar.Length);
        }
    }

    public TypeMode GetWritingStatus()
    {
        return _typeMode;
    }
    public IEnumerator SpeedUp()
    {
        float speedNormal = _timeBtwChars;
        while (_typeMode == TypeMode.Typing)
        {
            _timeBtwChars = -100;
            yield return null;
        }
        _timeBtwChars = speedNormal;
    }

    public IEnumerator SpeedUp(float speed)
    {
        _timeBtwChars = speed;
        yield return null;
    }
}