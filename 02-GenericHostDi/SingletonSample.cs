using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericHostDi;

public class SingletonSample : IDisposable
{
	private readonly TransientSample transient;
	//private readonly IServiceScopeFactory scopeFactory;
	//private readonly IServiceProvider provider;
	private readonly IServiceScope scope;
	private readonly ScopeSample scopePrivate;

	public ValueTask CallScopeAsync()
	{
		Console.WriteLine("CallScope");

		this.scopePrivate.Foo();

		Console.WriteLine("CallScope End");

		return ValueTask.CompletedTask;

		//await using (var scope = scopeFactory.CreateAsyncScope())
		//{
		//	using (var sample = scope.ServiceProvider.GetRequiredService<ScopeSample>())
		//	{
		//		sample.Foo();
		//	}
		//}
	}

	public SingletonSample(TransientSample transientSample, IServiceScopeFactory serviceScopeFactory)
	{
		Console.WriteLine("SingletonSample New!");
		this.transient = transientSample;
		//(this.transient, this.scopeFactory) = (transientSample, serviceScopeFactory);
		//this.provider = this.scopeFactory.CreateScope().ServiceProvider;

		this.scope = serviceScopeFactory.CreateScope();
		this.scopePrivate = this.scope.ServiceProvider.GetRequiredService<ScopeSample>();
	}

	public void Dispose()
		=> Console.WriteLine("SingletonSample Disposed");
}
