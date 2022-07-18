// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

namespace Simpleflow
{
    public sealed class ScriptHelperContext
    {
        readonly FlowOutput _flowOutput;
        public ScriptHelperContext(FlowOutput flowOutput)
        {
            _flowOutput = flowOutput;
        }

        public bool HasErrors => _flowOutput.Errors.Count > 0;
        public bool HasMessages => _flowOutput.Messages.Count > 0;
        public bool HasOutput => _flowOutput.Output.Count > 0;
    }
}

