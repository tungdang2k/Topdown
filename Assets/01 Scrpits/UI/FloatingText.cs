using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private TextMeshPro textMesh;
    private void Awake()
    {
    
        textMesh = GetComponent<TextMeshPro>();
    }
    public void SetText(string text)
    {

        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshPro>();
        }

        if (textMesh != null)
        {
            textMesh.text = text;
        }
        else
        {
            Debug.LogError("LỖI: Prefab '" + gameObject.name + "' thiếu thành phần TextMeshPro (loại 3D)!");
        }
    }
}