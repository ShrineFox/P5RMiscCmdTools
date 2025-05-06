import bpy

# Define the material name to search for
material_name = "gfdDefaultMat0"

# Loop through all objects in the scene
for obj in bpy.context.scene.objects:
    if obj.type == 'MESH' and obj.data.materials:
        for mat in obj.data.materials:
            hide_obj = False

            # Check if the material name contains the specified string
            if material_name in mat.name:
                hide_obj = True
            else:
                # Check if the material uses nodes and has no diffuse texture
                if mat.use_nodes:
                    principled_node = None
                    for node in mat.node_tree.nodes:
                        if node.type == 'BSDF_PRINCIPLED':
                            principled_node = node
                            break

                    if principled_node:
                        base_input = principled_node.inputs.get("Base Color")
                        if base_input and not base_input.is_linked:
                            # No diffuse texture connected
                            hide_obj = True
                        elif base_input and base_input.is_linked:
                            # Check if the linked node is an image texture
                            from_node = base_input.links[0].from_node
                            if from_node.type != 'TEX_IMAGE':
                                hide_obj = True
                else:
                    # If nodes are not used, assume no diffuse texture
                    hide_obj = True

            # Hide the object if flagged
            if hide_obj:
                obj.hide_set(True)
                # Optionally hide from render as well
                # obj.hide_render = True
                break
