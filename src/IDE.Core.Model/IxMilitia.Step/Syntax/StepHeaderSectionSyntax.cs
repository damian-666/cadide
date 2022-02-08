﻿// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using IxMilia.Step.Tokens;

namespace IxMilia.Step.Syntax
{
    internal class StepHeaderSectionSyntax : StepSyntax
    {
        public override StepSyntaxType SyntaxType => StepSyntaxType.HeaderSection;

        public List<StepHeaderMacroSyntax> Macros { get; }

        public StepHeaderSectionSyntax(int line, int column, IEnumerable<StepHeaderMacroSyntax> macros)
            : base(line, column)
        {
            Macros = macros.ToList();
        }

        public override IEnumerable<StepToken> GetTokens()
        {
            throw new NotSupportedException();
        }
    }
}
