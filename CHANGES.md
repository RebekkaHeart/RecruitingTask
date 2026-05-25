# Changes

## Changes that I've made

| Change/Addition                                  | Reasoning       |
|--------------------------------------------------|-----------------|
| Moved all class properties in classes to the top | For readability |
| Removed regions from `LogLine.cs` | They were only present in this file and felt unnecessary in such a small project. |
| Renamed `LogTest` namespace to `LogComponent` | Fit better for folder structure and was confusing considering the project puts logs in a folder named "LogTest". |
| Renamed `LogInterface` and `AsyncLogInterface` to `ILogger` and `AsyncLogger` | Putting "I" as the first letter of the name of an interface is standard practice. An implementation of an interface should not have the word "interface" in its name, that can cause confusion. Naming it "Logger" and not "Log" makes it sound more like it's doing something rather than just being a class that can hold information. |
| Changed and added to many summaries of properties and methods. | |
| Deleted `LogComponent.csproj` and moved the LogComponent-folder into the Application-folder (thus also changing the namespace to `Application.LogComponent`) | Couldn't see a reason for LogComponent to have its own project or folder when it is only used by Application's `Program.cs` |
| Updated `Application.csproj`'s .NET-version to the newest (10) | The version it had was very old and no longer supported (thus unsafe). |
| Removed `if (f > 5)` in `MainLoop()` in `AsyncLogger` | It stopped any Log from making more than five logs. |
| Changed `_lines` in `AsyncLogger` from a `List<LogLine>` into `ConcurrectQueue<LogLine>`. This also caused several other changes to take this type-change into account | Since the list was being accessed by two threads, the one adding to it in `Program.cs` and the `MainLoop()`, it needed to be able to handle several things trying to access it at once (thread-safe) |
| Created test for `AsyncLogger` | |
| Turned all instances of the `DateTime`-data type into `DateTimeOffset` | In order to use the `TimeProvider` normally and `FakeTimeProvider` for testing. |
| Removed underscores from `Stop_With_Flush()` and `Stop_Without_Flush()` | No other methods use underscores in this way, so I made it consistent. |
| Made `StopWithFlush()` wait for the main thread to finish and made a test for it | |
| Made test for `StopWithoutFlush()` | |
| Fixed if-statement in `MainLoop()` so that the code does make a new file when crossing midnight and made test for it | |

## Changes I wish to make

- Fix location of LogTest-folder. Right now it's deep in the bin, both normally and for tests.
- Consider changing name of `ILogger`, something in C# is already called that.
- Consider having some of or all the tests get called with different data rows.
- Make helper-method in `AsyncLogger` for when it creates a new Log-file, because three lines of code are written twice.
- Consider making locks for `_exit` and `_QuitWithFlush` in `AsyncLogger` since they can get accessed by more than one thread at the same time.
- Consider renaming `MainLoop()` in `AsyncLogger` to something that explains what it does.
- Consider using `DateTimeOffset.GetUtcNow()` instead of `DateTimeOffset.GetLocalNow()`.
- Consider if some of or all the `Thread.Sleep()`'s in the tests should be removed.
- Consider if some exceptions should be thrown in some places and whether there should be some try-catch'es.
- Consider implementing more validation of input.