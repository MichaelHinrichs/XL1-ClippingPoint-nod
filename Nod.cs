//Written for XL1 ClippingPoint. https://store.steampowered.com/app/747240
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace XL1_ClippingPoint_nod
{
    public class Nod
    {
        static BinaryReader br;
        List<Material> materials = new();
        List<Mesh> meshs = new();
        public static Nod Read(string NodFile)
        {
            br = new(File.OpenRead(NodFile));
            br.ReadInt16();
            short meshCount = br.ReadInt16();
            br.BaseStream.Position = 16;
            Nod nod = new();
                
                for (int i = 0; i < meshCount; i++)
                {
                    nod.materials.Add(new());
                    br.ReadInt32();
                }

            for (int m = 0; m < meshCount; m++)
            {
                br.ReadBytes(7);
                br.ReadInt32();
                int faceCount = br.ReadInt32();
                int vertCount = br.ReadInt32();

                br.BaseStream.Position += 56;
                Mesh mesh = new();

                for (int i = 0; i < faceCount; i++)
                    mesh.faces.Add(new(br.ReadInt32(), br.ReadInt32(), br.ReadInt32()));

                for (int i = 0; i < vertCount; i++)
                {
                    mesh.verts.Add(new(br.ReadInt32(), br.ReadInt32(), br.ReadInt32()));
                    mesh.normals.Add(new(br.ReadInt32(), br.ReadInt32(), br.ReadInt32()));
                    br.BaseStream.Position += 32;
                }
                br.ReadByte();
                nod.meshs.Add(mesh);
            }
            return nod;
        }

        class Mesh
        {
            public List<Vector3> faces = new();
            public List<Vector3> verts = new();
            public List<Vector3> normals = new();
        }

        class Material
        {
            string name = new string(br.ReadChars(br.ReadInt16())).TrimEnd('\0');
            int unknown = br.ReadInt32();
        }
    }
}
