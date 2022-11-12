using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// m_start と m_end を繋ぐようなコライダーを作る機能を提供する。
/// 四角い棒のようなコライダーになるが、その太さを変えたい場合は Box Collider の Size.x, sixe.y を編集すること。
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class PivotColliderController : MonoBehaviour
{
    /// <summary>コライダーの始点</summary>
    public static Transform m_start;
    /// <summary>コライダーの終点</summary>
    public static Transform m_end;
    public static GameObject PlayerObj;
    BoxCollider col;
    void Start()
    {
        if (!m_start || !m_end)
        {
            Debug.LogError(name + " needs both Start and End.");
        }
        col = GetComponent<BoxCollider>();//コライダーの情報を取得
    }

    void FixedUpdate()
    {
        if (m_start && m_end)
        {
            transform.LookAt(PlayerObj.transform);//Boxコライダーに常にプレイヤーの方を向かせる
            float distance = Vector3.Distance(m_start.position, m_end.position);//始端と終端の距離を求める
            col.size = new Vector3(col.size.x, col.size.y, distance);//コライダーのサイズを変更する
        }
    }
}