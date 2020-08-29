const router = require("express").Router();

router.get("/", (req, res) => {
    res.send({ message: "Do you like puppies? They are cute!" });
});

module.exports = router;