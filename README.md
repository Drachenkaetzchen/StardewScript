# StardewScript

This is an experimental mod to provide scripting for Stardew Valley.
It uses [CS-Script](https://www.cs-script.net/) as the scripting engine.

# Experimental!

This is a very experimental mod. Things can change, things can break.
Only tested on Win10 with installed .NET SDKs.

# Features
- Can load+compile code at runtime
- Unload support
- Automatic unloading of a script throwing an exception in a event handler
- Almost all SMAPI events supported
- Intellisense/Auto-Completion support for many editors thanks to CS-Script
- Supports C# 7.0

# Use Cases
- For code which is way too small for a mod (like the FPS counter example)
- For playing around with the events and game internals
- Getting started with modding, without the need for a full-fledged C# solution

# Getting Started

- Build the project
- Run the game via SMAPI
- Copy `Mods/StardewScript/Examples/fps.cs` to the newly created `Scripts` directory in your game folder
- Type `script load fps` in the SMAPI console
- A simple FPS counter should now appear in the game

# Hints
- To allow for automatic assembly references, please add the following to
  the top of your script:
  ```
  //css_include ../Mods/StardewScript/Includes/references.cs
  ``` 
  This include references SMAPI, Stardew Valley and StardewScript.
