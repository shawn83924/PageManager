using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(PageManager))]
public class PageManagerEditor : Editor
{
    PageManager managerTarget;
    SerializedObject serializedObject;
    SerializedProperty PageSize;
    SerializedProperty PageDistance;
    public SerializedProperty DirectChange;
    bool _DirectChange;
    GameObject CanvasObj;
    Canvas CanvasComponent;

    private void OnEnable()
    {   
        managerTarget = (PageManager)target;
        serializedObject = new SerializedObject(FindObjectOfType<PageManager>());
        PageSize = serializedObject.FindProperty("pages.Array.size");
        PageDistance = serializedObject.FindProperty("PageDistance");
        //DirectChange = serializedObject.FindProperty("DirectChange");
        CanvasObj = GameObject.Find("Canvas");
        if (CanvasObj == null) 
        {
            CanvasObj = Instantiate((GameObject)Resources.Load("Canvas", typeof(GameObject)), Vector3.zero, Quaternion.identity);
            Instantiate((GameObject)Resources.Load("EventSystem", typeof(GameObject)), Vector3.zero, Quaternion.identity).name = "EventSystem";
            CanvasObj.name = "Canvas";
        }
        CanvasComponent = CanvasObj.GetComponent<Canvas>();
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        managerTarget.ScreenWidth = CanvasComponent.pixelRect.width;
        EditorGUILayout.PropertyField(PageSize, new GUIContent("Number of Pages"));
        EditorGUILayout.PropertyField(PageDistance);
        _DirectChange = EditorGUILayout.Toggle("DirectChange", managerTarget.DirectChange);
        managerTarget.DirectChange = _DirectChange;
        if (!_DirectChange)
        {
            managerTarget.MoveSpeed = EditorGUILayout.Slider("MoveSpeed", managerTarget.MoveSpeed, 0.01f, 10);
        }
        if (GUILayout.Button("Apply")) 
        {
            //針對每個頁面進行創建、位置設定
            managerTarget.ArrangeElement(PageSize.intValue, PageDistance.floatValue, CanvasObj);
            //Apply value to PageManager 
            //serializedObject.ApplyModifiedProperties();
            
        }

    }

}
