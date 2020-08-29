# Templater
Program for faster creating projects from templates.

## How to install?
First, clone the repository
```sh
$ github clone https://github.com/uwumouse/Templater.git
$ cd Templater
```

Now, you have to ways to go:
- Using installer (Creates default templates and settings for the program. Program will be added to path).
- Build and [configure](https://github.com/uwumouse/Templater/blob/master/SelfInstalling.md) system by yourself.

Run:
```sh
$ dotnet Installer/Installer.dll
```
And you will get folder with name `Build`.

## How to use?
Templater uses this syntax to create projects:  
```
$ Templater <language>:<template> <projectName>
```
For example:
```
$ Templater node:telegram myTelegramBot
```
creates a Project with Telegram Bot in it.

## How many templates?
If you installed Templater with installer, you will get **4 default templates**:
- **Socket.io + Express** server in NodeJS
- **Telegram Bot** written in NodeJS
- **Express API** with routers
- Simple **Python "Hello, World!"** program.

They're located in **Build/Templates** Folder, you can change them as you want.

## I want more templates!
You easily can create your own templates and locate them anywhere you want.
See [**here**](https://github.com/uwumouse/Templater/blob/master/CreatingTemplate.md) how to **create templates**.

# Which templates have I already created?
To see available templates and potential warnings, run:
```sh
$ Templater list
```
