using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Tropico.Methods;
namespace Tropico.GameSave.Structs
{
    public class Table
    {
        public List<Entry> Entries;
        public Table(Stream xIn, Header hdr)
        {
            Entries = new List<Entry>();
            for (int i = 0; i < hdr.Count; i++)
                Entries.Add(new Entry(xIn, i));

            //Calculating Lengths
            for (int i = 0; i < Entries.Count; i++)
                if (i == Entries.Count - 1)
                    Entries[i].Length = (int)xIn.Length - Entries[i].Offset;
                else
                    Entries[i].Length = Entries[i + 1].Offset - Entries[i].Offset;
        }
        public void Write(Stream xMain, Stream xOut, Header hdr)
        {
            StreamUtils.WriteBytes(xOut, new byte[hdr.Count * 4]);
            for (int i = 0; i < hdr.Count; i++)
            {
                long tablePos = 12 + (i * 4);

                long xPos = xOut.Position;
                xOut.Position = tablePos;
                StreamUtils.WriteInt32(xOut, (int)xPos, false);
                xOut.Position = xPos;

                if (Entries[i].CustomStream != null)
                    Entries[i].CustomStream.Position = 0;
                else
                    xMain.Position = Entries[i].Offset;

                if (Entries[i].CustomStream == null)
                    StreamUtils.ReadBufferedStream(xMain, Entries[i].Length, xOut);
                else
                    StreamUtils.ReadBufferedStream(Entries[i].CustomStream, (int)Entries[i].CustomStream.Length, xOut);
            }
        }

        public Entry GetEntryByEID(int EID)
        {
            for (int i = 0; i < Entries.Count; i++)
                if (Entries[i].EID == EID)
                    return Entries[i];
            return null;
        }
    }
    public class Entry
    {
        public Int32 Offset { get; set; }
        public Int32 Length { get; set; }
        public Stream CustomStream { get; set; }
        public int EID;
        Stream xMain;
        public Entry(Stream xIn, int EID)
        {
            xMain = xIn;
            Offset = StreamUtils.ReadInt32(xIn, false);
            this.EID = EID;
        }
        public void ExtractToStream(Stream xOut)
        {
            if (CustomStream != null)
                CustomStream.Position = 0;
            else
                xMain.Position = Offset;

            if (CustomStream == null)
                StreamUtils.ReadBufferedStream(xMain, Length, xOut);
            else
                StreamUtils.ReadBufferedStream(CustomStream, (int)CustomStream.Length, xOut);
        }
    }
    public class Header
    {
        public string Magic { get; set; }
        public Int32 Count { get; set; }
        public Int16 Unk { get; set; }

        public Header(Stream xIn)
        {
            Magic = new string(StreamUtils.ReadChars(xIn, 6));
            Count = StreamUtils.ReadInt32(xIn, false) + 1;
            Unk = StreamUtils.ReadInt16(xIn, false);
        }

        public void Write(Stream xOut)
        {
            StreamUtils.WriteChars(xOut, Magic.ToCharArray());
            StreamUtils.WriteInt32(xOut, Count - 1, false);
            StreamUtils.WriteInt16(xOut, Unk, false);
        }

    }
}