# P5RMiscCmdTools
A bunch of commandline utilities I use for convenience while modding P5R.

# CreateLooseBINs
When run, it takes .dds textures from a nearby ``/_DevStuff`` folder.  
Then it outputs emulated ``.BIN`` folders containing only matching textures in a nearby ``/LooseBINs`` folder.

# RepackBINs
When run, it asks for the location of your unedited ``FIELD_TEX/TEXTURES`` ``.BIN`` files.  
Then, it takes the emulated ``.BIN`` folders in the nearby ``/LooseBINs`` folder and merges them with a copy of the unedited ``.BIN``.  
Repacked ``.BIN`` files are output to a nearby ``RepackedBINs/TEX_WIP.CPK`` folder.

# GFDHelperID
This program edits all the GMD files in a given folder to:  
- Set the helper bone ID on ``root`` node to ``420``
- Set the helper bone ID on ``rot`` node to ``69``
- Remove properties named "ScalingMax" or "MapChannel:8"
These changes make it possible to attach models to the root/rot nodes in-game using flowscript.

# InitScriptMaker
Used this to create a hooked ``.flow`` for each field init script in the game.  
First it adds a hook for our custom script to the top of the file.  
Then copies the contents of the first procedure, inserts a check for field script timing 4, and then executes our custom procedure.

# RandomText
Used this to generate a ``.msg`` file containing a range of random characters for each message,  
with the intent to give the illusion of a "corrupted" file.

# ClearReloadedIICache
Place this ``.exe`` in your mod's folder and run it whenever you need to clear the cache.  
This deletes the following directories:
- ``crifs.v2.hook\Bind``
- ``p5rpc.modloader\Cache``
Useful if you need to make sure scripts are recompiled or need to rule out other caching bugs.

# CreateDummyFiles
Looks for custom ``.flow`` and ``.msg`` files in a nearby ``/BF`` directory.  
Creates a matching ``.BF`` or ``.BMD`` in a nearby ``/CPK/DUMMYFILES.CPK`` directory.  
Removes dummy files for ``.flow`` or ``.msg`` that no longer exist to prevent crashes.  
This makes it so that the Reloaded-II BF/BMD Emulators can load hooked files.

# StringSearch
This returns the filename of each binary file in a given folder that contains a specified string.