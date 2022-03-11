namespace Microsoft.CodeAnalysis.CSharp.SourceGenerators.CompileTime;

public class CompileTimeAttribute: Attribute { }

public static class CompileTimeReflection
{
	public static Type[] GetTypes() =>
        System.Reflection.Assembly.GetCallingAssembly().GetTypes();
	public static Type GetType(string typeName) =>
        System.Reflection.Assembly.GetCallingAssembly().GetType(typeName)!;
}

public class CompileTimeMethod
{
    public void AddSwitch(string switcher, IEnumerable<(string, string)> cases) { }
}
public class CompileTimeClass
{
    public CompileTimeMethod AddStaticMethod(string name, string args) => new CompileTimeMethod();
}

public static class CompileTimeEmit
{
    public static void EmitEnum(string name, IEnumerable<string> values) { }
    public static CompileTimeClass EmitClass(string name) => new CompileTimeClass();
}

public enum Token { Double, Plus1, NotFound, Bye }

public static class VmExt
{
    public static Token? FindToken(string name) {
        var found = Enum.TryParse<Token>(name, out var result);
        return found ? result : null;
    }
    public static void  ExecToken(Vm vm, Token token)
    {
        switch(token)
        {
            case Token.Plus1    : vm.Op_Plus1(); break; 
            case Token.Double   : vm.Op_Double(); break;
            case Token.Bye      : vm.Op_Bye();break;
            case Token.NotFound : vm.Op_NotFound(); break;
        }
    }
}
