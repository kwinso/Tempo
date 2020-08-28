const app = require('express')();
const server = require('http').createServer(app);
const io = require('socket.io')(server);

app.get("/", (req, res) => {
    res.send("Hello, World! This is my socket.io application!");
});

io.on('connection', socket => { 
    console.log("We got a new connection!");
    
    socket.on("disconnect", () => "Socket disconnected");
});

const port = 3000;
app.listen(port, () => console.log(`Socket.io server is up on http://localhost:${port}`));