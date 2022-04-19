# LightsPacketAction
![Windowed](https://user-images.githubusercontent.com/43557445/75826974-a856b400-5d76-11ea-8732-e63ef70b6077.PNG)

![Fullscreen](https://user-images.githubusercontent.com/43557445/75827195-18fdd080-5d77-11ea-93e0-87e598d47ff5.PNG)
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
- Run PacketReceiver.exe via command line by providing exe path followed by the receiving IP address and the port.
- Example of command '"C:\Users\Torrin\Desktop\PacketReceiver.exe" 172.16.4.351 13000'.
- Will display the messages received by this port.
