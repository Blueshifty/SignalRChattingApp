Vue.config.devtools = true;

var vue = new Vue({
    el: "#vue-wrapper",
    delimiters: ["${", "}}"],
    data: {
        connection: null,
        userName: null,
        password: null,
        viewState: "login",
        errorMessage: null,
        rooms: [],
        messages: [],
        newRoomName: null,
        onlineUsers: [],
        currentRoom: null,
        newMessage: null,
    },
    methods: {
        startConnection: async function () {
            try {
                await this.connection.start();
                console.log("SignalR Connected.");
            } catch (err) {
                console.log(err);
                setTimeout(this.startConnection, 5000);
            }
        },
        loginRegister: async function (login) {
            let url = 'api/Auth/' + (login ? 'Login' : 'Register');
            await axios.post(url, {
                username: this.userName,
                password: this.password
            }).then(response => {
                sessionStorage.setItem("token", response.data.data.accessToken);
                sessionStorage.setItem("token-expiration", response.data.data.expiration);
                sessionStorage.setItem("refreshToken", response.data.data.refreshToken);
                this.axiosConfig = {
                    headers: {
                        Authorization: "Bearer" + sessionStorage.getItem("token")
                    }
                };
                this.initializeSignalRConnection();
                this.viewState = "dashboard";
            }).catch(error => {
                this.errorMessage = error.response.data.message;
                setTimeout(() => {
                    this.errorMessage = null;
                }, 2000);
                console.log(error.response);
            });
        },
        initializeSignalRConnection: async function () {
            this.connection = new signalR.HubConnectionBuilder()
                .withUrl("https://localhost:5001/chatHub",
                    {accessTokenFactory: () => sessionStorage.getItem("token")})
                .configureLogging(signalR.LogLevel.Information)
                .build();

            this.initializeSignalRMethods();

            this.connection.onclose(this.startConnection);

            this.startConnection();
        },
        initializeSignalRMethods: function () {
            this.connection.on("onError", (method, message) => {
                this.errorMessage = "Error : " + method + message;
            });
            this.connection.on("getRooms", (username) => {
                this.connection.invoke("GetRooms").then(response => {
                    this.rooms = response;
                });
            });
            this.connection.on("addChatRoom", (room) => {
                this.rooms.push(room);
            });
            this.connection.on("newMessage", (message) => {
                this.messages.push(message);
            });
        },
        roomActions(action, room) {
            this.connection.invoke(action, room).then(response => {
                console.log(action, response);
                switch (action) {
                    case 'CreateRoom':
                        let modal = new bootstrap.Modal(document.getElementById('roomModal'));
                        modal.hide();
                        break;
                    case "Join":
                        this.roomActions("GetMessageHistory", room);
                        this.roomActions("GetOnlineUsersInRoom", room);
                        this.currentRoom = room;
                        break;
                    case "GetMessageHistory":
                        this.messages = response;
                        break;
                    case "GetOnlineUsersInRoom":
                        this.onlineUsers = response;
                }
            });
        },
        sendMessage() {
            this.connection.invoke("SendToRoom", this.currentRoom, this.newMessage).then(response => {
                this.newMessage = null;
            });
        }
    },
    created: function () {
    },
    mounted: function () {
    },
});