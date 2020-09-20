const express = require("express");
const cors = require("cors");
const morgan = require("morgan");
const helmet = require("helmet");

// Connecting variables from .env
require("dotenv").config();

const app = express();

app.use(cors({
    origin: "*",
}));
app.use(morgan("combined")); // Logs for requests
app.use(helmet()); // Security
app.use(express.json()); // JSON bodyparser

app.get("/", (req, res) => {
    res.send("Hello, World!");
});

const apiRouter = require("./routers/api/v1");

app.use("/api/v1", apiRouter);

const port = process.env.PORT;

app.listen(port, () => console.log(`API Server is up on http://localhost:${port}`));