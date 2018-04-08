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
        /// <summary>
        /// 获取MP3格式文件元信息（ID3v1、ID3v2）
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public override List<string> GetTags(string filename) {
            MemoryStream ms = new MemoryStream();
            using (FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                const int bufferLen = 1024;
                byte[] buffer = new byte[bufferLen];
                List<string> tags = new List<string>();

                //获取ID3v2歌曲信息

                //标签头：char[3] id3 = "ID3"，byte version，byte _version，byte flag，int size
                int len = fs.Read(buffer, 0, 10);
                int size = 0;
                if (len == 10 && Encoding.ASCII.GetString(buffer, 0, 3) == "ID3") {
                    size = size << 7 | buffer[6] & 0b01111111;
                    size = size << 7 | buffer[7] & 0b01111111;
                    size = size << 7 | buffer[8] & 0b01111111;
                    size = size << 7 | buffer[9] & 0b01111111;
                    tags.Add(size.ToString());
                }
                var iso = Encoding.GetEncoding("ISO-8859-1");
                int test = 10;

                while (test < size) {
                    len = fs.Read(buffer, 0, 10);
                    test += 10;
                    if (buffer[0] == 0)
                        break;
                    string str = Encoding.ASCII.GetString(buffer, 0, 4);
                    int _size = BitConverter.ToInt32(buffer, 4);
                    _size = System.Net.IPAddress.NetworkToHostOrder(_size);
                    test += _size;
                    if (_size < bufferLen) {
                        len = fs.Read(buffer, 0, _size);
                        if (buffer[0] == 1) {
                            if (buffer[1] == 0xFE && buffer[2] == 0xFF) {
                                str += " - " + Encoding.BigEndianUnicode.GetString(buffer, 3, len - 3);
                            } else {
                                str += " - " + Encoding.Unicode.GetString(buffer, 3, len - 3);
                            }

                        } else {
                            str += " - " + iso.GetString(buffer, 1, len - 1);
                        }
                    } else {
                        fs.Seek(_size, SeekOrigin.Current);
                    }
                    tags.Add(str);
                }
                fs.Seek(size + 10, SeekOrigin.Begin);

                //获取ID3v1歌曲信息：128bytes
                fs.Seek(-128, SeekOrigin.End);
                len = fs.Read(buffer, 0, 128);
                if (Encoding.ASCII.GetString(buffer, 0, 3) == "TAG") {
                    //歌名:30bytes
                    tags.Add(Encoding.ASCII.GetString(buffer, 3, 30));
                    //作者:30bytes
                    tags.Add(Encoding.ASCII.GetString(buffer, 33, 30));
                    //专辑名:30bytes
                    tags.Add(Encoding.ASCII.GetString(buffer, 63, 30));
                    //年份:4bytes
                    tags.Add(Encoding.ASCII.GetString(buffer, 93, 4));
                    //备注:28bytes
                    tags.Add(Encoding.ASCII.GetString(buffer, 97, 28));
                    //音轨号（注意前面有一位保留位）:1byte
                    tags.Add(buffer[126].ToString());
                }

                return tags;
            }
        }
    }
}
