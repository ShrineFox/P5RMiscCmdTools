import bpy
import mathutils

# Base name & starting index for new armatures
BASE_NAME = "M000_"
start_idx = 73

# Active object must be the original armature
original_armature = bpy.context.active_object
if not original_armature or original_armature.type != 'ARMATURE':
    raise Exception("Active object must be the source armature.")

# Gather only mesh objects that are selected
meshes = [obj for obj in bpy.context.selected_objects if obj.type == 'MESH']
if not meshes:
    raise Exception("Select one or more mesh objects (besides the armature).")

# Switch to Edit Mode on the original armature to add bones
bpy.context.view_layer.objects.active = original_armature
bpy.ops.object.mode_set(mode='EDIT')
ebones = original_armature.data.edit_bones

# Track the bones we create so we can add GFS props later
bone_info = []  # list of (mesh_obj, bone_name)

for mesh in meshes:
    # Compute world-space head & tail
    wm = mesh.matrix_world.copy()
    head = wm.to_translation()
    orientation = wm.to_quaternion()

    # Apply global -90 degree Z rotation
    z_rot = mathutils.Euler((0, 0, math.radians(-90)), 'XYZ').to_quaternion()
    rotated_orientation = z_rot @ orientation
    tail = head + (rotated_orientation @ mathutils.Vector((0, 0.1, 0)))

    # Create the bone
    bone = ebones.new(mesh.name)
    bone.head = head
    bone.tail = tail
    bone.use_connect = False

    bone_info.append((mesh, mesh.name))

# Back to Object Mode
bpy.ops.object.mode_set(mode='OBJECT')

# Now add the GFS Node properties to each new bone
for mesh, bname in bone_info:
    # Access the Bone data (plugin API lives on the bone data, not edit bone)
    bone_data = original_armature.data.bones[bname]
    node_props = bone_data.GFSTOOLS_NodeProperties

    # Define the properties you wanted
    values = [
        ("fldLayoutOfModel_major",  "INT32", 0),
        ("fldLayoutOfModel_minor",  "INT32", 72),
        ("fldLayoutOfModel_animNo", "INT32", 0),
        ("fldLayoutOfModel_animLoop","INT32", 0),
        ("fldLayoutOfModel_animRandom","INT32",0),
    ]

    # Add each one
    for dname, dtype, ival in values:
        new_prop = node_props.properties.add()
        new_prop.dname = dname
        new_prop.dtype = dtype
        new_prop.int32_data = ival

# Now create per-mesh armatures, clear parenting, add constraints, etc.
for i, (mesh, bone_name) in enumerate(bone_info):
    idx = start_idx + i
    arm_name = f"{BASE_NAME}{idx:03d}"

    # 1) Create new Armature object for hierarchy only
    bpy.ops.object.armature_add(enter_editmode=False, location=(0, 0, 0))
    new_arm = bpy.context.active_object
    new_arm.name = arm_name
    new_arm.data.name = arm_name + "_Data"

    # Link it into all of the mesh's collections
    for coll in mesh.users_collection:
        if new_arm.name not in coll.objects:
            coll.objects.link(new_arm)
    if bpy.context.scene.collection.objects.get(new_arm.name):
        bpy.context.scene.collection.objects.unlink(new_arm)

    # 2) Unparent the mesh (keep transforms)
    bpy.context.view_layer.objects.active = mesh
    mesh.select_set(True)
    bpy.ops.object.parent_clear(type='CLEAR_KEEP_TRANSFORM')

    # 3) Add Child Of constraint to the original armature + bone
    con = mesh.constraints.new('CHILD_OF')
    con.name = f"ChildOf_{bone_name}"
    con.target = original_armature
    con.subtarget = bone_name

    # 4) Simulate “Set Inverse”
    bpy.context.view_layer.objects.active = mesh
    mesh.select_set(True)
    bpy.ops.constraint.childof_set_inverse(constraint=con.name, owner='OBJECT')

    # 5) Parent the mesh to the new armature (object parent, not deform)
    mesh.parent = new_arm

    mesh.select_set(False)

print(f"Done! Created {len(bone_info)} bones in '{original_armature.name}',\n"
      f"added GFS properties, and built separate armature containers M000_{start_idx:03d}…")
