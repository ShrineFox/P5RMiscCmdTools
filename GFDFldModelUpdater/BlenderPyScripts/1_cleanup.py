import bpy

# Set the focal length (lens), clip start, and clip end
for workspace in bpy.data.workspaces:
    for screen in workspace.screens:
        for area in screen.areas:
            if area.type == 'VIEW_3D':
                for space in area.spaces:
                    if space.type == 'VIEW_3D':
                        space.lens = 250
                        space.clip_start = 2.5
                        space.clip_end = 1000000

# Set viewport display of each armature to 'WIRE'
for obj in bpy.data.objects:
    if obj.type == 'ARMATURE':
        obj.data.display_type = 'WIRE'  # Set display type to wire

# Enable 'Automatically Pack Resources' using the file operator
bpy.ops.file.pack_all()
bpy.ops.file.autopack_toggle()

# Purge unused data
bpy.ops.outliner.orphans_purge(do_local_ids=True, do_linked_ids=True, do_recursive=True)
