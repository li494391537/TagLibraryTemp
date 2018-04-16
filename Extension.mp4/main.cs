using System.Collections.Generic;
using Lirui.TagLibray.ExtensionCommon;

namespace Lirui.TagLibray.Extension {
    [ExtensionInfo(new string[] { "mp3" }, Author = "lirui", Version = "1.0.0.0")]
    public class Mp4 : AbstractExtension {
        public override Dictionary<string, string> GetTags() {
            throw new System.NotImplementedException();
        }
    }
}
