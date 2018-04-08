using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lirui.TagLibray.Extension;
using Lirui.TagLibray.ExtensionCommon;

namespace Extension.mp3.Test {
    class Program {
        static void Main(string[] args) {
            IExtensionCommon mp3 = new Mp3();
            try {
                var tags = mp3.BeginGetTags(@"D:\CloudMusic\AZU\SINGLE BEST ＋ ～10th Anniversary～\AZU - For You.mp3");
                for (int i = 0; i < tags.Result.Count; i++) {
                    Console.WriteLine($"Tag[{i.ToString().PadLeft(2, '0')}] : {tags.Result[i]}");
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
