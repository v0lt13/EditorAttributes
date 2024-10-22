EditorAttributes v2.4.1 Changelog:
- The validation system will no longer check for assets inside the Packages folder
- The PropertyDropdown Attribute will now save the state of the foldouts
- Fixed an issue with property dropdowns throwing "This Visual Element is not my child" errors
- Fixed an issue where visual elements don't update inside a property dropdown

EditorAttributes v2.4.0 Changelog:
- Added Validation system
- Added Validate Attribute
- Added DrawHandle Attribute
- Added SimpleTransform struct
- Added ValueButtons Attribute (only on Unity 6 and above)
- Added lineThickness parameter to the Title and Line Attribute
- Added paramters to the Required Attribute to include it in validation or not
- Added repetable button support to the button attributes
- Added VectorUtils class
- Fixed alpha not applying on the Line Attribute when using hex colors
- Fixed vectors not drawing properly in data tables
- Fixed the PropertyDropdown Attribute not refreshing when part of a group
- Fixed obsolete error thrown from the SelectionButtons drawer in Unity 6
- Renamed the ColorUtility class to ColorUtils to avoid conflicts with UnityEngine.ColorUtility
- Added the possibility to show static fields in the inspector but it’s experimental and must be manually enabled from code

EditorAttributes v2.3.0 Changelog:
- Added HideProperty Attribute
- Added button parameter support for uint, long and ulong
- Added documentation for scripting API
- Improved the backend of the package to make it easier to create custom attributes or expand it
- Serialized objects and collections should now display corectly inside groups
- Fixed conditional attributes not being able to find functions and properties inside serialized objects
- Fixed an error sometimes being thrown then deleting an object using a conditional attribute
- Fixed an error thrown when a string button parameter is null 
- Fixed an issue where the Dropdown Attribute error box doesn't update properly
- The SelectionButtons Attribute has been deprecated in Unity 6 in favor of the built in EnumButtons Attribute
- The Rename Attribute is now a decorative attribute

EditorAttributes v2.2.0 Changelog:
- Added TimeField Attribute
- Added OnValueChanged Attribute
- Added AnimatorParamDropdown Attribute
- Added titleSpace parameter to the TitleAttribute
- Removed the fieldWidth parameter from all attributes having it
- Void fields will no longer be drawn in the inspector
- Removed the drawProperty parameter from the MessageBox Attribute
- The DataTable Attribute will no longer display the field labels in array elements except for the first element
- Fixed Button Attribute parameter serialization with Unity types
- Reorganized the project
- Updated samples

• Ported the whole package from the ImGUI system to UI Toolkit which results in the following changes:
- Better layouts
- Collections will now have a scrollbar when too long
- Dynamic text will always update even if not focused on the inspector window
- The TypeFilter Attribute will now display the filtered type in the object field
- The IndentProperty Attribute will now use pixel values
- The labelWidth parameters have been changed to widthOffset and now offset the existing width instead of setting a completly new width
- The way inspectors are colored has changed and the ColorField Attribute has been deprecated for now
- You can use the UI Toolkit Debugger window with the package
- Since the inspector is now drawn with UI Toolkit any custom property drawers using ImGUI will not work, you will have to port them to UI toolkit or use the IMGUIContainer

EditorAttributes v2.1.1 Changelog:
- Added an option to disable button parameter serialization
- The MessageBox Attribute now supports dynamic string inputs
- The PropertyDropdown properties are now indented a bit
- The PropertyDropdown Attribute will now work when placed directly on a field of type ScriptableObject or Component

EditorAttributes v2.1.0 Changelog:
- Added IndentProperty Attribute
- Added HideInChildren Attribute
- Fixed members inside serialized objects nested in arrays or other serialized objects not being found
- Fixed an issue where serialized properties could not be found by grouping attributes
- Updated the ProgresBar Attribute with the built in look and removed color parameters
- Grouping attributes now work inside serialized objects
- The ToggleGroup Attribute will return the toggle value when placed on a bool
- The Prefix Attribute offset parameter will now increase the space between it and the field instead of decreasing it
- SelectionButtons, MinMaxSlider, FilePath and FolderPath attributes now display properly inside collections
- The File/FolderPath Attribute relative path will now include the Assets folder
- You can now dynamically change the string inputs on the Title, Image, Rename, Suffix and Prefix attributes
- Collections can now be affected by certain attributes (only available in unity 2023.3 and above)
- Updated samples

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
- Fixed functions not being found
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
