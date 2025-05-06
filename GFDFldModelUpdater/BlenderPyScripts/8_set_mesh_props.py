import bpy
import mathutils

# Gather only mesh objects that are selected
meshes = [obj for obj in bpy.context.selected_objects if obj.type == 'MESH']
if not meshes:
    raise Exception("Select one or more mesh objects (besides the armature).")

for obj in meshes:
    mesh_data = obj.data
    mesh_props = mesh_data.GFSTOOLS_MeshProperties
    mesh_props.unknown_0x12 = 69