# NetworkedBallBounce
## Unity HTTP enabled ball demo
This is a test unity application for sending HTTP coordinates via asynchronous HttpClient POST requests. I used this chance to explore
using the System.Net networking tools, as I have used unitys own HTTP methods before and wanted to learn more about the .NET framework.

The implementation has been done with several network components as follows. 

### NetworkConfig.cs
This script has fields for selecting the http address endpoint as well as the tick rate of the updates. As per the request this has been
set to once a second. Every second it fires an event that can be subscribed to alerting networking components that they need to synchronise their data.

### NetworkSocket.cs
Slightly oddly named, but this is added to Unity gameobjects to enable them for network synchronisation. This script contains the 
object ID to assign the data to for GET requests, and has some foward thinking fields for deciding client ownership. The Network 
socket works together with SyncComponent to synchronise and send data. Network sockets can take multiple SyncComponents.

### SyncComponent.cs
Base class for synchronsing data over the network. Inherited by TransformSync.

### TransformSync.cs (File contains DataSync class which should be seperated and made into a base class with dependency injections(?)
This is a type of sync component that defines its data types for syncrhonisation. It subscribes to NetworkConfig sync calls as all
sync components should, and then calls for a post request with its data.

Besides this there is some simple UI using textmesh and some graphical facelifts with textures provided by MegaScans. I've also included some audio to keep it while I tested.

On application start, 8.8.8.8 is pinged to check for an internet connection, and then once verified we create a connection with the endpoint
and start sending data.

The one hitch I ran into concerns how the native system handles its Async calls from the Httpclient. There is a DNS lookup and TLS handshake
which for some reason always takes exactly 20s when connecting/sending to a new endpoint. Using debug statements shows the delay happening after calling `Ping.SendPingAsync()` or `HttpClient.PostAsync()` when either uses a new address.
If I were to continue this I would use wireshark to check the outgoing and 
incoming packets and then implement the `IHttpClientFactory` interface (if possible, as unity .NET Framework support is often limited for a more robust integration that respects DNS changes.
This is also important as to not exhaust sockets by simply doing a `using(new HttpClient)` call for every request.

Overall though this has been a great exercise! If I were doing this for my own game though I would probably use UDP for faster throughput.
