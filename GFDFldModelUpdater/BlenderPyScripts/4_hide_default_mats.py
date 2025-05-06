import bpy

# Define the material name to search for
material_name = "gfdDefaultMat0"

# Loop through all objects in the scene
for obj in bpy.context.scene.objects:
    # Check if the object has a material
    if obj.type == 'MESH' and obj.data.materials:
        # Loop through the materials of the object
        for mat in obj.data.materials:
            # Check if the material name contains the specified string
            if material_name in mat.name:
                # Hide the object in the viewport (Alt+H will unhide this)
                obj.hide_set(True)
                # Optionally, hide the object in the final render
                # obj.hide_render = True
                break
