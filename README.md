libpd (pure data) with unity 4.x

### status: ###
* working with unity pro and unity free*
* Windows 32bit & 64bit
* OSX
* Android

** You need to copy libpdcsharp.dll where Unity.exe is installed or where your game executable is located in case of a pc standalone build. This hasn't been tested on OSX.

The following files/folders in the Unity project are of relevance:
* LibPdFilterRead.cs: Initialises libpd, sets up audio callback, opens patches, etc. Best attached to the camera or an empty game object. Make sure there is only ONE instance of this script active in a scene. 
* GUITextScript.cs: Example that shows how to receive messages from Pd
* GUIToggleScript.cs: Example that shows how to send messages to Pd
* 01\_LibPd_Basic.unity: A scene that shows the above working
* 02\_LibPd_ADC.unity: A scene that shows how to pass audio into Pd using adc~

The whole libpd API hasn't been tested, but will happen at some point in the near future. Feel free to test and report any issues. 

### setup: ###
To ensure cross-platform compatibility, place all patches, audio files and externals in Assets > StreamingAssets > PdAssets. Externals have been tested and work fine on OSX. Other platforms are pending.

### known issues: ###
* Unpredictable SIGILL when using Unity Editor
* Android: Can currently only open patches that have no dependencies. Pd won't find externals or audio files within the PdAssets folder. This is an Android OS related issue. Might need to use one of the Android APIs to fix this.

### LibPd4UnityTools ###
Set of tools to facilitate the communication between LibPd and the Unity game engine
https://github.com/Magicolo/LibPd4UnityTools

### thanks to: ###
* Miller Puckette (pure data)
* Peter Brinkmann (libpd)
* Tebjan Halm (libpdcsharp)
* Jean-Sébastien Leduc (bridge)
* Magicolo (LibPd4UnityTools)
* Patrick Sebastien || http://www.workinprogress.ca || (all the initial work and Windows support)
* Varun Nair || http://www.re-sounding.com/ || (OSX and Android) 
* Peter Cardwell-Gardner || http://www.thefuntastic.com/ || (dll not found fix)

### alternative: ###
https://github.com/hagish/kalimba

### howto: ###
* Windows: install mingw legacy (http://sourceforge.net/projects/mplayer-ww/files/MinGW-full/) and compile libpd (https://github.com/libpd/libpd)
make csharplib. copy libs/libpdcsharl.dll / pthreadGC2.dll to Assets/Plugins
* OSX: More info coming soon (libpdcsharp.bundle included with this project works fine)
* Android: More info coming soon (libpdcsharp.so included with this project works fine)
