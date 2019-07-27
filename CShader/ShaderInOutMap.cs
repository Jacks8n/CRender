using System;

using static CUtility.Extension.MarshalExtension;

namespace CShader
{
    public unsafe class ShaderInOutMap : IDisposable
    {
        public readonly SemanticLayout Layout = new SemanticLayout();

        internal byte* InOutBufferPtr { get; private set; } = null;

        private bool _generated = false;

        internal void GenerateMap(ShaderSemantic semantic)
        {
            if (_generated) return;

            _generated = true;
            Layout.SetLayout(semantic);
            InOutBufferPtr = AllocBytes<byte>(Layout.TotalBufferSize);
            Layout.SetReadWritePointer(InOutBufferPtr);
        }

        void IDisposable.Dispose()
        {
            if (!_generated)
                return;

            Free(InOutBufferPtr);
        }
    }
}