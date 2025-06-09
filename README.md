# EditorAttributes
EditorAttributes is a unity package that adds some extra editor attributes to your project to easily customize your editors without having to write any editor code.

# Links
Documentation: https://editorattributesdocs.readthedocs.io

Asset Store Page: https://assetstore.unity.com/packages/tools/gui/editorattributes-269285

Discord Server: https://discord.gg/jKXvXyTzYn

Support Me: https://buymeacoffee.com/v0lt

# FAQ
Q: With what Unity version is the package compatible with?

A: The package is compatible with Unity 2022 and above, but there a few features that are only available in Unity 6 and above. Package versions before v2.2.0 that use ImGUI may be compatible with Unity 2021 as well.

Q: Does this package serialize Dictionaries, HashSets, 2D Arrays, etc.?

A: No, custom serialization is a completly different beast and not the purpose of this package and it is not something that will be implemented in the future. But there are plenty of free custom serializers on the internet that you can use along with this package. 

Q: Whats the difference between this package and others like it (Odin Inspector, Naughty Attributes, Tri-Inspector, etc.)?

A: I can spend hours making a comparison list for each package, the gist of it is that EditorAttributes has most of the features Odin has along with some extra ones but for free and open sourced, unlike Naughty Attributes it's built on UI Toolkit so it's lightweight, modern, constantly updated and designed with the default Unity look and feel to it. But this package doesn't come close to the production level of Odin since it's just me, myself and I, and it doesn't rewrite the entire inspector or does custom serialization logic. Either way it's always best to research each package for yourself and conclude what fits best for your project.

Q: Can I contribute to the package?

A: Of course! Thats basically the point of open source, just create a pull requests and if your contribution is good for the package it will be accepted. If you want contribute a completly new feature/attribute/API talk with me first since for that documentation must also be written and I need to make sure everything is tested, has a sample and fits the package.

Q: Should I get the Github version or Asset Store version?

A: There is no content difference between the Github version and the Asset Store version of the package. The only differences are in the licence, installation and update delivery.
- On Github the package is licenced as public domain while on the Unity Asset Store it licenced with the standard Unity Extension Asset licence.
- On Github if you install the package from the git URL it will install in the Packages folder of Unity which makes the package read only, installing it from the asset store will add it in the Assets folder which will allow modifications.
- On Github you can receive a package update as soon as it's pushed along with any pull requests done by any contribuitors. On the Asset Store there is a review process the update must go trough which can last up to 3 days and any contributions done after an update will only be added in the next package update.

Q: I found a problem/bug with the package, what do I do?

A: If you found a problem/bug just go to the issues tab and create a new issue explaining what and how it happened in detail with reproduction steps make sure you check other closed issues first and the Common Issues part of this README, additionaly you can join the discord server and make the issue apparent there.

Q: I have a feature idea, where do I put it?

A: Simply create a new discussion at the discussions tab or join the discord server and tell me more about the feature and it's usecase.

Q: I found a typo or an error in the documentation, what do I do?

A: You can either raise the issue here, on discord or the documentation repo (https://github.com/v0lt13/EditorAttributesDocs), you can also create a pull request there and fix the issue yourself.

Q: When is the next update coming?

A: I usually let submitted bugs/features/improvements or other ideas I have pile up on my Trello board and once there is enough content for an update I do them all at once, rather then creating 10 minor updates with one thing each. Unless there is a critical package braking issue happening in which case I fix it immediatly and push the fix on the same version.

Q: Is there any performance impact from using this package?

A: While the package is lightweight there is a small impact on editor performance that comes by default with more customization complexity but it should be negligible, if you actually experience considerably decreased editor performace try decreasing the complexity of your editors.

# Common Issues

1. The attribute applies on the collection elements instead of the collection itself.
- Unity only supports property drawers applying to collections in Unity 6 and above, upgrading your project will fix the issue.
  
2. Attribute doesn't work on custom serialized object members inside collections.
- It's a Unity bug, a temporary solution until Unity fixes this is to go to the attribute's definition and add an additional parameter to any of the constructors like this `ExampleConstructor(applyToCollection = false) : base(applyToCollection)` now you can individually set when an attribute should apply to a collection or not from the attribute itself, leaving it to false will fix the problem with nested objects in collections.

3. Attributes like Button, ShowInInspector, GUIColor and PropertyOrder don't work after I create a custom inspector.
- The logic for Button, ShowInInspector, GUIColor and PropertyOrder attributes is implemented in an EditorExtension class that inherits from the UnityEditor.Editor class, if you want those attributes to work with your custom editor you need to inherit your editor from EditorAttributes.Editor.EditorExtension and call the appropriate functions, you can read more in the Scripting API documentation.

4. Attribute doesn't work on inherited members or doesn't find it.
- The reflection system can't find inherited members if they are not accesible by the child class, mark those inherited members as protected or another modifier that gives it access to the child class.

# Features
- The asset adds over 50 Attributes that:
- Show/Hide or Enable/Disable fields based on one or more conditions
- Easily add buttons with parameter support
- Mark fields as readonly
- Create Enum like Dropdowns for any data types
- Color and personalize your inspector
- Validate assets and scenes
- Draw handles
- Create min max sliders
- Add dropdowns for Tags, Scenes, Animator Parameters and SortingLayers
- Make data tables
- Add helpboxes
- Clamp or Wrap numerical values
- Group together multiple fields
- Draw non serialized members in the inspector
- And more!

![EditorAttributes08](https://github.com/v0lt13/EditorAttributes/assets/83181883/5680dc39-d9c7-4f41-8ef4-10945b6817d6)
![EditorAttributes03](https://github.com/v0lt13/EditorAttributes/assets/83181883/e015bc88-b861-41ab-a071-fb8eab64eb3c)
![EditorAttributes04](https://github.com/user-attachments/assets/8614df33-162c-4c5f-b9fa-7a1b12b151db)
![EditorAttributes05](https://github.com/v0lt13/EditorAttributes/assets/83181883/adb2a037-bc56-4817-9e44-450cc86ed7d6)
![EditorAttributes06](https://github.com/v0lt13/EditorAttributes/assets/83181883/1525814a-2ba0-4719-9116-100336b3a48f)
![EditorAttributes09](https://github.com/user-attachments/assets/ff3a1f13-3aec-42bd-98ea-c4206d85a338)
![EditorAttributes07](https://github.com/v0lt13/EditorAttributes/assets/83181883/4588fa62-121e-4f51-945f-d83bde2d8c47)
![EditorAttributes02](https://github.com/v0lt13/EditorAttributes/assets/83181883/57792a16-3f1f-42f1-ae46-50a443a4a78e)
![ComplexInspector](https://github.com/v0lt13/EditorAttributes/assets/83181883/d25d867d-ba81-4f46-b1e3-e9a5c5c9ba3a)
