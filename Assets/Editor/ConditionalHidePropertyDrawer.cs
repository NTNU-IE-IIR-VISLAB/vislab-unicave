
/// <summary>
/// Manages enabling/disabling fields in the inspector
/// depending on the attribute value from <c>ConditionalHideAttribute</c>
/// 
/// Author:
/// http://www.brechtos.com/hiding-or-disabling-inspector-properties-using-propertydrawers-within-unity-5/
/// 
/// 
/// MODIFICATIONS:
/// Author: Christoffer A Træen
/// Added reversing so we can enable a property and disable another 
/// with the same controling source.
/// </summary>

using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalHidePropertyDrawer : PropertyDrawer
{
	/// <summary>
	/// Updated on each GUI render.
	/// This checks the state of the the property.
	/// </summary>
	/// <param name="position">Position on the GUI</param>
	/// <param name="property">Property</param>
	/// <param name="label">Label of the property</param>
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
		bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

		bool wasEnabled = GUI.enabled;
		GUI.enabled = enabled;
		if (!condHAtt.HideInInspector || enabled)
		{
			EditorGUI.PropertyField(position, property, label, true);
		}

		GUI.enabled = wasEnabled;
	}

	/// <summary>
	/// Calculate the height of our property so that (when the property needs to be hidden)
	/// the following properties that are being drawn don’t overlap
	/// </summary>
	/// <param name="property">The property to get height of</param>
	/// <param name="label">The label of the property</param>
	/// <returns></returns>
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
		bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

		if (!condHAtt.HideInInspector || enabled)
		{
			return EditorGUI.GetPropertyHeight(property, label);
		}
		else
		{
			return -EditorGUIUtility.standardVerticalSpacing;
		}
	}

	/// <summary>
	/// Returns a boolean if we should enable or disable the provided attribute
	/// </summary>
	/// <param name="condHAtt">The attribute to check</param>
	/// <param name="property">The property</param>
	/// <returns></returns>
	private bool GetConditionalHideAttributeResult(ConditionalHideAttribute condHAtt, SerializedProperty property)
	{
		bool enabled = true;
		string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
		string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField); //changes the path to the conditionalsource property path
		SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);
		if (sourcePropertyValue != null)
		{
			enabled = condHAtt.Reverse ? !sourcePropertyValue.boolValue : sourcePropertyValue.boolValue;
		}
		else
		{
			Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSourceField);
		}

		return enabled;
	}
}