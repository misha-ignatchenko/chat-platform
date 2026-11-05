using System;
using System.Collections.Generic;
using System.Text;

namespace ChatPlatform.Shared
{
    public record GroupInfo(Guid Id, string Name, GroupRole Role);
}
