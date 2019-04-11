using System;
using System.Collections.Generic;
using System.Text;

namespace Monkey.Services
{
    public interface IMetadataChangedSubscriber
    {
        void Changed();
    }
}
