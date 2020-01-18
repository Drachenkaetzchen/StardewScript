# StardewScript

This is an experimental mod to provide scripting for Stardew Valley.
It uses [CS-Script](https://www.cs-script.net/) as the scripting engine.

Think of StardewScript as sized down mods which can be loaded and
unloaded while the game is running. You can do almost anything you could
do with a mod, with the same APIs.

See the [FPS Counter Example](StardewScript/Examples/fps.cs)
# Experimental!

This is a very experimental mod. Things can change, things can break.
Only tested on Win10 with installed .NET SDKs.

Another drawback is that each loaded script hooks into *all* SMAPI events
because the `Script` base class exposes all events as overridable 
methods.

# Features
- Can load+compile code at runtime
- Unload support
- Automatic unloading of a script throwing an exception in a event handler
- Almost all SMAPI events supported
- Intellisense/Auto-Completion support for many editors thanks to CS-Script
- Supports C# 7.0
- High performance - scripts are compiled, not interpreted

# Use Cases
- For code which is way too small for a mod (like the FPS counter example)
- For playing around with the events and game internals
- Getting started with modding, without the need for a full-fledged C# development environment

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
- Only the first class implementing `StardewScript.Framework.Script` is executed.
- Multiple classes per file should in theory be possible, but not tested.

# Use the base class

Each script class must inherit from `StardewScript.Framework.Script`. This
base class provides overridable methods for all events (see below) and access
to the `IMonitor` and `IModHelper` members.

## Event Handlers

All [SMAPI events](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Events#Events)
are supported except for the specialized ones. See
[Script.Overridables.cs](StardewScript/Framework/Script.Overridables.cs) for a reference.
 
You should never attach your own event handlers directly to `IModHelper.Events` because
things will break when your script is reloaded or unloaded.
