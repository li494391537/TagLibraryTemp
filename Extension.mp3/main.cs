using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lirui.TagLibray.ExtensionCommon;

namespace Lirui.TagLibray.Extension {
    [ExtensionInfo(new string[] { "mp3" }, Author = "lirui", Version = "1.0.0.0")]
    public class Mp3 : AbstractExtension { 

        public Mp3(string filename) {
            Filename = filename;
        }
        
        /// <summary>
        /// 获取MP3格式文件元信息（ID3v1、ID3v2）
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public override Dictionary<string, string> GetTags() {
            Dictionary<string, string> id3 = null;
            using (FileStream fs = File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                BufferedStream bufferedStream = new BufferedStream(fs);
                byte[] vs = new byte[3];
                int count = fs.Read(vs, 0, 3);
                string str = Encoding.ASCII.GetString(vs);
                if (str != "ID3") {
                    return null;
                }
                id3 = new Dictionary<string, string>();
            }
            return id3;
            //throw new NotImplementedException();
        }

        

    }
}
