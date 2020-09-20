const router = require("express").Router();

const puppiesRouter = require("./puppies");

router.use("/puppies", puppiesRouter);

module.exports = router;