# Mercy
Mercy is my custom 3D game engine. This engine is not meant to be used to make games; the purpose of this engine is to deepen my knowledge of game development and game engines through building one from the ground up.

Feature list currently includes:
 - Event system
 - Input system

## Running the Engine
Mercy uses a slightly edited version of Premake as a build system. Running the `GenerateProject.bat` file will configure the project for Visual Studio 2022.
If you wish to change anything for whatever reason, make sure to call the Premake executable located in the top level `Vendor` folder to ensure proper configuration.

Work on the renderer will be visible through running the Sandbox project until work on the editor has started.

The editor for Mercy uses WPF and .NET 8.0. To run the editor, select it as the start project in Visual Studio 2022 with the correct version of .NET installed.
Currently, the editor exists to make sure this path is viable and will not be functional until I get to it in my development cycle.

## Current Work
With some of the infrastructure for the engine now implemented, I am currently focusing on adding a renderer using OpenGL and Glad. This will allow me to explore graphics programming while also allowing me to see any future work I do (e.g. verify physics module).

## Future Work
As previously mentioned, the purpose of this engine is not to make games but to learn more about game engines. Because of this, the modules that I work on will be driven more by personal interest rather than by parts necessary for a game.

List of modules I plan to work on:
 - Level Editor
 - Game Object & Component System
 - Physics Module
 - Hot Reloading
