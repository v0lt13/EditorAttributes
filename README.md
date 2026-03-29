# EditorAttributes
Some handy editor attributes to easily customize editors without code.

# Links
Documentation: https://editorattributesdocs.readthedocs.io

Asset Store Page: https://assetstore.unity.com/packages/tools/gui/editorattributes-269285

Discord Server: https://discord.gg/jKXvXyTzYn

Support Me: https://buymeacoffee.com/v0lt

# FAQ
Q: With what Unity version is the package compatible with?

A: The package is compatible with Unity 6.0 and above. Package versions eariler then 3.0.0 are also compatible with Unity 2022.

Q: Does this package serialize Dictionaries, HashSets, 2D Arrays, etc.?

A: No, custom serialization is a completly different beast and not the purpose of this package and it is not something that will be implemented in the future. But there are plenty of free custom serializers on the internet that you can use along with this package. 

Q: Whats the difference between this package and others like it (Odin Inspector, Naughty Attributes, Tri-Inspector, etc.)?

A: I can spend hours making a comparison list for each package, the gist of it is that EditorAttributes has most of the features Odin has along with some extra ones but for free and open sourced, unlike Naughty Attributes it's built on UI Toolkit so it's lightweight, modern, constantly updated and designed with the default Unity look and feel to it. But this package doesn't come close to the production level of Odin since it's just me, myself and I, and it doesn't rewrite the entire inspector or does custom serialization logic. Either way it's always best to research each package for yourself and conclude what fits best for your project.

Q: Can I contribute to the package?

A: Of course! Thats basically the point of open source, just create a pull requests and if your contribution is good for the package it will be accepted. If you want contribute a completly new feature/attribute/API talk with me first since for that documentation must also be written and I need to make sure everything is tested, has a sample and fits the package.

Q: Should I get the Github version or Asset Store version?

A: There is no content difference between the Github version and the Asset Store version of the package. The only differences are in the licence, installation and update delivery.
- On Github the package is licenced as public domain while on the Unity Asset Store it licenced with the standard Unity Extension Asset licence.
- On Github if you install the package from the git URL it will install in the Packages folder of Unity, installing it from the asset store will add it in the Assets folder.
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

1. Attributes like Button Attribute, ShowInInspector Attribute, GUIColor Attribute and PropertyOrder Attribute don’t work after I create a custom inspector.

- The logic for Button Attribute, ShowInInspector Attribute, GUIColor Attribute and PropertyOrder Attribute is implemented in an EditorExtension class that inherits from the UnityEditor.Editor class, if you want those attributes to work with your custom editor you need to inherit your editor from EditorExtension and call the appropriate functions

2. I get a “You cannot use EditorAttributes with ImGUI based editors. Convert your editor to UI Toolkit for attributes to work, or remove the attributes from properties drawn by the editor script.” warning message in my inspector.

- This warning message appears when you try to use EditorAttributes in an ImGUI based custom editor, EditorAttributes only works with UI Toolkit based editors. To fix this follow the instructions in the message, also check if the issue comes from another package you have installed that overrides the base editor using ImGUI.

# Features
The asset adds over 50 Attributes that:

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
- Add help boxes
- Clamp or Wrap numerical values
- Group together multiple fields
- Draw non serialized members in the inspector
- And more!

<img width="1515" height="1059" alt="EditorAttributes08" src="https://github.com/user-attachments/assets/3e24e43c-b2ef-4e19-92f0-0ec714c4cab9" />
<img width="1515" height="1059" alt="EditorAttributes03" src="https://github.com/user-attachments/assets/1065b0f1-74ba-4e92-99cc-d4f2098efaac" />
<img width="1515" height="1059" alt="EditorAttributes04" src="https://github.com/user-attachments/assets/c0af8fa9-c424-47b8-84ea-bc2c5748cc94" />
<img width="1515" height="1059" alt="EditorAttributes05" src="https://github.com/user-attachments/assets/c7db0966-8897-407c-8f97-aaf71304fa09" />
<img width="1515" height="874" alt="EditorAttributes06" src="https://github.com/user-attachments/assets/7394a34d-ba4b-445c-ac69-35cec82e5adf" />
<img width="1515" height="1059" alt="EditorAttributes09" src="https://github.com/user-attachments/assets/0bb3f7d1-568e-4225-ae7c-90a15de9eeef" />
<img width="1515" height="693" alt="EditorAttributes07" src="https://github.com/user-attachments/assets/5634cd74-3e9e-455f-91f0-c1563f9ffa69" />
<img width="1368" height="1304" alt="EditorAttributes02" src="https://github.com/user-attachments/assets/6f17a097-f8e7-4de0-be42-cec604813dc1" />
<img width="1200" height="924" alt="ComplexInspector" src="https://github.com/user-attachments/assets/f20e3018-0d89-472a-8a0b-5b1f15bae130" />
