# Changes

## Changes that I've made

I've made the code live up to the demands and have the desired tests. In many cases there were certain things blocking or just not behaving the way they should. For example, there was an if-statement within the `MainLoop()` that blocked the loggers from making more than 5 logs.

I have changed the names and occasionally datatypes of different things so that they better lived up to best practices. For example renaming `LogInterface` to `ILogger` because interfaces usually have an "I" at the beginning of their name. 

A major change I made was deleting the `LogComponent.csproj` and moving the LogComponent-folder into the Application-folder. The extra project seemed unnecessary when the folder holding `Application.csproj` was practically empty and yet was the only thing to use the stuff inside LogComponent.


## Running the project

It needs .NET 10 to be installed.

In Visual Studio:
- Project: 
  - Open the solution with Visual Studio.
  - Click the green play button and it should run. 
  - The printed logs can be seen in `Application\LogTest`
- Test project: 
  - Open the solution with Visual Studio.
  - Use the top toolbar, go to "Test" and click on it. 
  - There should be a dropdown, where the first option is "Run All Tests". Click it.
  - The tests should run and pass. The results can be seen in the `Application.Tests\LogTest`

In the terminal:
- Project:
  - First, in the code, comment out the `_logFolderPath` in `AsyncLogger` that is in use and uncomment the one below that states it is for the terminal.
  - Then, in the terminal, navigate into `Application`.
  - Write `dotnet run` into the terminal and it should run.
  - The printed logs can be seen in `Application\LogTest`
- Test project: 
  - Navigate into the Application.Tests-folder.
  - Write `dotnet test` into the terminal. 
  - The tests should run and pass. The results can be seen in the `Application.Tests\LogTest`


## Changes in detail

| Change/Addition                                  | Reasoning        |
|--------------------------------------------------|------------------|
| Moved all class properties in classes to the top | For readability. |
| Removed regions from `LogLine.cs` | They were only present in this file and felt unnecessary in such a small project. |
| Renamed `LogTest` namespace to `LogComponent` | Fit better for folder structure and was confusing considering the project puts logs in a folder named "LogTest". |
| Renamed `LogInterface` and `AsyncLogInterface` to `ILogger` and `AsyncLogger` | Putting "I" as the first letter of the name of an interface is standard practice. An implementation of an interface should not have the word "interface" in its name, that can cause confusion. Naming it "Logger" and not "Log" makes it sound more like it's doing something rather than just being a class that can hold information. |
| Changed and added to many summaries of properties and methods | Documentation. |
| Deleted `LogComponent.csproj` and moved the LogComponent-folder into the Application-folder (thus also changing the namespace to `Application.LogComponent`) | Couldn't see a reason for LogComponent to have its own project or folder when it is only used by Application's `Program.cs`. |
| Updated `Application.csproj`'s .NET-version to one of the newest (10) | The version it had was very old and no longer supported (thus unsafe). |
| Removed `if (f > 5)` in `MainLoop()` in `AsyncLogger` | It stopped any Log from making more than five logs. |
| Changed `_lines` in `AsyncLogger` from a `List<LogLine>` into `ConcurrectQueue<LogLine>`. This also caused several other changes to take this type-change into account | Since the list was being accessed by two threads (the one adding to it in `Program.cs` and the `MainLoop()`) it needed to be able to handle several things trying to access it at once (thread-safe). |
| Created test for `WriteToLog()` | |
| Turned all instances of the `DateTime`-data type into `DateTimeOffset` | In order to use the `TimeProvider` normally and `FakeTimeProvider` for testing. |
| Removed underscores from `Stop_With_Flush()` and `Stop_Without_Flush()` | No other methods use underscores in their name, so I made it consistent. |
| Made `StopWithFlush()` wait for the main thread to finish and made a test for it. | |
| Made test for `StopWithoutFlush()` | |
| Fixed if-statement in `MainLoop()` so that the code does make a new file when crossing midnight and made test for it. | |
| Made helper-method `SetUpWriter()` in `AsyncLogger` that sets up the StreamWriter | Replaced three lines of very specific code that were written twice. |
| Deleted `CreateLineText()`-method from `LogLine` | It was a helper method that was one line, was only used once and just gave an empty string. Didn't serve any purpose. |
| Renamed `LineText()` in `LogLine` to `GetLineText()` | It is good practice to put "Get" at the beginning of get-method's name. |
| Removed `virtual` keyword from places in `LogLine` | `LogLine` has no derived classes (and probably never will since it's such a small project), so no need for the option to override. |
| Changed the constructor for `LogLine` and made two options for it | The original set `Text` to an empty string, making it seem like it would always be set it to an empty string no matter the input. |
| Made `Timestamp` in `LogLine` required | It is necessary for the logging that the time is not empty. |
| Renamed one of the columns that get logged from "Data" to "Text" | The `LogLine` calls it text, so better to stay consistent. |


## Changes to consider making

- Consider changing name of `ILogger`, something in C# is already called that.
- Consider having some of or all the tests get called with different data rows and just making more tests in general.
- Consider making locks for `_exit` and `_QuitWithFlush` in `AsyncLogger` since they can get accessed by more than one thread at the same time.
- Consider renaming `MainLoop()` in `AsyncLogger` to something that explains what it does.
- Consider using `DateTimeOffset.GetUtcNow()` instead of `DateTimeOffset.GetLocalNow()`.
- Consider whether some of or all the `Thread.Sleep()`'s in the code and tests should be removed.
- Consider if some exceptions should be thrown in some places and whether there should be some try-catch'es.
- Consider implementing validation of input.
- Consider if something could be done to avoid having the two while-loops and many if's in `MainLoop()`.
- Consider if `MainLoop()` should start out waiting for something to be added to `_lines` and time out if nothing gets added after a certain amount of time.
- Consider if `GetLineText()` should add a dot at the end of every text line. What if the text already ends with a dot?
- Figure out if there's a way to decide which folder it picks in `AsyncLogger` depending on whether you're running the program in Visual Studio or the command line.
