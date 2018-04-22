using Lirui.TagLibray.ExtensionCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.png {
    [ExtensionInfo(new string[] { "png" }, Author = "lirui", Version = "1.0.0.0")]
    public class Png : AbstractExtension {
        public Png(string filename) : base(filename) { }
        public override KeyValuePair<string, string>[] GetTags() {
            throw new NotImplementedException();
        }
    }
}
