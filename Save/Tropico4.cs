using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Tropico.Methods;
using Tropico.GameSave.Structs;
namespace Tropico.GameSave
{
    public class Save
        {
            public Header Header { get; set; }
            public Table Table { get; set; }
            Stream xMain;
            public Save(Stream xIn)
            {
                xMain = xIn;
                Header = new Header(xIn);
                Table = new Table(xIn, Header);
            }

            public void Write(Stream xOut)
            {
                Header.Count = Table.Entries.Count;
                Header.Write(xOut);
                Table.Write(xMain, xOut, Header);
            }
        }
    
}
