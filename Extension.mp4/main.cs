using System.Collections.Generic;
using Lirui.TagLibray.ExtensionCommon;

namespace Lirui.TagLibray.Extension {
    [ExtensionInfo(new string[] { "mp4" }, Author = "lirui", Version = "1.0.0.0")]
    public class Mp4 : AbstractExtension {
        public Mp4(string filename) : base(filename) { }
        public override KeyValuePair<string, string>[] GetTags() {
            throw new System.NotImplementedException();
        }
    }
}
