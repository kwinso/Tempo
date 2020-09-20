# Tempo
Tempo is a tool for creating projects from templates. 

Map
- [Introduction](#how-does-it-work)
- [Installation](#installation)
- [Usage](#usage)
- [Base Templates](#how-many-templates)

## How does it work?
Tempo uses **template groups** to manage different templates.  
_**Template Group**_ is a folder in your system that contains templates for your projects.  

Let's say you have to create NodeJS Socket Server, and you want to repeat this process later.  
You will need to init project, create same files, install same deps everytime. So, with **Tempo** you can just create template once and use it every time you want to create project.  

Tempo stores templates paths in **settings.json**. However, you can not to worry about this file, you can manage your templates with Tempo itself. 
But if you want to know how it works in depth, this is not to hard.

Basic template group looks like that:
```json
{
  "Language": "node",
  "Name": "NodeTemplates",
  "Path": "/home/uwumouse/NodeTemplates"
}
```
What are these fields? Let me explain:
- `Language` - Language for the templates. Used to install dependencies. NodeJS is recognized as `node`.
- `Name` - Name of Template Group. You can name this field as you want. (Required)
- `Path` - Actual path to the Templates. (Required)

This is how `settings.json` looks in real life:
```json
{
  "TemplateGroups": [
      {
        "Language": "node",
        "Name": "NodeTemplates",
        "Path": "/home/uwumouse/NodeTemplates"
      }
  ]
}
```
Yes, you're right. Template Groups are stored in array `TemplateGroups`.  

But, what's a **template**?
Template is just a folder with files in your system. _Name of a template_ is a _Name of a Folder_.
With that said, you can store everything that has to be copied many times in your templates. But there's some features for programmers, like auto-installing dependencies.

Let's say, I have folder `/express` in my `/home/uwumouse/NodeTemplates`.  
That means I have `express` template in my `Node-Templates` Template Group.

## Installation
Before you try out Tempo, you need to install it. Tempo has it's own installer.  
You will get fully configured Tempo Program. And also some default templates to try out.

Firstly, clone the repository
```sh
$ git clone https://github.com/uwumouse/Tempo.git
$ cd Tempo
```
Now, run the installer. (Tempo will be installed to the `Tempo/Build`. Choosing own folder will be added in next updates.)  
```bash
# Linux
$ chmod +x Setup/linux.sh && sudo Setup/linux.sh
# Windows
# Sorry, this is not implemented yet.
```

On Linux, you will have to run it as root, because it'll be installed in `/opt` and `/usr/local/bin/tempo` file will be created.

## Usage
Tempo uses this syntax to create projects:  
```
$ tempo @<template_group>/<template> <projectName>
```
For example:
```
$ tempo @base_node/telegram myTelegramBot
```
creates a Project with Telegram Bot in it.  
This `base_node` group gets _installed with Tempo_.

With `tempo list` you can see all available templates.
Example:
```sh
$ tempo list
> [INFO] Template group base_node at @local/Templates/Node
          Template: api, language: node
          Template: telegram, language: node
          Template: socket, language: node
```
`@local` means that the path is relative to the program executable (**Actual executable, not the shell script!**).

## How many templates?
Tempo ships 2 base template groups:
- base_node
    - **Socket.io + Express** server in NodeJS (Template Name: socket)
    - **Telegram Bot** written in NodeJS (Template Name: telegram)
    - **Express API** with routers (Template Name: api)
- base_py
    - Simple **Python "Hello, World!"** program (Template Name: hello)

They're located in `<InstallationDir>/Templates` folder, you can change them as you want, but it's recommended to use Tempo commands to avoid errors.

## I want more templates!
See [**here**](https://github.com/uwumouse/Tempo/blob/master/TemplatesInDepth.md) how to **work with templates**.