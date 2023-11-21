EditorAttributes v1.6.1 Changelog:
- Made the Helpbox a decorator attribute
- Removed the UseRGB option from the GUIColor enum
- Added enum support to Button parameters

EditorAttributes v1.6.0 Changelog:
- Added ColorField attribute
- Added GUIColor attribute
- Added option to draw groups inside boxes
- Fixed functions not beeing found
- Handled Illegal characters in path ArgumentException on the button
- Updated FoldoutGroup GUI

EditorAttributes v1.5.0 Changelog:
- Added AssetPreview attribute
- Added FoldoutGroup attribute
- Added ToggleGroup attribute
- Added the ability to show/hide or enable/disable buttons
- Button parameters now persist after you deselect an object

EditorAttributes v1.4.0 Changelog:
- Added Prefix/Suffix attribute
- Added DrawLine attribute
- Added TagDropdown attribute
- Added Image attribute
- Added VerticalGroup attribute
- Added SceneDropdown attribute

EditorAttributes v1.3.0 Changelog:
- Added MinMaxSlider attribute
- Added Clamp attribute
- Added PropertyWidth attribute
- Added LayerMask support to button parameters
- Refactored the Button attribute drawing system so now the attributes can be placed directly on the function

EditorAttributes v1.2.1 Changelog:
- Fixed public fields, properties and functions of type List not working with the dropdown attribute
- Handled an AmbiguousMatchException when creating a button that uses a function with overloads
- Buttons with parameters now show in a nice box

EditorAttributes v1.2.0 Changelog:
- Added Rename attribute
- Added HideInEditMode attribute
- Added DisableInEditMode attribute
- Properties and functions can now be used as parameters
- The MessageBox now supports enums
- The Dropdown attribute is no longer limited to strings
- You can now have functions with parameters as buttons
- Updated the summaries for some attributes
- Internal refactoring and general optimization

EditorAttributes v1.1.1 Changelog:
- Removed offline documentation
- Removed samples
- Updated links
- Updated Readme

EditorAttributes v1.1.0 Changelog:
- You can now enable/disable fields using the ConditionalField attribute
- Added enum support to the Enable/DisableField attribute
- Integer casting is no longer required for enum paramters
- Fixed drawing issues with UnityEvents and Structs on certain attributes

EditorAttributes v1.0.0 Changelog:
- Initial release
