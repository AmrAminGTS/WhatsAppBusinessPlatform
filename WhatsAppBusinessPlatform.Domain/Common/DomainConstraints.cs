using System;
using System.Collections.Generic;
using System.Text;

namespace WhatsAppBusinessPlatform.Domain.Common;

public static class DomainConstraints
{
    public static LengthConstraint ShortString => new(1, 49);
    public static LengthConstraint MediumString => new(50, 199);
    public static LengthConstraint LongString => new(200, 999);
}
public readonly record struct LengthConstraint(int Min, int Max);
