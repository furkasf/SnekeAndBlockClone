/*	MIT License

	Copyright (c) 2021 hibzz.games

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	SOFTWARE.
	
	---

	Author: Hibnu Hishath (sliptixx)

	Notes: Crediting is not necessary, but is greatly appreciated.

	Notes: Feel free to use it in your games/projects as is or in modified form. You
	may sell modified versions of this code, but try not to sell the code as is in the
	form of developer packages. This is licensed under MIT, so I can't stop you from 
	doing that, but that's just a dick move.

	Notes: This script is a heavily modified version of the response found in 
	https://stackoverflow.com/a/62845768/8182289. Thanks to u/ack.

*/

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using System.Reflection;

namespace Hibzz.Utility
{
	public class ScriptableListProperty<T> : PropertyDrawer
	{
		/// <summary>
		/// Is this scriptable list drawer initialized?
		/// </summary>
		private bool initialized = false;

		/// <summary>
		/// Reorderable list that can be viewed in the inspector
		/// </summary>
		private ReorderableList list;

		private float listHeight = 0;

		/// <summary>
		/// A private property
		/// </summary>
		private SerializedProperty listProperty;

		private struct PropertyCreationParams
		{
			public string Path;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if(!initialized) { Initialize(property); }

			EditorGUI.BeginProperty(position, label, property);
			
			list.DoList(position);
			
			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (list != null)
			{
				return list.GetHeight() + EditorGUIUtility.standardVerticalSpacing * 3;
			}

			return base.GetPropertyHeight(property, label);
		}

		private void Initialize(SerializedProperty property)
		{
			// mark the item as initialized
			initialized = true;

			// find list property and add it to the list
			listProperty = property.FindPropertyRelative("items");
			
			// using theat generate the reorderable list
			list = new ReorderableList(
				property.serializedObject,
				listProperty,
				draggable: true,
				displayHeader: true,
				displayAddButton: true,
				displayRemoveButton: true);

			// how to draw the header?
			list.drawHeaderCallback =
				(Rect rect) =>
				{
					EditorGUI.LabelField(rect, property.displayName);
				};

			// what to do when the remove button is pressed in the list?
			list.onRemoveCallback =
				(ReorderableList list) =>
				{
					SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(list.index);
					Object obj = element.objectReferenceValue;

					AssetDatabase.RemoveObjectFromAsset(obj);

					Object.DestroyImmediate(obj, true);
					list.serializedProperty.DeleteArrayElementAtIndex(list.index);

					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
				};

			list.drawElementCallback =
				(Rect rect, int index, bool isActive, bool isFocused) =>
				{
					SerializedProperty element = listProperty.GetArrayElementAtIndex(index);

					rect.y += 2;
					rect.width -= 10;
					rect.height = EditorGUIUtility.singleLineHeight;

					if (element.objectReferenceValue == null) { return; }

					string label = element.objectReferenceValue.name;
					EditorGUI.LabelField(rect, label, EditorStyles.boldLabel);

					// Convert this element's data to a SerializedObject so we can iterate
					// through each SerializedProperty and render a PropertyField.
					SerializedObject nestedObject = new SerializedObject(element.objectReferenceValue);

					// loop over all properties and render them
					SerializedProperty prop = nestedObject.GetIterator();
					float y = rect.y;
					while (prop.NextVisible(true))
					{
						if (prop.name == "m_Script") { continue; }

						rect.y += EditorGUIUtility.singleLineHeight;
						EditorGUI.PropertyField(rect, prop);
					}	

					nestedObject.ApplyModifiedProperties();

					// Mark edits for saving 
					if (GUI.changed) 
					{ 
						EditorUtility.SetDirty(property.serializedObject.targetObject); 
					}
				};

			// this callback helps calculate the height of the list
			list.elementHeightCallback =
				(int index) =>
				{
					float baseProp = EditorGUI.GetPropertyHeight(
					list.serializedProperty.GetArrayElementAtIndex(index),
					true);

					float additionalProps = 0;
					SerializedProperty element = listProperty.GetArrayElementAtIndex(index);
					if (element.objectReferenceValue != null)
					{
						SerializedObject property = new SerializedObject(element.objectReferenceValue);
						SerializedProperty prop = property.GetIterator();
						while (prop.NextVisible(true))
						{
							if (prop.name == "m_Script") { continue; }
							additionalProps += EditorGUIUtility.singleLineHeight;
						}
					}

					float spaceBetweenElements = EditorGUIUtility.singleLineHeight / 2;
					listHeight = baseProp + additionalProps + spaceBetweenElements;

					return listHeight;
				};

			list.onAddDropdownCallback =
				(Rect buttonRect, ReorderableList list) =>
				{
					GenericMenu menu = new GenericMenu();
					var guids = AssetDatabase.FindAssets("t:script");

					foreach (var guid in guids)
					{
						var path = AssetDatabase.GUIDToAssetPath(guid);
						var type = AssetDatabase.LoadAssetAtPath(path, typeof(Object));

						// get the current assembly and look through all classes to find the fullname of the asset
						bool typeFound = false;
						Assembly assembly = typeof(T).Assembly;

						// make sure that incoming guid type inherits the type of the list
						foreach(System.Type t in assembly.GetTypes())
						{
							if (t.IsSubclassOf(typeof(T)) && t.Name == type.name)
							{
								typeFound = true;
								break;
							}
						}

						// if the type isn't found then skip
						if(!typeFound) { continue; }

						menu.AddItem(
							new GUIContent(Path.GetFileNameWithoutExtension(path)),
							false,
							(object dataobj) =>
							{
								// make room in list
								var data = (PropertyCreationParams)dataobj;
								var index = list.serializedProperty.arraySize;
								list.serializedProperty.arraySize++;
								list.index = index;
								var element = list.serializedProperty.GetArrayElementAtIndex(index);

								// create new sub property
								var type = AssetDatabase.LoadAssetAtPath(data.Path, typeof(Object));
								var newProperty = ScriptableObject.CreateInstance(type.name);
								newProperty.name = type.name;

								AssetDatabase.AddObjectToAsset(newProperty, property.serializedObject.targetObject);
								AssetDatabase.SaveAssets();
								element.objectReferenceValue = newProperty;
								property.serializedObject.ApplyModifiedProperties();
							},
							new PropertyCreationParams() { Path = path });
					}

					menu.ShowAsContext();
				};
		}
	}
}
