Test Setting: left arm + phone

New Object - Phone: added UI scripts, pending actual screen page sprites but current code will allow displaying faceID screen -> home page screen -> match app screen -> switch person or matched screen, colliders added for interactions

Mechanics/Use Case:

Assuming phone on right hand and hand rotates to rotate phone, FaceID will succeed once phone screen front face player camera
Using left hand pointer finger, press on screen for opening app + swipe on screen to perform app action
Next:

add screen images to UIcontrol as sprites (link in Inspector) - no need adjust size should fit into UI image
Merge to arm rotation and phone pickup complete - phone on hand rather than on its own
Long File Log Below:

All files added:

Scenes: PhoneTest
Script: DebugCollider, FingerTouch, PhoneScreen, PhoneTestHandMove (delete), PhoneUIController, TestPhoneMove (refer but delete)
Inputs: TestControls: HandControls (delete), PhoneControls (delete, no need refer?)
Phone

rigidbody (kinematic, used for movement but no need in use case -- tried making collision but fail, keep for future) -- need add to prefab
TestPhoneMove.cs (no need later but refer, attached to TestControls.inputactions PhoneControls input map -- refer for match arm rotations and how phone x is opposite from hand/player x)
PhoneUIController.cs (pending sprites)
PhoneScreen.cs (for starting screen and FaceID machanics) -> (attach main camera (head view) into player camera)
++ AppButtonArea (tag "PhoneButton", box collider, isTrigger, DebugCollider just for visual in Scene)
++ SwipeArea (tag "SwipeArea", box collider, isTrigger, DebugCollider just for visual in Scene)
++ ScreenCanvas (render mode = world space)
++++screenImage (a UI Image, to display sprite onto)
LeftArm

FingerTip (sphere collider, FingerTouch.cs for pressing screen - link Phone object to Phone, added rigidbody (kinetic) for testing collision (failed) and no actual use)
Arm:LeftHandRig:target: HandMovement.cs removed and replaced with PhoneTestHandMove.cs (was not actually used or needed, easy switch back, linked to TestControls.inputactions but again, not used)
All files added:

Scenes: PhoneTest
Script: DebugCollider, FingerTouch, PhoneScreen, PhoneTestHandMove (delete), PhoneUIController, TestPhoneMove (refer but delete)
Inputs: TestControls: HandControls (delete), PhoneControls (delete, no need refer?)
