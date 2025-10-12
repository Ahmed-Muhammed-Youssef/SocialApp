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




const ToastService = (() => {
    /**
     * Display a toast message.
     * @param {string} message - The message to display.
     * @param {string} type - The type of the message ('success', 'error').
     */
    function showToast(message, type) {
        const toastId = `toast-${Date.now()}`;
        const bgColor = type === 'success' ? 'bg-success' : 'bg-danger';

        const toastHtml = `
            <div id="${toastId}" class="toast align-items-center text-white ${bgColor}" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body">
                        ${message}
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
            </div>
        `;

        const toastContainer = document.getElementById('toast-container');
        toastContainer.insertAdjacentHTML('beforeend', toastHtml);

        const toastElement = document.getElementById(toastId);
        const toast = new bootstrap.Toast(toastElement, { delay: 3000 }); // 3-second delay
        toast.show();

        toastElement.addEventListener('hidden.bs.toast', () => {
            toastElement.remove();
        });
    }

    return {
        showToast
    };
})();