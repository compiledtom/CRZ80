using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z80
{
    /// <summary>
    /// Computer display
    /// </summary>
    public class Framebuffer 
    {
        public bool RedrawDone = false;

        public readonly int Width;
        public readonly int Height;
        public readonly byte[] CharacterData = null;
        public readonly byte[] Colors = null;
        public readonly byte[] Attributes = null;

        public Framebuffer(int NewWidth, int NewHeight)
        {
            int Length = Width * Height;
        }

        public delegate void DataChangedEvent(object Sender, int Address, byte Data);
        public event DataChangedEvent DataChanged;
        protected void OnDataChanged(int Address, byte Data)
        {
            if (RedrawDone || DataChanged == null)
                return;

            RedrawDone = true;
            DataChanged(this, Address, Data);
        }

        public void SetCharacter(int X, int Y, byte C)
        {
            int i = Y * Width + X;
            CharacterData[i] = C;
            OnDataChanged(i, C);
        }

        public byte GetCharacter(int X, int Y)
        {
            int i = Y * Width + X;
            return CharacterData[i];
        }
    }
}
