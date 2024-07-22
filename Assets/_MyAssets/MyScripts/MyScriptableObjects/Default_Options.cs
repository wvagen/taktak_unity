using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DefaultOptions", order = 1)]

public class Default_Options : ScriptableObject
{
    public string[] userName_prefix;
    public string[] name;
    public string[] surname;
    public string[] phoneNumber;
    public string[] photoPath;
    public string[] birthDate;
}
