using System.Linq;

namespace Tetris {
    class Blocks {
        private System.Random rnd;
        private readonly byte[] recentBlocks;
        private byte pos;

        public Blocks() {
            rnd = new System.Random();
            recentBlocks = new byte[3];
            pos = 2;
        }

        public byte[] GetBlock() {
            var block = new byte[][] {
                    new byte[] { 0, 4, 1, 4, 2, 4, 3, 4, 1, 1 },
                    new byte[] { 0, 4, 0, 5, 1, 4, 1, 5, 2, 1 },
                    new byte[] { 0, 4, 1, 3, 1, 4, 1, 5, 3, 1 },
                    new byte[] { 0, 5, 1, 5, 1, 4, 2, 4, 4, 1 },
                    new byte[] { 0, 4, 1, 4, 1, 5, 2, 5, 5, 1 },
                    new byte[] { 0, 4, 1, 4, 2, 4, 2, 5, 6, 1 },
                    new byte[] { 0, 5, 1, 5, 2, 5, 2, 4, 7, 1 } };
            while (true) {
                byte[] temp = block[rnd.Next(7)];
                if (!recentBlocks.Contains(temp[8])) {
                    pos++;
                    if (pos == 3) pos = 0;
                    recentBlocks[pos] = temp[8];
                    return temp;
                }
            }
        }

        public byte[] Rotate(byte b0, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7, byte h, byte r) =>
            h == 1 ? (r == 1 ? new byte[] { (byte)(b0 + 2), (byte)(b1 + 2), ++b2, ++b3, b4, b5, --b6, --b7, 1, 2 }
            : new byte[] { (byte)(b0 - 2), (byte)(b1 - 2), --b2, --b3, b4, b5, ++b6, ++b7, 1, 1 })
            : h == 3 ? (r == 1 ? new byte[] { ++b0, ++b1, --b2, ++b3, b4, b5, ++b6, --b7, 3, 2 }
            : r == 2 ? new byte[] { ++b0, --b1, ++b2, --b3, b4, b5, --b6, ++b7, 3, 3 }
            : r == 3 ? new byte[] { --b0, --b1, ++b2, ++b3, b4, b5, --b6, --b7, 3, 4 }
            : new byte[] { --b0, ++b1, --b2, --b3, b4, b5, ++b6, ++b7, 3, 1 })
            : h == 4 ? (r == 1 ? new byte[] { (byte)(b0 + 2), ++b1, ++b2, b3, b4, ++b5, --b6, b7, 4, 2 }
            : new byte[] { (byte)(b0 - 2), --b1, --b2, b3, b4, --b5, ++b6, b7, 4, 1 })
            : h == 5 ? (r == 1 ? new byte[] { ++b0, ++b1, b2, b3, ++b4, --b5, b6, (byte)(b7 - 2), 5, 2 }
            : new byte[] { --b0, --b1, b2, b3, --b4, ++b5, b6, (byte)(b7 + 2), 5, 1 })
            : h == 6 ? (r == 1 ? new byte[] { ++b0, ++b1, b2, b3, --b4, --b5, b6, (byte)(b7 - 2), 6, 2 }
            : r == 2 ? new byte[] { ++b0, --b1, b2, b3, --b4, ++b5, (byte)(b6 - 2), b7, 6, 3 }
            : r == 3 ? new byte[] { --b0, --b1, b2, b3, ++b4, ++b5, b6, (byte)(b7 + 2), 6, 4 }
            : new byte[] { --b0, ++b1, b2, b3, ++b4, --b5, (byte)(b6 + 2), b7, 6, 1 })
            : (r == 1 ? new byte[] { ++b0, ++b1, b2, b3, --b4, --b5, (byte)(b6 - 2), b7, 7, 2 }
            : r == 2 ? new byte[] { ++b0, --b1, b2, b3, --b4, ++b5, b6, (byte)(b7 + 2), 7, 3 }
            : r == 3 ? new byte[] { --b0, --b1, b2, b3, ++b4, ++b5, (byte)(b6 + 2), b7, 7, 4 }
            : new byte[] { --b0, ++b1, b2, b3, ++b4, --b5, b6, (byte)(b7 - 2), 7, 1 });
    }
}