require("dotenv").config(); // Loading variables from .env

const Telegraf = require("telegraf");

const bot = new Telegraf(process.env.BOT_TOKEN);

bot.start(async ctx => {
    await ctx.reply("Hello, I'm a telegram bot!");
});

bot.on("message", async (ctx) => {
   await ctx.reply("Oh don't understand you."); 
});

bot.launch().then(() => console.log("Telegram Bot Started"));
