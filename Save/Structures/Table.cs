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
}
