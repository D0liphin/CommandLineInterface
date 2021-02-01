# CommandLineInterface
A C# class library for easily creating a CLI.

## Motivation
I wanted an easy way to execute functions from the command line in a closed environment (so, without using `EvaluateExpression` etc.).

# Breaking down a command
All expressions are in the format `<name> <parameters>`.   
Commands, like methods, have position arguments and keyword arguments.

## Command Names
Command names must be contain only alphanumeric characters, underscores and hyphens.  

### Examples

- `command_name`
- `CommandName`
- `0commandname`
- `command-name`
- `commandname0`

It is recommended that command names are all lowercase and as short as possible.  
E.g. `msg` is better than `send_message` and `ls` is better than `list_all_items`.

## Positional Arguments
Positional arguments work just as you'd expect.

Arguments are either strings or literals. 
- **literals** contains alphanumeric characters, underscores, hyphens, forward-slashes and periods. 
They cannot lead with a hyphen.  
- **strings** are enclosed **only by double quotes**. They can contain any character. `\\` will evaluate to `\` and `\"` will evaluate to `"`.

### Examples

- `command arg1 arg2 arg3`
- `command "a string argument" arg2 arg3`
- `command "a string \"something in quotes\"" arg2 "argument number 3"`


## Tags

Tags lead with a hyphen. They can contain alphanumeric characters, hyphens and underscores.  
Any arguments that follow a tag are considered to be part of that tag's 'scope' and apply to that tag.  
Tags are referenced without the first hyphen `-some-tag` would be called `some-tag` and `---some-tag` would be called `--some-tag`.  
as a general rule, tags should be spaced with hyphens.  
Tags do not need arguments.

### Examples

- `command arg1 -tag arg1 arg2 arg3` `tag1` has 3 arguments, `arg1`, `arg2` and `arg3`.
- `command arg1 -tag arg1 "some string argument" -tag2 -tag3 anotherArgument`

# `Parser.Parse(string plainTextCommand)`

using `Parser.Parse(string plainTextCommand)` returns a `CommandDetails` object that represents a parsed command.

# `CommandDetails`

`CommandDetails` objects contain several `readonly` attributes.  

## `string[] Args`

An array of all the arguments the user has entered. All arguments are strings.  
This measns that `argument` and `"argument"` are both stored the same.

### Examples

```
> command arg1 arg2 arg3

command [
  "arg1",
  "arg2",
  "arg3"
]
```
```
> command "argument one" arg2 "argument number 3"

command [
  "argument one",
  "arg2",
  "argument number 3"
]
```




