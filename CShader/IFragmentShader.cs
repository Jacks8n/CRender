namespace CShader
{
    public unsafe interface IFragmentShader
    {
        void Fragment(void* input, void* output);
    }
}