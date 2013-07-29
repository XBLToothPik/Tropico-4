using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Tropico.Methods;
namespace Tropico.GameSave.Structs
{
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
}
