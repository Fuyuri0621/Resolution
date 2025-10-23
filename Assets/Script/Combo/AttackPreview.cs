using UnityEngine;
using System.Collections;

public class AttackPreview : MonoBehaviour
{
    public Mesh debugBoxMesh;
    public Material debugBoxMaterial;
    public float debugDisplayDuration = 0.2f;

    public GameObject attackBoxPreview;

    public IEnumerator ShowAttackBoxVisual(Vector3 pos, Vector3 size, Quaternion rot)
    {
        float timer = 0f;
        while (timer < debugDisplayDuration)
        {
            Graphics.DrawMesh(debugBoxMesh, Matrix4x4.TRS(pos, rot, size), debugBoxMaterial, 0);
            timer += Time.deltaTime;
            yield return null;
        }
    }

   public void ShowBoxFX(Vector3 pos, Vector3 size, Quaternion rot)
    {
        GameObject obj = Instantiate(attackBoxPreview, pos, rot);
        obj.transform.localScale = size;
        Destroy(obj, 0.3f); // Εγ₯ά 0.3 ¬ν
    }
}
