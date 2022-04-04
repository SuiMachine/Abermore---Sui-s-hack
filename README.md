Abermore - Sui's hack
============
This is a hack made using BepInEx 6 and HarmonyX.

# Features
* Fixes issue a softlock in the tutorial level in regions that don't use dot as a decimal seperator, by enforcing en-GB culture for parsing.
* (Optional - Optimisation) Experimental mesh baker that tries to combine meshes in 150 unites from each other to reduce the amount of drawcalls.
* (Optional - Optimisation) An ability to disable 2nd outdoor camera responsible for extending the city's skybox. This might save 2-10 fps in the city.
* (Optional - Feature) Option to disable edge border texture - enabling this will result in a bug related to conversation icons being visible however, as these icons pop up in the lower left part of the screen, but with this feature enabled, there is nothing hiding them.
* (Optional - Feature) An ability to modify FOV (it is buggy though and needs further work).
* (Optional - Graphics) Option to disable camera optimisations, which sets an absurd near-plane for the camera and multiple culling distances for Unity layers - this will lower framerate significently.
* (Optional - Graphics) Prevent the game from disabling shadow casters for meshes.

# Installation
Download the content of Release folder or Github release and copy it to the game's directory. Do **not** modify the file structure and do **not** move the files outside the BepInEx/plugins folder.

# Configuration
* Make sure to launch the game at least once with Bepinex and this hack installed. Afterwards a config file should be created and ready for edit: **BepInEx/config/AbemoreSuisHack.cfg**.
