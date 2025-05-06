
import bpy
import math

bpy.ops.object.mode_set(mode='OBJECT')

for obj in bpy.context.scene.objects:
    if obj.type == 'MESH':
        mesh = obj.data

        if mesh.uv_layers:
            # Check that at least one UV layer exists
            if len(mesh.uv_layers) > 0:
                    mesh.uv_layers[0].name = "UV0"
            else:
                print(f"Error: No UV layers found for object {obj.name}")
        
        if mesh.materials:
            for material in mesh.materials:
                material.GFSTOOLS_MaterialProperties.diffuse_uv_out = '0'
                if material.node_tree:
                    for node in material.node_tree.nodes:
                        if node.type == 'TEX_IMAGE':
                            node.name = "Diffuse Texture"
                            node.label = "Diffuse Texture"
                            
                            uv_map_node = material.node_tree.nodes.new(type='ShaderNodeUVMap')
                            
                            # Check if any active UV map exists
                            if len(mesh.uv_layers) > 0:
                                uv_map_node.uv_map = mesh.uv_layers.active.name
                            else:
                                print(f"Error: No UV layers to assign for object {obj.name}")
                            
                            links = material.node_tree.links
                            links.new(uv_map_node.outputs['UV'], node.inputs['Vector'])

