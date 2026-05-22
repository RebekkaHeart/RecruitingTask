# Changes

## Changes that I've made

| Change                                           | Reasoning       |
|--------------------------------------------------|-----------------|
| Moved all class properties in classes to the top | For readability |
| Removed regions from `LogLine.cs` | They were only present in this file and felt unnecessary in such a small project. |
| Renamed `LogTest` namespace to `LogComponent` | Fit better for folder structure and was confusing considering the project puts logs in a folder named "LogTest". |
| Renamed `LogInterface` and `AsyncLogInterface` to `ILogger` and `AsyncLogger` | Putting "I" as the first letter of the name of an interface is standard practice. An implementation of an interface should not have the word "interface" in its name, that can cause confusion. Naming it "Logger" and not "Log" makes it sound more like it's doing something rather than just being a class that can hold information. |
| Changed and added to many summaries of properties and methods. | |
| Deleted `LogComponent.csproj` and moved the LogComponent-folder into the Application-folder (thus also changing the namespace to `Application.LogComponent`) | Couldn't see a reason for LogComponent to have its own project or folder when it is only used by Application's `Program.cs` |
| Updated `Application.csproj`'s .NET-version to the newest (10) | The version it had was very old and no longer supported. |
| Changed `_lines` in `AsyncLogger` from a `List<LogLine>` into `ConcurrectQueue<LogLine>`. This also caused several other changes to take this type-change into account | Since the list was being accessed by two threads, the one adding to it in `Program.cs` and the `MainLoop`, it needed to be able to handle several things trying to access it at once (thread-safe) |

## Changes I wish to make

- 