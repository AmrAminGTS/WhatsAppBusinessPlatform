using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsAppBusinessPlatform.Domain.Common;

public static class GeneralErrors
{
    public static DomainError FailureWhileSavingToDatabase() 
        => DomainError.Failure("General.FailureWhileSavingToDatabase");

    public static DomainError NotFound(string name, string id)
        => DomainError.NotFound("General.NotFoundWithId", name, id);
    public static DomainError NotFound(string name)
        => DomainError.NotFound("General.NotFound", name);
}
