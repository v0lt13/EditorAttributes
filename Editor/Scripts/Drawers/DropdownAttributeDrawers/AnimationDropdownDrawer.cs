using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEditor.Animations;
using UnityEngine.UIElements;
using System.Collections.Generic;
using EditorAttributes.Editor.Utility;
using EditorAttributes.Attributes;
using UnityEditor.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(AnimationDropdownAttribute), true)]
    public class AnimationDropdownDrawer : CollectionDisplayDrawer
    {
        private readonly Dictionary<int, string> _animatorHashToName = new();

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (!IsSupportedPropertyType(property))
            {
                return new HelpBox($"{attribute.GetType().Name} can only be attached to a string or int (current type: {property.propertyType})", HelpBoxMessageType.Error);
            }

            VisualElement root = new();
            HelpBox errorBox = new();

            List<string> choices = GetChoices(property, errorBox);
            DropdownField dropdownField = CreateDropdownField(choices, property);

            root.Add(dropdownField);

            UpdateVisualElement(dropdownField, () =>
            {
                errorBox.text = string.Empty;
                var updatedChoices = GetChoices(property, errorBox);

                if (IsCollectionValid(updatedChoices))
                {
                    dropdownField.choices = updatedChoices;

                    if (!property.hasMultipleDifferentValues)
                    {
                        var storedName = GetStoredName(property);
                        dropdownField.SetValueWithoutNotify(storedName);

                        if (!string.IsNullOrEmpty(storedName) && !updatedChoices.Contains(storedName))
                        {
                            errorBox.text = $"<b>{storedName}</b> was not found in the Animator Controller";
                        }
                    }
                }

                DisplayErrorBox(root, errorBox);
            });

            return root;
        }

        protected override void PasteValue(VisualElement element, SerializedProperty property, string clipboardValue)
        {
            var dropdown = element as DropdownField;

            string entryName = clipboardValue;

            if (int.TryParse(clipboardValue, out int entryHash) && _animatorHashToName.TryGetValue(entryHash, out string hashedName))
            {
                entryName = hashedName;
            }

            if (dropdown.choices.Contains(entryName))
            {
                dropdown.value = entryName;
            }
            else
            {
                Debug.LogWarning($"Could not paste value <b>{clipboardValue}</b> since it is not available as an option in the dropdown");
            }
        }

        protected override DropdownField CreateDropdownField(List<string> choices, SerializedProperty property)
        {
            var isValid = IsCollectionValid(choices);
            DropdownField dropdownField = new(property.displayName)
            {
                choices = isValid ? choices : nullList,
                tooltip = property.tooltip
            };

            dropdownField.SetValueWithoutNotify(isValid ? GetStoredName(property) : nullList[0]);
            dropdownField.showMixedValue = property.hasMultipleDifferentValues;
            dropdownField.AddToClassList(DropdownField.alignedFieldUssClassName);
            AddPropertyContextMenu(dropdownField, property);

            dropdownField.TrackPropertyValue(property, trackedProperty => SetDropdownValueFromProperty(trackedProperty, dropdownField));
            dropdownField.RegisterValueChangedCallback(_ => SetPropertyValueFromDropdown(property, dropdownField));
            dropdownField.RegisterCallbackOnce<GeometryChangedEvent>(_ => dropdownField.Q(className: DropdownField.inputUssClassName).style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f);

            return dropdownField;
        }

        private string GetStoredName(SerializedProperty property)
        {
            if (property.propertyType is SerializedPropertyType.String) { return property.stringValue; }
            if (_animatorHashToName.TryGetValue(property.intValue, out var entryName)) { return entryName; }
            return property.intValue == 0 ? string.Empty : $"Missing ({property.intValue})";
        }

        protected override void SetPropertyValueFromDropdown(SerializedProperty property, DropdownField dropdown)
        {
            if (property.hasMultipleDifferentValues) { return; }

            if (property.propertyType is SerializedPropertyType.String)
            {
                property.stringValue = dropdown.value;
            }
            else
            {
                property.intValue = Animator.StringToHash(dropdown.value);
            }

            property.serializedObject.ApplyModifiedProperties();
        }

        protected override void SetDropdownValueFromProperty(SerializedProperty trackedProperty, DropdownField dropdownField)
        {
            string entryName;

            if (trackedProperty.propertyType is SerializedPropertyType.Integer)
            {
                _animatorHashToName.TryGetValue(trackedProperty.intValue, out entryName);
            }
            else
            {
                entryName = trackedProperty.stringValue;
            }

            if (dropdownField.choices.Contains(entryName))
            {
                dropdownField.SetValueWithoutNotify(entryName);
            }
        }

        protected override bool IsSupportedPropertyType(SerializedProperty property) => property.propertyType is SerializedPropertyType.String or SerializedPropertyType.Integer;

        private List<string> GetChoices(SerializedProperty property, HelpBox errorBox)
        {
            _animatorHashToName.Clear();

            RuntimeAnimatorController runtimeController = GetRuntimeAnimatorController(property, errorBox);
            if (runtimeController == null) { return null; }

            AnimatorController editorController = GetEditorController(runtimeController, errorBox);
            if (editorController == null) { return null; }

            return attribute switch
            {
                AnimationParamDropdownAttribute => GetAnimatorParameters(editorController),
                AnimationNameDropdownAttribute => GetAnimationStateNames(editorController),
                _ => throw new NotImplementedException(),
            };
        }

        private string GetFieldName() => ((AnimationDropdownAttribute)attribute).FieldName;

        private RuntimeAnimatorController GetRuntimeAnimatorController(SerializedProperty property, HelpBox errorBox)
        {
            string fieldName = GetFieldName();
            if (string.IsNullOrEmpty(fieldName))
            {
                if (property.serializedObject.targetObject is Component component)
                {
                    if (!component.TryGetComponent<Animator>(out var animator))
                    {
                        errorBox.text = "The target object does not have an <b>Animator</b> component";
                        return null;
                    }

                    var controller = animator.runtimeAnimatorController;
                    if (controller == null)
                    {
                        errorBox.text = "The Animator component does not have a RuntimeAnimatorController assigned";
                        return null;
                    }

                    return controller;
                }

                errorBox.text = "The target object is not a <b>Component</b>";
                return null;
            }

            MemberInfo memberInfo = ReflectionUtils.GetValidMemberInfo(fieldName, property);
            if (memberInfo == null)
            {
                errorBox.text = $"The field <b>{fieldName}</b> could not be found";
                return null;
            }

            Type memberInfoType = ReflectionUtils.GetMemberInfoType(memberInfo);

            if (memberInfoType == typeof(Animator))
            {
                var animator = ReflectionUtils.GetMemberInfoValue(memberInfo, property) as Animator;
                if (animator == null)
                {
                    errorBox.text = $"The provided field <b>{fieldName}</b> is not assigned";
                    return null;
                }

                var controller = animator.runtimeAnimatorController;
                if (controller == null)
                {
                    errorBox.text = $"The Animator component on field <b>{fieldName}</b> does not have a RuntimeAnimatorController assigned";
                    return null;
                }

                return controller;
            }

            if (typeof(RuntimeAnimatorController).IsAssignableFrom(memberInfoType))
            {
                var controller = ReflectionUtils.GetMemberInfoValue(memberInfo, property) as RuntimeAnimatorController;
                if (controller == null)
                {
                    errorBox.text = $"The provided field <b>{fieldName}</b> is not assigned";
                    return null;
                }

                return controller;
            }

            errorBox.text = $"The provided field <b>{fieldName}</b> is not of type <b>Animator</b> or <b>RuntimeAnimatorController</b>";
            return null;
        }

        private AnimatorController GetEditorController(RuntimeAnimatorController runtimeController, HelpBox errorBox)
        {
            var controller = runtimeController;
            if (runtimeController is AnimatorOverrideController overrideController)
            {
                controller = overrideController.runtimeAnimatorController;
                if (controller == null)
                {
                    errorBox.text = "Could not resolve the <b>AnimatorController</b> from AnimatorOverrideController. Have you assigned the base Controller?";
                    return null;
                }
            }

            // Hack for having the animator refresh its parameters when editing them in edit mode otherwise the parameters array will be empty
            var controllerPath = AssetDatabase.GetAssetPath(controller);
            var editorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);

            if (editorController == null)
            {
                errorBox.text = $"Could not resolve the <b>AnimatorController</b> asset at path <b>{controllerPath}</b>";
                return null;
            }

            return editorController;
        }

        private List<string> GetAnimatorParameters(AnimatorController editorController)
        {
            List<string> paramList = new();

            foreach (var parameter in editorController.parameters)
            {
                paramList.Add(parameter.name);
                _animatorHashToName[parameter.nameHash] = parameter.name;
            }

            return paramList;
        }

        private List<string> GetAnimationStateNames(AnimatorController editorController)
        {
            List<string> stateList = new();

            foreach (var layer in editorController.layers)
            {
                CollectStateNames(layer.stateMachine, layer.name);
            }

            return stateList;

            void CollectStateNames(AnimatorStateMachine stateMachine, string path)
            {
                foreach (var childState in stateMachine.states)
                {
                    var statePath = $"{path}.{childState.state.name}";

                    if (_animatorHashToName.TryAdd(Animator.StringToHash(statePath), statePath))
                    {
                        stateList.Add(statePath);
                    }
                }

                foreach (var childMachine in stateMachine.stateMachines)
                {
                    CollectStateNames(childMachine.stateMachine, $"{path}.{childMachine.stateMachine.name}");
                }
            }
        }
    }
}