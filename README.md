# LightsPacketAction

### Windowed
![Windowed](/Images/Window.png?raw=true)

### Config Editor
![Edit Button](/Images/ConfigEditorButton.png?raw=true)
![Config Editor](/Images/ConfigEditor.png?raw=true)

### Full Screen
![Fullscreen](/Images/Fullscreen.png?raw=true)
![FullscreenToggle](/Images/FullscreenToggle.png?raw=true)
![FullscreenMenu](/Images/FullscreenMenu.png?raw=true)

## SHORTCUTS
- Ctrl + M: toggles buttons visibility.
- Esc: opens the menu overlay when in full screen windows.
- Double Tap Right Corner: closes full screen windows.

## SCENARIO
- Run application exe.
- Setup config with the number of buttons and messages you want.
- Fill out server name or IP address.
- Fill out port number (must be numbers only).
- Fill out image path by typing or browsing.
- Click launch. 
- Click buttons to send message.

## MESSAGE
- Messages are editable in the configuration editor, "\r" end carriage is added at the time of button press.

## USING PACKET RECEIVER
![Packet Receiver](/Images/PacketReceiver.png?raw=true)
- Run PacketReceiver.exe via command line by providing exe path followed by the receiving IP address and the port.
- Example of command '"C:\Users\Torrin\Desktop\PacketReceiver.exe" 172.16.4.351 13000'.
- Will display the messages received by this port.
