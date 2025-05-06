import bpy

# Loop through all objects in the scene
for obj in bpy.context.scene.objects:
    if obj.type != 'MESH':
        continue

    mesh_data = obj.data

    # Check if the mesh has the GFSTOOLS_MeshProperties
    if hasattr(mesh_data, "GFSTOOLS_MeshProperties"):
        mesh_props = mesh_data.GFSTOOLS_MeshProperties

        if mesh_props.unknown_0x12 == 96:
            obj.hide_set(True)
            print(f"Hid object: {obj.name}")
