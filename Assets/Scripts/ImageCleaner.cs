using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class ColorMath
{
    public static float Distance(this Color a, Color b)
    {
        return Mathf.Abs(a.r - b.r) + Mathf.Abs(a.g - b.g) + Mathf.Abs(a.b - b.b); 
    }
}

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

    public CoordRect(Texture2D tex)
    {
        m_x = 0;
        m_y = 0;
        m_w = tex.width;
        m_h = tex.height;
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

public class ImageCleaner : MonoBehaviour
{

    [SerializeField]
    Texture2D emptyScene;

    [SerializeField]
    Texture2D[] sequence;

    [SerializeField]
    float colorDistanceThreshold = 0.1f;

    [SerializeField]
    string outputDirectory;

    [SerializeField]
    bool cropImage;

    public void CleanUp()
    {
        for(int index = 0; index < sequence.Length; index++)
        {
            CleanUp(index);
        }

    }

    void CleanUp(int index)
    {
        var diff = GetDifferential(index);

        CoordRect clipBox;
        if (cropImage)
        {
            clipBox = BoxTrue(diff);
        }
        else
        {
            clipBox = new CoordRect(sequence[index]);
        }

        var newTex = Clone(index, diff, clipBox);
        Save(newTex, index, clipBox);
    }

    public bool[,] GetDifferential(int index)
    {
        Texture2D tex = sequence[index];
        bool[,] diff = new bool[tex.width, tex.height];
        for (int x = 0; x < tex.width; x++)
        {
            for (int y = 0; y < tex.height; y++)
            {                

                if (tex.GetPixel(x, y).Distance(emptyScene.GetPixel(x, y)) > colorDistanceThreshold)
                {
                    diff[x, y] = true;

                }
            }
        }
        return diff;
    }

    CoordRect BoxTrue(bool[,] data)
    {
        int w = data.GetLength(0);
        int h = data.GetLength(1);
        int minX = -1;
        int minY = -1;
        int maxX = 0;
        int maxY = 0;

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                if (data[x, y])
                {
                    if (minX < 0 || x < minX)
                    {
                        minX = x;
                    }
                    if (minY < 0 || y < minY)
                    {
                        minY = y;
                    }
                    if (y > maxY)
                    {
                        maxY = y;
                    }
                    if (x > maxX)
                    {
                        maxX = x;
                    }
                }
            }

        }
        minX = Mathf.Max(minX, 0);
        minY = Mathf.Max(minY, 0);
        maxX = Mathf.Max(maxX, minX);
        maxY = Mathf.Max(maxY, minY);

        return new CoordRect(minX, minY, maxX - minX, maxY - minY);
    }

    Texture2D Clone(int sourceIndex, bool[,] filter, CoordRect cropping)
    {
        var outTex = new Texture2D(cropping.Width, cropping.Height);
        var inTex = sequence[sourceIndex];

        for (int inX = cropping.X, outX = 0; inX < outTex.width; inX++, outX++)
        {
            for (int inY = cropping.Y, outY = 0; outY < outTex.height; inY++, outY++)
            {
                Color c = inTex.GetPixel(inX, inY);
                if (!filter[inX, inY])
                {
                    c.a = 0;
                }
                outTex.SetPixel(outX, outY, c);
            }
        }

        outTex.Apply();
        return outTex;
    }

    void Save(Texture2D tex, int source, CoordRect box)
    {
        string targetPath = outputDirectory + sequence[source].name + "-clean.png";
        Debug.Log(string.Format("{0}: Saving '{1}' using box {2}", source, targetPath, box));
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(outputDirectory + sequence[source].name + "-clean.png", bytes);
    }
}

