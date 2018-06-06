using System.Linq;

namespace Tetris {
    internal class Blocks {
        readonly System.Random rnd;
        readonly int[] recentBlocks;
        int pos;

        public Blocks() {
            rnd = new System.Random();
            recentBlocks = new int[3];
            pos = 2;
        }

        public int[] GetBlock() {
            var block = new[] {
                    new[] { 0, 4, 1, 4, 2, 4, 3, 4, 1, 1 },
                    new[] { 0, 4, 0, 5, 1, 4, 1, 5, 2, 1 },
                    new[] { 0, 4, 1, 3, 1, 4, 1, 5, 3, 1 },
                    new[] { 0, 5, 1, 5, 1, 4, 2, 4, 4, 1 },
                    new[] { 0, 4, 1, 4, 1, 5, 2, 5, 5, 1 },
                    new[] { 0, 4, 1, 4, 2, 4, 2, 5, 6, 1 },
                    new[] { 0, 5, 1, 5, 2, 5, 2, 4, 7, 1 } };
            while (true) {
                var temp = block[rnd.Next(7)];
                if (recentBlocks.Contains(temp[8])) continue;
                pos++;
                if (pos == 3) pos = 0;
                recentBlocks[pos] = temp[8];
                return temp;
            }
        }

        public int[] Rotate(int b0, int b1, int b2, int b3, int b4, int b5, int b6, int b7, int h, int r) =>
            h == 1 ? (r == 1 ? new[] { b0 + 2, b1 + 2, ++b2, ++b3, b4, b5, --b6, --b7, 1, 2 }
            : new[] { b0 - 2, b1 - 2, --b2, --b3, b4, b5, ++b6, ++b7, 1, 1 })
            : h == 3 ? (r == 1 ? new[] { ++b0, ++b1, --b2, ++b3, b4, b5, ++b6, --b7, 3, 2 }
            : r == 2 ? new[] { ++b0, --b1, ++b2, --b3, b4, b5, --b6, ++b7, 3, 3 }
            : r == 3 ? new[] { --b0, --b1, ++b2, ++b3, b4, b5, --b6, --b7, 3, 4 }
            : new[] { --b0, ++b1, --b2, --b3, b4, b5, ++b6, ++b7, 3, 1 })
            : h == 4 ? (r == 1 ? new[] { b0 + 2, ++b1, ++b2, b3, b4, ++b5, --b6, b7, 4, 2 }
            : new[] { b0 - 2, --b1, --b2, b3, b4, --b5, ++b6, b7, 4, 1 })
            : h == 5 ? (r == 1 ? new[] { ++b0, ++b1, b2, b3, ++b4, --b5, b6, b7 - 2, 5, 2 }
            : new[] { --b0, --b1, b2, b3, --b4, ++b5, b6, b7 + 2, 5, 1 })
            : h == 6 ? (r == 1 ? new[] { ++b0, ++b1, b2, b3, --b4, --b5, b6, b7 - 2, 6, 2 }
            : r == 2 ? new[] { ++b0, --b1, b2, b3, --b4, ++b5, b6 - 2, b7, 6, 3 }
            : r == 3 ? new[] { --b0, --b1, b2, b3, ++b4, ++b5, b6, b7 + 2, 6, 4 }
            : new[] { --b0, ++b1, b2, b3, ++b4, --b5, b6 + 2, b7, 6, 1 })
            : (r == 1 ? new[] { ++b0, ++b1, b2, b3, --b4, --b5, b6 - 2, b7, 7, 2 }
            : r == 2 ? new[] { ++b0, --b1, b2, b3, --b4, ++b5, b6, b7 + 2, 7, 3 }
            : r == 3 ? new[] { --b0, --b1, b2, b3, ++b4, ++b5, b6 + 2, b7, 7, 4 }
            : new[] { --b0, ++b1, b2, b3, ++b4, --b5, b6, b7 - 2, 7, 1 });
    }
}