import bpy
import re
import math

# Regex for names starting with M###_### (allowing more stuff after)
armature_pattern = re.compile(r"^M\d{3}_\d{3}.*")

def log(msg):
    print(f"[Script] {msg}")

def rotate_bone(obj, bone_name, angle_deg):
    bpy.context.view_layer.objects.active = obj
    bpy.ops.object.mode_set(mode='POSE')

    bone_name_lower = bone_name.lower()
    found_bone = None
    for bone in obj.pose.bones:
        if bone.name.lower() == bone_name_lower:
            found_bone = bone
            break
    
    if not found_bone:
        log(f"    ⚠️ Bone '{bone_name}' not found in armature.")
        return

    found_bone.rotation_mode = 'XYZ'  # Ensure Euler mode
    current_rot = found_bone.rotation_euler.copy()

    log(f"    Current rotation for bone '{bone_name}': {current_rot}")

    # Modify Z rotation
    current_rot.z += math.radians(angle_deg)
    found_bone.rotation_euler = current_rot

    log(f"    New rotation for bone '{bone_name}': {found_bone.rotation_euler}")

    bpy.context.view_layer.update()

# Main loop
for obj in bpy.data.objects:
    for constraint in obj.constraints:
        if constraint.type == 'CHILD_OF':
            armature = constraint.target
            bone_name = constraint.subtarget

            print(f"[Script]   Found 'Child Of' constraint on {obj.name}")
            print(f"[Script]     Constraint subtarget: {bone_name}")
            
            if not armature or armature.type != 'ARMATURE':
                print(f"[Script]     ⚠️ Invalid armature target for {obj.name}")
                continue

            rotate_bone(armature, bone_name, 90)
