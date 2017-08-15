﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Scheme.NET.VirtualMachine.Instructions
{
    public interface IInstruction
    {
        string Name { get; }

        string Serialize();
        IInstruction Execute(ISchemeVM vm);
    }
}
