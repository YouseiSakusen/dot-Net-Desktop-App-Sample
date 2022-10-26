namespace GenericHostDi;

public class ScopeSample : IDisposable
{
	public ScopeSample()
		=> Console.WriteLine($"{nameof(ScopeSample)} New!");

	public void Foo()
		=> Console.WriteLine($"{nameof(Foo)}!");

	public void Dispose()
		=> Console.WriteLine($"{nameof(ScopeSample)} Dispose...");
}
