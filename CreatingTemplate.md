# Creating Template Group
Here will be explained how to add new template group to a program scope.

Map
- [Creating group](#creating-group-and-templates)
- [Adding group](#adding-group-to-the-program-scope)
- [Language property](#about-languages)
- [Using Template](#using-template)
- [Unregistering group](#unregistering-templates)
- [In Addition](#in-addition)


## Creating group and templates
Let's say I want to create "Hello, World" program in python.  
I need to create template group and store my template there.

I'll do it in my **home** folder:
```sh
$ mkdir Groups
$ cd Groups
```
And then group for the python templates.
```sh
$ mkdir Python
$ cd Python
```
_**You can name folders and locate folders as you want**_  
Now, time to create template.
You need to know, that name of a folder represents name of a template.
I'll name my template `hello`:
```sh
$ mkdir hello
```

**Time for code!**  
I created little script in python, `main.py`:
```python
print("Hello, World!")
```

**Our group is ready for adding to the settings!**


## Adding group to the program scope.
Once you created your group, you will need to add it to the program scope to use templates from there.  
You can do it by running command `add`:
```sh
$ templater add <language>:<name_of_group> <path>
```
So, what are these variables? Let me explain:
- `<language>` - Language of your templates, e.g. `node`, `python`
- `<name_of_group>` - Any name you want to give to your group of templates (Without spaces). Used for determining groups in program.
- `<path>` - Actual path to the Template Groups Folder.

For me it's
```sh
$ tempo add python:PythonTemplates /home/uwumouse/Groups/Python
```

As you remember, I store my python templates in `/home/uwumouse/Groups/Python/`.  
So that's why I provided that path, you must provide path where you created your group folder.

**You should provide path to the folder where you store your group, not to the actual templates!**

## About languages
Languages are used for automatically installing dependencies for the project.  
*Tempo supports only NodeJS* at this moment.  

If you provide `node` as Group language, Tempo will try to find `package.json` in your template and install dependencies from it.  
Basically, that means you don't have to _store `node_modules`_ in your template, it'll be installed by Tempo.


But what to do, if you don't use NodeJS?  
You still can provide any language, but this will be used only in `list` command.  
Or do not provide language at all.  
There's shortcut for this:
```sh
$ tempo add :PythonTemplates /home/uwumouse/Groups/Python
```
Just put semicolon before the name of Group and it'll be without language.
You can always add language to your template, just change property `Language` from `""` to needed language.

## Using Template
Now, to try out your template just run this:
```sh
$ tempo PythonTemplates:hello MyHelloWorld
```
where:
- `PythonTemplates` - Name of a group where your template is stored.
- `hello` - Name of the folder where template is stored.
- `MyHelloWorld` - Name of the your new project.

Answer some questions and... voila! You have fresh project just built from your template!

## Unregistering Templates
To unregister group, run this command:
```sh
$ tempo remove <group_name>
``` 
From this moment no templates will be searched in this group.

## In Addition
Also, if you want to put your templates group into one folder with Templater executable you can type start your path with
`@local`.
You might seen this in `settings.json`:
```json5
{
  // Example from program
  "Path": "@local/Templates/Node"
}
```
This `@local` statement will be changed to absolute path in runtime.