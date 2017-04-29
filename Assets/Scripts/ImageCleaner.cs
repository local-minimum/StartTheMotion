using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CoordRect
{
    int m_x;
    int m_y;
    int m_w;
    int m_h;

    public int X
    {
        get
        {
            return m_x;
        }
    }

    public int Y
    {
        get
        {
            return m_y;

        }
    }

    public int Width
    {
        get
        {
            return m_w;
        }
    }

    public int Height
    {
        get
        {
            return m_h;
        }
    }

    public CoordRect(int x, int y, int w, int h)
    {
        m_x = x;
        m_y = y;
        m_w = w;
        m_h = h;
    }

    public bool IsInside(int x, int y)
    {
        return m_x <= x && m_y <= y && m_x + m_w > x && m_y + m_h > y;
    }

    public override string ToString()
    {
        return string.Format("CoordRect ({0}, {1}), width {2}, height {3}", m_x, m_y, m_w, m_h);
    }
}

public class ImageCleaner : MonoBehaviour {

    [SerializeField]
    Texture2D emptyScene;

    [SerializeField]
    Texture2D[] sequence;

    [SerializeField]
    float maxColorDistance = 0.1f;

    public void CleanUp()
    {
        var diff = GetDifferential(0);
        Debug.Log(BoxTrue(diff));
    }

    public bool[,] GetDifferential(int index)
    {
        Texture2D tex = sequence[index];
        bool[,] diff = new bool[tex.width, tex.height];

        return diff;
    }

    static CoordRect BoxTrue(bool[,] data)
    {
        return new CoordRect();
    }
}
