let presenceConnection;
let chatConnection;

function startSignalR() {

    // Create SignalR connection
    presenceConnection = new signalR.HubConnectionBuilder()
        .withUrl("/hubs/presence", { withCredentials: true })
        .withAutomaticReconnect()
        .build();

    chatConnection = new signalR.HubConnectionBuilder()
        .withUrl("/hubs/message", { withCredentials: true })
        .withAutomaticReconnect()
        .build();

    // Handle initial users list
    presenceConnection.on("GetOnlineUsers", (users) => {
        onlineUsers.push(...users);
        document.dispatchEvent(new CustomEvent("GetOnlineUsers"));
    });

    // Handle new user coming online
    presenceConnection.on("UserIsOnline", (user) => {
        onlineUsers.push(user);
        document.dispatchEvent(new CustomEvent("UserIsOnline"));
    });

    // Handle user going offline
    presenceConnection.on("UserIsOffline", (userId) => {
        const index = onlineUsers.findIndex(u => u.id === userId);
        if (index !== -1) {
            onlineUsers.splice(index, 1);
        }
        document.dispatchEvent(new CustomEvent("UserIsOffline"));
    });

    // Handle new messages
    chatConnection.on("NewMessage", (message) => {
        document.dispatchEvent(new CustomEvent("NewMessage", { detail: message }));
    });

    // Start the connection
    presenceConnection.start()
        .catch(err => console.error(err));

    chatConnection.start()
        .catch(err => console.error(err));
}

function sendMessage(newMessage) {
    return chatConnection.invoke("SendMessages", newMessage);
}