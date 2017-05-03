# Jasily.Frameworks.Cli-CSharp

This is a command line interface framework for .Net.

**The engine is write on .net standard.**

## Example for **.Net Framework**

First: Create a command class with `public` methods.

``` cs
[CommandClass(IsNotResult = true)]
class Cmd
{
    public int Add(int value1, int value2) => value1 + value2;

    public Cmd Next(int value) => new Cmd();
}
```

Second: Build engine and `Fire` the `Cmd` instance, then execute the arguments.

``` cs
static void Main(string[] args)
{
    new EngineBuilder().InstallConsoleOutput().Build().Fire(new Cmd()).Execute(args);
}
```

Finally: Compile and execute it.

``` cmd
ConsoleApp2.exe
// Usage:
//    Commands:
//       Add
//       Next
ConsoleApp2.exe Next
// Usage:
//    Parameters of Commands <Next>:
//       (required)   value : Int32
ConsoleApp2.exe Next 1
// Usage:
//    Commands:
//       Add
//       Next
ConsoleApp2.exe Next 1 Add
// Usage:
//    Parameters of Commands <Add>:
//       (required)   value1 : Int32
//       (required)   value2 : Int32
ConsoleApp2.exe Next 1 Add 2 3
// 5
```
