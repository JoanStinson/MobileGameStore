Camera Aspect Ratio Scaler
by DitzelGames (contact: xxxditzelxxx@gmail.com)
Made in Unity: "2017.2"



Contents of the asset:
CameraAspectRatioScaler.cs	-- The main script which should be placed on the camera.



About the asset:
This package helps your project to support different aspect ratios and resolutions. 
The script can be directly attached to a camera and zooms in or out depending on the aspect ratio.



Quick start:
- Add the script "CameraAspectRatioScaler" to the camera.

The camera should work right away. You can set the reference resolution for your primarily targeted device or the resolution you have in the editor. The resolution will be used to calculate an aspect ratio and modify the camera position to fit the screen content accordingly. If you are not satisfied with the result, you can change the zoom factor to change the strength of the effect.



Methods and Features:
OriginPosition:	You can use the field OriginPosition to reposition the camera. The Update method will adjust the position depending on the aspect ratio. Make sure that the script is called after the repositioning of the camera. You can edit this in Edit > Project Settings > Script Execution Order.

Code and Compatibility:
If you reposition the camera in a FixedUpdate and observe some shaky movement, you will have to change Update to FixedUpdate in CameraAspectRatioScaler.cs  .


Feel free to contact me if you need assistance, have any questions or would like to report bugs.