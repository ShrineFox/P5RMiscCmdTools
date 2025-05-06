import bpy

# Step 1: Get the selected mesh and its parent armature
selected_obj = bpy.context.object
if not selected_obj or selected_obj.type != 'MESH':
    raise Exception("Please select a mesh object that is parented to an armature.")

# Manually check if parent is an armature
armature = selected_obj.parent
if not armature or armature.type != 'ARMATURE':
    raise Exception("Selected mesh is not directly parented to an armature.")

# Step 2: Collect all mesh children of the armature
mesh_children = [child for child in armature.children if child.type == 'MESH']

# Step 3: Delete all objects except the armature and its mesh children
for obj in bpy.data.objects:
    if obj != armature and obj not in mesh_children:
        bpy.data.objects.remove(obj, do_unlink=True)

# Step 4: Select all mesh children of the armature
bpy.ops.object.select_all(action='DESELECT')
for mesh in mesh_children:
    mesh.select_set(True)

# Set one active mesh (needed by some operators)
bpy.context.view_layer.objects.active = mesh_children[0] if mesh_children else None

# Step 5: Set origin of selected meshes to 3D cursor
bpy.ops.object.origin_set(type='ORIGIN_CURSOR')

# Step 6: Move geometry to object origin
for mesh in mesh_children:
    bpy.context.view_layer.objects.active = mesh
    bpy.ops.object.mode_set(mode='EDIT')
    bpy.ops.mesh.select_all(action='SELECT')
    bpy.ops.transform.translate(value=[-v for v in mesh.location])
    bpy.ops.object.mode_set(mode='OBJECT')
