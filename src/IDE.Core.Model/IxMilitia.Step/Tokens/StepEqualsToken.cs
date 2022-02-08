﻿// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Step.Tokens
{
    internal class StepEqualsToken : StepToken
    {
        public override StepTokenKind Kind => StepTokenKind.Equals;

        public StepEqualsToken(int line, int column)
            : base(line, column)
        {
        }

        public override string ToString()
        {
            return "=";
        }

        public static StepEqualsToken Instance { get; } = new StepEqualsToken(-1, -1);
    }
}
