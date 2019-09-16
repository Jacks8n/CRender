using System;
using System.Runtime.CompilerServices;

using static CUtility.Extension.MarshalExtension;

namespace CShader
{
    public unsafe class ShaderInOutInstance : IDisposable
    {
        public MappedLayout* MappedLayout { get; private set; }

        internal byte* InOutBufferPtr { get; private set; } = null;

        internal readonly SemanticLayout Layout;

        private bool _instantiated = false;

        public ShaderInOutInstance(SemanticLayout layout)
        {
            Layout = layout;
        }

        internal void Instantiate()
        {
            AssertIfInstantiated();

            InOutBufferPtr = Alloc<byte>(Layout.TotalBufferSize);
            MappedLayout = Layout.MapToValues(InOutBufferPtr);
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