using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace COMP1640.Service
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}