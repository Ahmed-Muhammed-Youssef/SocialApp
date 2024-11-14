

function startSignalR() {
    // Create SignalR connection
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/hubs/presence", { withCredentials: true })
        .withAutomaticReconnect()
        .build();

    // Handle initial users list
    connection.on("GetOnlineUsers", (users) => {
        onlineUsers.push(...users);
        document.dispatchEvent(new CustomEvent("GetOnlineUsers"));
    });

    // Handle new user coming online
    connection.on("UserIsOnline", (user) => {
        onlineUsers.push(user);
        document.dispatchEvent(new CustomEvent("UserIsOnline"));
    });

    // Handle user going offline
    connection.on("UserIsOffline", (userId) => {
        const index = onlineUsers.findIndex(u => u.id === userId);
        if (index !== -1) {
            onlineUsers.splice(index, 1);
        }
        document.dispatchEvent(new CustomEvent("UserIsOffline"));
    });

    // Start the connection
    connection.start()
        .catch(err => console.error(err));
}