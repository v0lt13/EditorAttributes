EditorAttributes v2.0.0 Changelog:
- Added FilePath Attribute
- Added FolderPath Attribute
- Added ButtonField Attribute
- Added PropertyDropdown Attribute
- Added TabGroup Attribute
- Fixed ProgressBar label missalignment
- Handled SceneDropdown throwing an error when there are no scenes in the build settings

EditorAttributes v1.9.0 Changelog:
- Added Title Attribute
- Added InlineButton Attribute
- Added SelectionButtons Attribute
- Changed the ProgressBar attribute label
- The attributes GUIColor and ColorField can now be attached to button functions
- When dragging a GameObject into a field using the TypeFilter Attribute will get the filtered component from it instead of nothing
- Added rich text support to HelpBox and MessageBox attributes
- Added drawInBox and showLabels parameters to the DataTable Attribute

EditorAttributes v1.8.1 Changelog:
- Added some helpboxes when group fields cannot be found
- Fixed the dropdown attribute not finding the collection
- Added some missing null checks

EditorAttributes v1.8.0 Changelog:
- Added DataTable Attribute
- Added ProgressBar Attribute
- Renamed the Assembly Definitions
- The name of a field using the Required Attribute inside the help box now looks nicer
- Fixed members couldn't be found by attributes if those attributes are used inside a base class
- When a member could not be found it will now display a helpbox instead of spamming errors in the console

EditorAttributes v1.7.0 Changelog:
- You can now find values inside structs
- Fixed min or max value of a MinMaxSlider going over or under eachother when values are set by fields
- Added HideLabel Attribute
- Added Wrap Attribute
- Added Required Attribute
- Added TypeFilter Attribute
- Added SortingLayerDropdown Attribute

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
