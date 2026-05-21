# Changes

## Changes that I've made

1. Moved all class properties to the top -> For readability.
2. Removed regions from LogLine.cs -> They were only present in this file and felt unnecessary in such a small project.
3. Renamed LogTest namespace to LogComponent -> Fit better for folder structure and was confusing considering the project will generate a "LogTest"-folder.
4. Renamed LogInterface and AsyncLogInterface to ILogger and AsyncLogger -> Putting "I" as the first letter of the name of an interface is standard practice. An implementation of an interface should not have the word "interface" in its name, that can cause confusion. Naming it "Logger" and not "Log" makes it sound more like it's doing something rather than just being a class that can hold information.
5. Changed and added to many summaries of properties and methods.

## Changes I wish to make

- Consider if LogComponent needs its own project and couldn't just be a folder inside Application