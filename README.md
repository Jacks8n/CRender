# Abstract
A soft renderer can output to command line interface, which is fully written in C#

# TODO
- [x] Support of shader in C#
- [ ] Interpolation for primitives
- [ ] Flexible pipeline with sets of interfaces
- [ ] Performance optimization
...

#Introduction

##How to build
|  Projecct|Dependency|Description|
|--|--|--|
|  CRender|CUtility, CShader|Main pipeline, rendering objects. **Library**|
|  CRenderTest|CRender, NUnit|Unit tests and samples for `CRender`. **Executable**|
|  CShader|CUtility|Independent shader system. **Library**|
|  CShaderTest|CShader, NUnit|Unity tests and samples for `CShader`. **Executable**|
|  CUtility|None|Math, extensions etc. **Library**|
Thus, at least the projects `CUtility`,`CShader`,`CRender` is required to build, and `CRenderTest` contains some samples to invoke render functions

##Write Shaders
`CShader` provides a unconventional manner to write shaders, it depends on pointers

It's recommended to see `CShaderTest.TestShader` or `CShader.ShaderDefault` to confirm how to start a shader, here we use the former to illustrate

```C#
using CShader;
using CUtility.Math;
```
Firstly, these two namespaces are useful, e.g. `CShader.ShaderValue` which contains useful runtime values, `CUtility.Math.GenericVector` which is used to represent RGBA values

```C#
    public struct VertexInputTest
    {
        public Vector4 Position;
    }
```

Then the inputs and outputs of shader stages should be defined, it's better to reuse them because of the machanism of `CShader` system

```C#
public class TestShader : IVertexShader //Interfaces of stages to be programmed
    {
        public unsafe void Main(
            [ShaderInput(typeof(VertexInputTest))] void* inputPtr,
            [ShaderOutput(typeof(FragmentInputTest))] void* outputPtr, IShaderStage<IVertexShader> _)
        {
            VertexInputTest* vin = (VertexInputTest*)inputPtr;
            FragmentInputTest* vout = (FragmentInputTest*)outputPtr;

            vout->Position = vin->Position * 2.5f;
        }
    }
```

The main shader should implement desired interfaces of stages, e.g. `IVertexShader`, `IFragmentShader`, `IGeometryShader`. However, only the vertex stage is supported currently.

The `Main(void*, void* IShaderStage<T>)` is a little complex
1. The input and output are `void*`, `ShaderInputAttribute` and `ShaderOutputAttribute` is required to mark their actual types
2. The parameter `IShaderStage<T> _` is just a marker, its value is the shader itself, it shouldn't be used

After these steps, a shader is finished, but to use them, some extra work is required:
1. Invoke `CShader.ShaderInterpreter<T>.Interprete<T>()` to interprete a shader, the first `T` is the type of stage, the second `T` is the type of shader. An easy way is to do that in a static constructor
2. Shaders should be wrapped in `CRender.Material`, which is ready to feed pipeline