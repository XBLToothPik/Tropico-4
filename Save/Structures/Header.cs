using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Tropico.Methods;
namespace Tropico.GameSave.Structs
{
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
