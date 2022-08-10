// Copyright (c) navtech.io. All rights reserved.
// See License in the project root for license information.

using System.Threading;

namespace Simpleflow
{
    public sealed class ScriptHelperContext
    {
        readonly FlowOutput _flowOutput;
        readonly CancellationToken _token;

        private ScriptHelperContext() { }

        internal ScriptHelperContext(FlowOutput flowOutput, bool isArgumentMutable, CancellationToken token)
        {
            _flowOutput = flowOutput;
            _token = token;
            IsArgumentMutable = isArgumentMutable;
        }

        public bool IsArgumentMutable { get; }
        public bool HasErrors => _flowOutput.Errors.Count > 0;
        public bool HasMessages => _flowOutput.Messages.Count > 0;
        public bool HasOutputs => _flowOutput.Output.Count > 0;
        public CancellationToken CancellationToken => _token;
    }
}

