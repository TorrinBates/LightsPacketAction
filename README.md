# LightsPacketAction

	SHORTCUTS
=========================
- Ctrl + M: toggles buttons styles
- Esc: closes full screen window
- Double Tap Right Corner: closes full screen window

	SCENARIO
=========================
- Run application exe
- Fill out server name or IP address
- Fill out port number (must be numbers only)
- Fill out image path by typing or browsing
- Click launch 
- Click buttons to send message

	MESSAGE
=========================
- Messages are formatted as "Button#\r" so an example of how the message might look is "Button21"

 USING PACKET RECEIVER
=========================
- Run PacketReceiver.exe via command line by providing exe path followed by the receiving IP address and the port
- Example of command '"C:\Users\Torrin\Desktop\PacketReceiver.exe" 172.16.4.351 13000'
- Will display the messages received by this port
