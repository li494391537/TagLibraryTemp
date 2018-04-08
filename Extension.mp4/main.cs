using System.Collections.Generic;
using Lirui.TagLibray.ExtensionCommon;

namespace Lirui.TagLibray.Extension {
    [ExtensionInfo(new string[] { "mp3" }, Author = "lirui", Version = "1.0.0.0")]
    public class Mp4 : AbstractExtension {
        public override List<string> GetTags(string filename) {
            throw new System.NotImplementedException();
        }
    }
}
