using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Blackboard
{
    [HideInInspector]
    public List<CustomVariable> dynamicFields = new List<CustomVariable>();
    [HideInInspector]
    private Dictionary<string, object> _blackboardDictionary = new Dictionary<string, object>();
    
    public Animator animator;
    public Rigidbody rb;
    public SkillController skillController;
    public AudioSource audioSource;

    public MonoBehaviour owner;
    public MonoBehaviour target;

    public void Initialize(MonoBehaviour monoBehaviour)
    {
        InitializeDictionary();

        owner = monoBehaviour;
        target = GameInstance.Instance.GetPlayerController();
        animator = monoBehaviour.GetComponent<Animator>();
        rb = monoBehaviour.GetComponent<Rigidbody>();
        skillController = monoBehaviour.GetComponent<SkillController>();
        audioSource = monoBehaviour.GetComponent<AudioSource>();
    }

    private void InitializeDictionary()
    {
        _blackboardDictionary = new Dictionary<string, object>();
        foreach (var variable in dynamicFields)
        {
            if (!_blackboardDictionary.ContainsKey(variable.variableName))
            {
                switch (variable.type)
                {
                    case VariableType.Int:
                        _blackboardDictionary.Add(variable.variableName, variable.intValue);
                        break;
                    case VariableType.Float:
                        _blackboardDictionary.Add(variable.variableName, variable.floatValue);
                        break;
                    case VariableType.String:
                        _blackboardDictionary.Add(variable.variableName, variable.stringValue);
                        break;
                    case VariableType.Bool:
                        _blackboardDictionary.Add(variable.variableName, variable.boolValue);
                        break;
                }
            }
        }
    }

    public T Get<T>(string variableName)
    {
        if (_blackboardDictionary.TryGetValue(variableName, out object value))
        {
            return (T)value;
        }
        return default;
    }

    public bool Set<T>(string variableName, T value)
    {
        if (_blackboardDictionary.ContainsKey(variableName))
        {
            _blackboardDictionary[variableName] = value;
            return true;
        }
        else
        {
            _blackboardDictionary.Add(variableName, value);
            return false;
        }
    }
}

[System.Serializable]
public class CustomVariable
{
    public string variableName;
    public VariableType type;
    public int intValue;
    public float floatValue;
    public string stringValue;
    public bool boolValue;
}

public enum VariableType
{
    Int,
    Float,
    String,
    Bool
}
