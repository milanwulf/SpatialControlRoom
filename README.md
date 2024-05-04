
# Spatial Control Room (for Meta Quest 3)

Spatial Control Room was developed as part of my bachelor's thesis. It is a mixed-reality operating concept for use in production control rooms (PCR), which are mainly found in television productions. The focus was on developing an alternative to conventional vision mixers and monitor walls. I came up with the idea when I was working on a production myself. I noticed that although augmented reality and similar technologies are already being used in film and television to add virtual studios or additional information in sports, immersive technologies have so far mainly been used as a medium and not as a tool for actual content creation. I was therefore interested in whether the existing processes in a production control room could be improved and optimized through the integration of Extended Reality.

<img src="https://github.com/milanwulf/SpatialControlRoom/assets/56889501/2e095a42-3bb8-4bac-adb4-2bd743129448" width="30%"></img> 
<img src="https://github.com/milanwulf/SpatialControlRoom/assets/56889501/416a705d-0ed8-44e1-8214-5ac5d73e20b7" width="30%"></img> 
<img src="https://github.com/milanwulf/SpatialControlRoom/assets/56889501/aa2176c9-4ac0-4cd4-bb40-05c472d054b6" width="30%"></img>


## Features

Spatial Control Room connects to a computer running OBS (Open Broadcaster Software) and enables the simultaneous streaming of 10 individual scenes to a Quest 3 headset, along with the preview and program buses. Like professional vision mixers, the program bus (marked with a red frame) displays the outgoing video signal, while the preview bus (marked with a green frame) shows the source targeted for the next cut. Instead of pressing buttons on a physical control panel, users can simply tap a video panel to mark it as preview. With a hand gesture resembling scissors, users can signal a cut, causing the video panel to switch to the program state.

<img src="https://github.com/milanwulf/SpatialControlRoom/assets/56889501/d15ad517-35c5-4196-a3ce-242432ba8abf" width="30%"></img>

The different panels, each displaying various scenes, can be freely positioned and scaled within the environment, offering a clear advantage over traditional physical monitor walls. To improve ergonomics, panels automatically turn towards the user or follow their head position if desired. Another benefit is that the video panels can be duplicated infinitely, allowing placement in various locations, such as directly on cameras, thereby saving on additional hardware costs. The aspect ratio can be adjusted, which is particularly useful for productions that are also broadcast on social media channels with vertical or square video formats.

All basic functions of the application are controlled via a fold-out action bar. This includes a menu for placing individual scenes, virtual labels for labeling hardware, and controls to start or stop OBS's recording and streaming functions. Furthermore, various passthrough modes can be selected to change the real environment, enhancing concentration.

<img src="https://github.com/milanwulf/SpatialControlRoom/assets/56889501/e0e5bf0e-24e3-40d5-9e64-da84d12f0a3c" width="30%"></img> 


