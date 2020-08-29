# Creating Templates
Here will be explained how to add templates to a program scope.

## Create Settings File
`settings.json` is a file responsive for managing templates folder.  
Create this file and write:
```json
{
  "Templates": [
  ]
}
```
So, `Templates` will store, wow... _Templates!_
Now, you need to add first template.

## Creating Templates Folder.
Let's say I want to create "Hello, World" program in python.  
I need to create folder where I will store my templates.

I'll do it in my **home** folder:
```sh
$ mkdir Templates
$ cd Templates
```
And then folder for the python templates.
```sh
$ mkdir Python
$ cd Python
```
_**You can name folders and locate folders as you want**_  
Now, time to create template.
First of all, create folder, name of a folder represents name of a template.  
I'll name my template `hello`:
```sh
$ mkdir hello
```

**Time for code!**  
I created little script in python, `main.py`:
```python
print("Hello, World!")
```

**Our template is ready for adding to the settings!**


## Adding template to the program scope.
Do you remember that file `settings.json` in program directory?  
Add to the `Templates` array object like this:
```json
{
  "Language": "<language of app>",
  "Path": "<path to the folder with the templates>"
}
```
For me it's:
```json
{
  "Language": "python",
  "Path": "/home/uwumouse/Templates/Python/"
}
```
As you remember, I store my python templates in `/home/Templates/Python/`.  
So that's why I provided that path, you must provide path where you created your template.

**You should provide path to the folder where you store templates, not to the actual templates!**

## Be Careful With Languages
Now Templater supports only these languages:
- NodeJS - `node`
- Python - `python`

Languages are used for installing dependencies for the project.  
So, you can not to store, for example, `node_modules` in your template. You will need only `package.json` to create project.  
Unfortunately, these feature works _only for NodeJS_, but another languages will be implemented soon.

But what to do, if you got unsupported language?
You can provide `no-lang` property in `"Language"` field and Templater will skip language and just create project from template.

## Using Template
Now, to try out your template just run this:
```sh
$ Templater python:hello MyHelloWorld
```
where:
- `python` - Language you provided in settings
- `hello` - Name of the folder where template is stored.
- `MyHelloWorld` - Name of the your new project.

Answer some questions and... voila! You have fresh project just built from your template!

## In Addition
Also, if you want to put your templates folder in the one folder with Templater executable you can type this:
```json
{
  "Path": "@local/MyLocalTemplates/"
}
```
You could seen it in Build from Installer.
This `@local` statement will be changed to absolute path in runtime.