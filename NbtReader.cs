using System.IO.Compression;
using System.Text;

namespace MCMapCare
{
    /// <summary>
    /// Lecteur simple du format NBT (Named Binary Tag) de Minecraft.
    /// Utilisé pour extraire les informations du fichier level.dat.
    /// Le fichier est compressé en GZip et stocke les données en big-endian.
    /// </summary>
    public static class NbtReader
    {
        // Constantes des types de tags NBT
        private const byte TAG_End = 0;
        private const byte TAG_Byte = 1;
        private const byte TAG_Short = 2;
        private const byte TAG_Int = 3;
        private const byte TAG_Long = 4;
        private const byte TAG_Float = 5;
        private const byte TAG_Double = 6;
        private const byte TAG_Byte_Array = 7;
        private const byte TAG_String = 8;
        private const byte TAG_List = 9;
        private const byte TAG_Compound = 10;
        private const byte TAG_Int_Array = 11;
        private const byte TAG_Long_Array = 12;

        /// <summary>
        /// Lit un fichier level.dat (GZip + NBT) et retourne un dictionnaire plat
        /// des chemins de tags vers leurs valeurs textuelles.
        /// Exemple : "Data.LevelName" -> "MonMonde", "Data.GameType" -> "0"
        /// </summary>
        public static Dictionary<string, string> ReadLevelDat(string levelDatPath)
        {
            using var fileStream = File.OpenRead(levelDatPath);
            using var gzip = new GZipStream(fileStream, CompressionMode.Decompress);
            using var ms = new MemoryStream();
            gzip.CopyTo(ms);
            ms.Position = 0;

            using var reader = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);
            var result = new Dictionary<string, string>();

            // Le NBT commence par un TAG_Compound racine avec un nom (souvent vide)
            ReadNamedTag(reader, result, "");
            return result;
        }

        /// <summary>
        /// Lit un tag avec son type et son nom (format standard hors liste).
        /// </summary>
        private static void ReadNamedTag(BinaryReader reader, Dictionary<string, string> result, string parentPath)
        {
            byte type = reader.ReadByte();
            if (type == TAG_End) return;

            // Lecture du nom du tag
            ushort nameLen = ReadUShortBE(reader);
            string name = Encoding.UTF8.GetString(reader.ReadBytes(nameLen));

            string fullPath = string.IsNullOrEmpty(parentPath) ? name : $"{parentPath}.{name}";
            ReadPayload(reader, result, fullPath, type);
        }

        /// <summary>
        /// Lit le contenu d'un tag selon son type et le stocke dans le dictionnaire.
        /// </summary>
        private static void ReadPayload(BinaryReader reader, Dictionary<string, string> result, string path, byte type)
        {
            switch (type)
            {
                case TAG_Byte:
                    result[path] = reader.ReadSByte().ToString();
                    break;

                case TAG_Short:
                    result[path] = ReadShortBE(reader).ToString();
                    break;

                case TAG_Int:
                    result[path] = ReadIntBE(reader).ToString();
                    break;

                case TAG_Long:
                    result[path] = ReadLongBE(reader).ToString();
                    break;

                case TAG_Float:
                    result[path] = ReadFloatBE(reader).ToString("G");
                    break;

                case TAG_Double:
                    result[path] = ReadDoubleBE(reader).ToString("G");
                    break;

                case TAG_Byte_Array:
                    // On ignore le contenu mais on consomme les octets
                    int baLen = ReadIntBE(reader);
                    reader.ReadBytes(baLen);
                    result[path] = $"[tableau de {baLen} octets]";
                    break;

                case TAG_String:
                    ushort strLen = ReadUShortBE(reader);
                    result[path] = Encoding.UTF8.GetString(reader.ReadBytes(strLen));
                    break;

                case TAG_List:
                    byte listItemType = reader.ReadByte();
                    int listLen = ReadIntBE(reader);
                    for (int i = 0; i < listLen; i++)
                        ReadPayload(reader, result, $"{path}[{i}]", listItemType);
                    break;

                case TAG_Compound:
                    // Lit les tags enfants jusqu'à TAG_End
                    while (true)
                    {
                        byte childType = reader.ReadByte();
                        if (childType == TAG_End) break;

                        ushort childNameLen = ReadUShortBE(reader);
                        string childName = Encoding.UTF8.GetString(reader.ReadBytes(childNameLen));
                        string childPath = string.IsNullOrEmpty(path) ? childName : $"{path}.{childName}";
                        ReadPayload(reader, result, childPath, childType);
                    }
                    break;

                case TAG_Int_Array:
                    int iaLen = ReadIntBE(reader);
                    for (int i = 0; i < iaLen; i++) ReadIntBE(reader);
                    result[path] = $"[tableau de {iaLen} entiers]";
                    break;

                case TAG_Long_Array:
                    int laLen = ReadIntBE(reader);
                    for (int i = 0; i < laLen; i++) ReadLongBE(reader);
                    result[path] = $"[tableau de {laLen} longs]";
                    break;
            }
        }

        // ===========================
        // Lectures big-endian
        // ===========================

        private static short ReadShortBE(BinaryReader r)
            => (short)((r.ReadByte() << 8) | r.ReadByte());

        private static ushort ReadUShortBE(BinaryReader r)
            => (ushort)((r.ReadByte() << 8) | r.ReadByte());

        private static int ReadIntBE(BinaryReader r)
            => (r.ReadByte() << 24) | (r.ReadByte() << 16) | (r.ReadByte() << 8) | r.ReadByte();

        private static long ReadLongBE(BinaryReader r)
        {
            long hi = (uint)ReadIntBE(r);
            long lo = (uint)ReadIntBE(r);
            return (hi << 32) | lo;
        }

        private static float ReadFloatBE(BinaryReader r)
        {
            byte[] b = r.ReadBytes(4);
            Array.Reverse(b);
            return BitConverter.ToSingle(b, 0);
        }

        private static double ReadDoubleBE(BinaryReader r)
        {
            byte[] b = r.ReadBytes(8);
            Array.Reverse(b);
            return BitConverter.ToDouble(b, 0);
        }
    }
}