## Installation
### 1. Computer preperation
**Note:** Linux and MacOS versions and newer releases should also work, but have not been tested.
- Download and install OBS Studio (Open Broadcaster Software): https://obsproject.com/download
    
    *Tested with OBS Studio **windows** version [30.0.2](https://github.com/obsproject/obs-studio/releases/tag/30.0.2)*

- Download and install OBS-NDI and the NDI Runtime as described here: https://github.com/obs-ndi/obs-ndi
    
    *Tested with NDI Runtime 6 **windows** version [NDIRedistV6](http://ndi.link/NDIRedistV6) (is recognized by some browsers as an unsafe download)*
    
    *Tested with OBS-NDI **windows** version [obs-ndi-4.13.1-windows-x64](https://github.com/obs-ndi/obs-ndi/releases/download/4.13.1/obs-ndi-4.13.1-windows-x64-Installer.exe)*
- Start OBS and activate the "Studio Mode" with the button in the lower right corner ([more about Studio Mode](https://obsproject.com/wiki/OBS-Studio-Overview#studio-mode))
- Click on Tools in the top menu bar and open the NDI output settings, here the main output and the preview output must be activated
- Click on Tools in the top menu bar, open the WebSocket server settings, enable the server and click on Apply (click on Show Connect Info to see your credentials)
- Click on Scene Collection in the top menu bar and import the .json file found on [Latest Releases](https://github.com/milanwulf/SpatialControlRoom/releases/latest)
- Add any number of sources to Scene 1-10, such as webcams, capture cards, or videos
    
    *"Spatial Control Room Feeds" must be the lowest in the list and should not be changed!*

    *To display video sources correctly in the Quest App, uncheck "Hide source when playback ends" in its properties; it is also a good idea to enable "Restart playback when source becomes active".*


### 2. Quest Headset preperation
**Note:** The application has been developed for the Meta Quest 3, a Meta Quest Pro or newer headsets with Meta Horizon OS should also work, but have not been tested. All previous models may not have enough power to transmit more than one NDI stream.
- Make sure the headset's developer mode is enabled ([instructions can be found here](https://developer.oculus.com/documentation/native/android/mobile-device-setup/))
- Install the .APK found on the [Latest Releases](https://github.com/milanwulf/SpatialControlRoom/releases/latest), the easiest way is to use the [Meta Quest Developer Hub](https://developer.oculus.com/meta-quest-developer-hub/)
- Connect the headset to the same network as the computer running OBS
    
    *The computer should be connected with an Ethernet cable; for best performance use a modern Wi-Fi 6E router*
- Start the APP, open the action bar and click the gear icon to enter your WebSocket credentials and select the NDI streams as follows:

    `Feed Group 1 = YourComputerName (Feed1_Group)`/
    `Feed Group 2 = YourComputerName (Feed2_Group)`/
    `Feed Group 3 = YourComputerName (Feed3_Group)`

    Tab on "Connect". If the connection is successful, a message should appear below the action bar.

  <img src="https://github.com/milanwulf/SpatialControlRoom/assets/56889501/298a0165-88d4-4be9-b27c-a5dce26a74c9" width="50%"></img>

## Ideas and planned features
- Aids for aligning panels (snapping)

    <img src="https://github.com/milanwulf/SpatialControlRoom/assets/56889501/3d72fe95-0914-4800-99e4-9ed460099a10" width="50%"></img>

- Grouping of panels
- Saving and loading of arrangements using spatial anchors
- Clean-up function that automatically arranges panels logically
- Automatic scaling of UI elements as distance increases

    <img src="https://github.com/milanwulf/SpatialControlRoom/assets/56889501/687fd5ea-51d0-44e2-ab3e-802914e319f5" width="20%"></img>

- Object tracking for linking panels to physical objects like cameras

    <img src="https://github.com/milanwulf/SpatialControlRoom/assets/56889501/738920bf-c576-498d-b08b-c732c57bfc32" width="20%"></img>
- Option to show the title-safe area
- Displaying of audio levels and intercom integration

    <img src="https://github.com/milanwulf/SpatialControlRoom/assets/56889501/55fe8faf-91fd-4574-b63b-5d14cd55b52f" width="20%"></img>
- Integration of advanced tools such as vectorscope or waveform monitor
- Intercom integration
- Seperate workspaces for video, audio and light
- Multi-user support

    <img src="(https://github.com/milanwulf/SpatialControlRoom/assets/56889501/b8363f69-aa1a-4de8-af41-b05771b34b78" width="20%"></img>

## Problem Solving

#### Preview and program bus are not transmitted

Make sure that Main Output and Preview Output are checked in the NDI Output Settings panel in OBS.
Open the Spatial Control Room Feed scene, go to the Feed1 group and double-click the Preview source. In the properties, make sure the correct source name is selected (YourComputerName (OBS Preview)), do the same for "Program" (YourComputerName (OBS Program)).

#### Performance on the headset is very poor 

Streaming and receiving NDI requires a good network connection and places a heavy load on the CPU. Use a fast WiFi 6E standard router (e.g. TP-Link Archer AXE75) and make sure your computer is connected directly to it with an Ethernet cable. Make sure the headset is connected to the fastest connection (6GHz), as modern routers often create multiple WiFi networks for different bands. It may also help to block other devices from connecting to the network to free up bandwidth. 

If you do not need all 10 scenes, it is a good idea to disable the NDI streams you do not need. To do this, click the gear icon and set feed group 3 and feed group 2 to None. This should give you a significant performance boost by reducing the CPU load.

**Note:** Feed Group 3 contains scenes 7-10, Feed Group 2 contains scenes 3-6, Feed Group 1 contains scenes 1-2 and the Preview and Program feed. 

You can also set the CPU level of the Quest to level 5 by sending the following Adb command via the Meta Quest Developer Hub: `adb shell setprop debug.oculus.cpuLevel 5` ([more about ADB](https://developer.oculus.com/documentation/native/android/ts-adb/))

Also check whether your computer has enough power to run OBS smoothly.

## Contact
I studied user interface design and am a trained media designer with a strong interest in XR and spatial computing. I am also interested in 3D motion design and digital music production. For questions about the project or a general exchange I am available here:

[LinkedIn](https://www.linkedin.com/in/milanwulf/)

[Instagram](https://www.instagram.com/milan_wulf/)

[Website](https://milanwulf.com/)
