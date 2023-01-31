using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(RangeData))]
public class CheckAttackRangeDecisionDrawer : PropertyDrawer
{
    public float indentsize = 10f;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        TypeOfAbilityRange rangeType = (TypeOfAbilityRange)property.FindPropertyRelative("range").enumValueIndex;

        switch (rangeType)
        {
            case TypeOfAbilityRange.Cone:
                return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2;
            case TypeOfAbilityRange.Constant:
                return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2;
            case TypeOfAbilityRange.Infinite:
                return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2;

            case TypeOfAbilityRange.LineAbility:
                return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 6;

            case TypeOfAbilityRange.SelfAbility:
                return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2;

            case TypeOfAbilityRange.SquareAbility:
                return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 3;

            case TypeOfAbilityRange.Side:
                return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 5;
            case TypeOfAbilityRange.AlternateSide:
                return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 5;
            case TypeOfAbilityRange.Cross:
                return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 4;

            case TypeOfAbilityRange.Normal:
                return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 4;

            case TypeOfAbilityRange.Item:
                return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 5;

            default:
                return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2;
        }
        
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        //Calculate rects
        var typeRect = new Rect(position.x, position.y, position.width - indentsize, EditorGUIUtility.singleLineHeight);
        //Display name
        EditorGUI.LabelField(typeRect, label);

        //Save old indent
        int indent = EditorGUI.indentLevel;
        //Push everything further
        EditorGUI.indentLevel++;


        //Find property
        DisplayVariable("range", ref position, property);

        //Should show value

        TypeOfAbilityRange rangeType = (TypeOfAbilityRange)property.FindPropertyRelative("range").enumValueIndex;

        switch (rangeType)
        {
            case TypeOfAbilityRange.Cone:
                break;
            case TypeOfAbilityRange.Constant:
                break;
            case TypeOfAbilityRange.Infinite:
                break;
            case TypeOfAbilityRange.LineAbility:
                DisplayVariable("lineDir", ref position, property);
                DisplayVariable("lineLength", ref position, property);
                DisplayVariable("stopLine", ref position, property);
                DisplayVariable("lineOffset", ref position, property);
                break;
            case TypeOfAbilityRange.SelfAbility:
                break;
            case TypeOfAbilityRange.SquareAbility:
                DisplayVariable("squareReach", ref position, property);
                break;
            case TypeOfAbilityRange.Side:
                DisplayVariable("sideDir", ref position, property);
                DisplayVariable("sideReach", ref position, property);
                DisplayVariable("sideLength", ref position, property);
                break;

            case TypeOfAbilityRange.AlternateSide:
                DisplayVariable("alternateSideDir", ref position, property);
                DisplayVariable("alternateSideReach", ref position, property);
                DisplayVariable("alternateSideLength", ref position, property);
                break;
            case TypeOfAbilityRange.Cross:
                DisplayVariable("crossLength", ref position, property);
                DisplayVariable("crossOffset", ref position, property);

                break;
            case TypeOfAbilityRange.Normal:
                DisplayVariable("movementRange", ref position, property);
                DisplayVariable("removeOrigin", ref position, property);

                break;
            case TypeOfAbilityRange.Item:
                DisplayVariable("itemRange", ref position, property);
                DisplayVariable("itemRemoveContent", ref position, property);
                break;

            default:
                break;
        }
        //Restore indent
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public void DisplayVariable(string nameOfProperty, ref Rect position, SerializedProperty property)
    {
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        //Find property
        var propertyRelative = property.FindPropertyRelative(nameOfProperty);
        //Calculate rects
        var destinationRect = new Rect(position.x, position.y, position.width - indentsize, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(destinationRect, propertyRelative, new GUIContent(propertyRelative.name), true);
    }
}

