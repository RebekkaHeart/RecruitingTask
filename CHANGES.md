# Changes

## Changes that I've made

- Moved all class properties to the top -> For readability.
- Removed regions from LogLine.cs -> They were only present in this file and felt unnecessary in such a small project.
- Renamed LogTest namespace to LogComponent -> Fit better for folder structure and was confusing considering the project will generate a "LogTest"-folder.
- Renamed LogInterface and AsyncLogInterface to ILogger and AsyncLogger -> Putting "I" as the first letter of the name of an interface is standard practice. An implementation of an interface should not have the word "interface" in its name, that can cause confusion. Naming it "Logger" and not "Log" makes it sound more like it's doing something rather than just being a class that can hold information.
- Changed and added to many summaries of properties and methods.
- Deleted LogComponent.csproj and moved the LogComponent-folder into Application (thus also adding Application. in front of the namespace) -> Couldn't see a reason for LogComponent to have its own project or folder when it is only used by Application's Program.cs
- Updated Application.csproj's .NET-version to the newest (10) -> The version it had was very old and no longer supported.

## Changes I wish to make

- 