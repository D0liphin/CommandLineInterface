# CommandLineInterface
A C# class library for easily creating a CLI.

Using `CommandLineInterface.ParseCommand` should parse a command described with the below specification.

## `<command_name>`
Command names can be alphanumeric as well as contain underscores and hyphens. 

### Examples
```
command_name
commandName
CommandName
command_name123
123command_name
command-name
-command-name
```

It is recommended to use all-lowercase abbreviations for commands 

## `<command_name> <arg>`

ok i can't be bothered writing this rn
