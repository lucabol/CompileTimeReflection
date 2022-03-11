// Importing the CompileTime namespace to gain access to compile time capabilities.
using Microsoft.CodeAnalysis.CSharp.SourceGenerators.CompileTime;

// Main interpreter loop, understands 'Double', 'Plus1', 'Bye'
Vm vm = new();
while(true)
{
    var line = Console.ReadLine();
    if(line is null) break;
    
    var words = line.Split(' ');
    foreach (var word in words)
    {
        // 'Token', 'FindToken' and 'ExecToken' are generated at compile time
        // reflecting on the methods in the 'Vm' class in the 'CompileTimeExecutor'.
        Token token = VmExt.FindToken(word) ?? Token.NotFound;
        VmExt.ExecToken(vm, token);
    }
    Console.WriteLine($"State: {vm.state}");
}

// Adding a method to the 'Vm' class causes the interpreter to understand it.
// You don't have to add a new value to the Token enum or code to handle user input for it.
public class Vm
{
   public  int state;
    
    public void Op_Double()    => state *= 2;
    public void Op_Plus1()     => state += 1;
    public void Op_NotFound()  => Console.WriteLine("Word not found.");
    public void Op_Bye()       => Environment.Exit(0);
}

// A class marked with the `CompileTime` attribute causes the compiler to execute
// its `CompileTimeExecute' method at compile time.
[CompileTime]
public class CompileTimeExecutor
{
    public void CompileTimeExecute()
    {
        // Compile time reflection API. Get all the 'Op_*' methods from Vm.
        var compileVm = CompileTimeReflection.GetType("Vm");
        var opMethods = compileVm.GetMethods().Where(m => m.Name.StartsWith("Op_")); 
        var words = opMethods.Select(m => m.Name[3..].ToLowerInvariant());

        // Compile time reflection emit API. Generate the Enum.
        CompileTimeEmit.EmitEnum("Token", words);

        // Generate the class 'VmExt' with the two methods that we need.
        var vmExt = CompileTimeEmit.EmitClass("VmExt");

        var findToken = vmExt.AddStaticMethod("FindToken", "Token token");
        findToken.AddSwitch("token", words.Select(w => (w, $"return Token.{w}")));

        var exectToken = vmExt.AddStaticMethod("ExectToken", "Vm vm, Token token");
        exectToken.AddSwitch("token", words.Select(w => ($"Token.{w}", $"vm.{w}()")));
    }
}

