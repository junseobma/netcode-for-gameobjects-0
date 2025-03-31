using UnityEditor;

public class ToggleScene2D3D
{
    [MenuItem("Tools/Toggle 2D 3D View %`")] 
    static void Toggle2DView()
    {
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView != null)
        {
            sceneView.in2DMode = !sceneView.in2DMode;
            sceneView.Repaint();
        }
    }
}
