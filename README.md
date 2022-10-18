# Rain World Monsoon
A fan-made, improved level editor for the indie game "Rain World"

### Current version: 0.1 (Alpha) (17th of October 2022)

# If you find a problem, please make an issue for it :)

## Installation Instructions
- Get it [here](https://github.com/MACMAN2003/Rain-World-Monsoon/releases/tag/Alpha)
- Extract the .zip to where you have installed the Official or Community level editor(s)
- If you don't already have a level editor, download the official one from RainDB or the community one from the Rain World Discord
- It shouldn't ask to replace files, but if it does, make backups!

## Features
- [x] Import and Export for the official Editor and RWCOM + Drought Editor ver 0.3.6 (aka, the community editor)
- [x] Panning and Zooming with mouse controls.
- [x] Geometry Editor features (except the move tool ;-;)
- [x] Tile Editor features (except default material and special tools)
- [x] Evil Mode (makes the tiles red)

## Planned Features (In order of likely to be done first)
- [ ] Finishing touches for the Tile Editor
- [ ] Level Properties Editor
- [ ] Environment Editor
- [ ] Effects Editor
- [ ] Props Editor
- [ ] Light Editor
- [ ] Camera Editor

## Dreams (Features) that I want to make true (May or may not happen)
- [ ] Rendering
- [ ] Voxel Struct Maker
- [ ] Prop Maker
- [ ] Defeat Scope Creep
## Build Instructions
### N/A :(
Or in other words: It's difficult, and working around it will be a pain.
## Build Instructions workaround (might not work)
- Download the Godot Engine Editor 4.0 Beta .NET (C#) build and make a new project in it
- Copy the "Graphics" folder from any level editor installation into the same folder as the Godot Editor .exe
- Copy the LevelEditorProjects folder from the Monsoon .zip and put it in the same folder as the Godot Editor .exe
- Copy all the files from the "Source-Code" Directory into the project's root "res://" folder /n
- Peek through the spaghetti code to see the important files that need to be supplied (by you) (you can get the important files from the official or community editor's .dir files (you need adobe director 12 for that))
- Copy said important files into the directory that the spaghetti code wants (or fake it and make them yourself) (or mess around with the spaghetti code)
- Cross your fingers and click on the "Play" button (if the editor throws a fit and wants a specific scene, select the "level_editor" scene)
- If it explodes, read the errors and try to figure out where it blew up
# Credits
- MACMAN2003 - Spaghetti "code"
- Videocult Media - Rain World and the Official Editor
- LB Gamer/M4rbleL1ne - Community Editor project lead, Community Editor materials
- DryCryCrystal - Community Editor materials
- Henpemaz - Community Editor camera snap
- LB Gamer/M4rbleL1ne - Drought Editor, which was absorbed by the Community Editor
- MSC Team - RainDB Editor, which was also absorbed by the Community Editor
