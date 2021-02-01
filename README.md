# CommandLineInterface
A C# class library for easily creating a CLI.

## Motivation
I wanted an easy way to execute functions from the command line in a closed environment (so, without using `EvaluateExpression` etc.).

## Breaking down a command
All expressions are in the format `<name> <parameters>`.   
Commands, like methods, have position arguments and keyword arguments.

### Command Names
Command names must be contain only alphanumeric characters, underscores and hyphens.  

#### Examples
```
command_name
CommandName
0commandname
command-name
commandname0
```

It is recommended that command names are all lowercase and as short as possible.  
E.g. `msg` is better than `send_message` and `ls` is better than `list_all_items`.

### Positional Arguments
Positional arguments work just as you'd expect.

Arguments are either strings or literals. 
- **literals** contains alphanumeric characters, underscores, hyphens, forward-slashes and periods. 
They cannot lead with a hyphen.  
- **strings** are enclosed **only by double quotes**. They can contain any character. `\\` will evaluate to `\` and `\"` will evaluate to `"`.

#### Examples
```
command arg1 arg2 arg3
command "a string argument" arg2 arg3
command "a string \"something in quotes\"" arg2 "argument number 3"
```

