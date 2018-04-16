using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lirui.TagLibray.ExtensionCommon {
    public abstract class AbstractExtension : IExtensionCommon {
        
        protected string Filename { get; set; }
        
        public async Task<Dictionary<string, string>> BeginGetTags() {
            Func<Dictionary<string, string>> func = () => {
                return GetTags();
            };
            Task<Dictionary<string, string>> task = new Task<Dictionary<string, string>>(func);
            task.Start();
            return await task;
        }

        public abstract Dictionary<string, string> GetTags();
    }
}
