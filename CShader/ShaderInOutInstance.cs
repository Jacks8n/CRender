using System;

using static CUtility.Extension.MarshalExtension;

namespace CShader
{
    public unsafe class ShaderInOutInstance : IDisposable
    {
        public readonly SemanticLayout Layout;

        public SemanticLayout.MappedLayout* MappedLayout { get; private set; }

        internal byte* InOutBufferPtr { get; private set; } = null;

        private bool _instantiated = false;

        public ShaderInOutInstance(SemanticLayout layout)
        {
            Layout = layout;
        }

        internal void Instantiate()
        {
            AssertIfInstantiated();

            InOutBufferPtr = AllocBytes<byte>(Layout.TotalBufferSize);
            MappedLayout = Layout.MapTo(InOutBufferPtr);
            _instantiated = true;
        }

        private void AssertIfInstantiated()
        {
            if (_instantiated)
                throw new Exception("InOutMap has been instantiated");
        }

        void IDisposable.Dispose()
        {
            if (!_instantiated)
                return;

            Free(InOutBufferPtr);
        }
    }
}