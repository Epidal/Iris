![CABAL GlobalMgrSvr Communication Library](http://i35.tinypic.com/21bjiur.png "Logo")

Iris is a GlobalMgrSvr Communication Library.  In layman's terms, it lets you communicate with your server's GlobalMgrSvr and do things such as:
* Obtain information about the server, including how many people are logged on.  You can even see how many people are in each map.
* Send a GM message by mail.  Because this is done by directly communicating with the GMS, the GM mail appears on your users' screens immediately after sending it.
* Broadcast a message.  This message will appear at the top-center and bottom-right of all connected users' screens.
* Disconnect all players from a channel.  This is especially handy if you want to restart your server, or take it down for maintenance.

To use Iris, simply reference Iris.DLL in your application, create an instance of the Iris.Communication class, then give it an IP and port to connect to.  Here's an example in C#:

```csharp
var iris = new Iris.Communication();
iris.Connect("192.168.1.2", 38170);
```

Simple, no?  By default, Iris sends a "heartbeat" packet every 30000 milliseconds (30 seconds).  You can, however, specify a custom interval.  You can do so by passing the desired interval in milliseconds when you create an instance of Iris.Communication:

```csharp
var iris = new Iris.Communication(5000);  // 5 second heartbeat interval
```

Iris can also notify your application when certain events happen, like receiving the channel list, for example:

```csharp
var iris = new Iris.Communication(5000);
iris.Events.OnReceiveServerList += new iris.EventHandler.ServerListHandler(myApp_OnReceiveServerList);
iris.Connect("192.168.1.2", 38170);
```

Iris can easily send both GM messages and broadcasts using the same method:

```csharp
iris.SendMessage(6, 3, "my message");  // Broadcasts "my message" to channel 3 on server 6.
iris.SendMessage(6, 0, "msg");  // BC's "msg" to all channels on server 6.
iris.SendMessage(4, new byte[] { 2, 4, 6 }, "msg");  // BC's "msg" to channels 2, 4, and 6 on server 4.

iris.SendMessage(5, 2, "title", "message");  // Sends a GM message with the title "title", and containing the message "message" to channel 2 on server 5.

// You can use the same sort of principles with Broadcasting as GM messages to send messages to multiple/all channels.
```

Iris can also disconnect all players from a specified channel.  This helps prevent accidental rollbacks or data loss when taking your server down for maintenance.  Just disconnect everyone before you shut your server down, and you're good to go:

```csharp
iris.DisconnectPlayers(6, 2);  // Disconnects all players in channel 2 on server 6.

// You can do the same here as with Broadcasting to disconnect from multiple channels.
```