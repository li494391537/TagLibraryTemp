using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lirui.TagLibray.ExtensionCommon {
    public abstract class AbstractExtension : IExtensionCommon {
        public async Task<List<string>> BeginGetTags(string filename) {
            Func<List<string>> func = () => {
                return this.GetTags(filename);
            };
            Task<List<string>> task = new Task<List<string>>(func);
            task.Start();
            return await task;
        }

        public abstract List<string> GetTags(string filename);
    }
}
