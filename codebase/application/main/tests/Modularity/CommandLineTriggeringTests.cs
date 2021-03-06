﻿using Axle.DependencyInjection;
using Axle.Modularity;
using NUnit.Framework;


namespace Axle.ApplicationTests.Modularity
{
    [TestFixture]
    public class CommandLineTriggeringTests
    {
        public abstract class AbstractModule
        {
            [ModuleInit]
            internal void Init(ModuleExporter mex)
            {
                mex.Export(this);
            }
        }

        [Module]
        [ModuleCommandLineTrigger(ArgumentIndex = 0, ArgumentValue = "a")]
        public class A : AbstractModule { }

        [Module]
        [ModuleCommandLineTrigger(ArgumentIndex = 0, ArgumentValue = "b")]
        public class B : AbstractModule { }

        [Module]
        [ModuleCommandLineTrigger(ArgumentIndex = 1)]
        public class C
        {
            [ModuleInit]
            internal void Init(ModuleExporter mex, string[] cmdArgs)
            {
                mex.Export(this);
                System.Console.WriteLine("Triggering command argument: {0}", cmdArgs[1]);
            }
        }

        [Requires(typeof(A))]
        [Module]
        public class A1 : AbstractModule { }

        [Requires(typeof(B))]
        [Module]
        public class B1 : AbstractModule { }

        private void ConfigureModules(IApplicationModuleConfigurer cfg)
        {
            cfg.Load<A1>().Load<B1>().Load<C>();
        }
       
        [Test]
        public void TestTriggeredCommandLineTriggerrableModuleAndDependenciesLoad()
        {
            IContainer container = null;
            using (Application.Build().ConfigureDependencies(c => container = c).ConfigureModules(ConfigureModules).Run("a"))
            {
                if (!container.TryResolve<A>(out _))
                {
                    Assert.Fail("Module triggered by command-line argument must load");
                }
                if (!container.TryResolve<A1>(out _))
                {
                    Assert.Fail("Dependent modules of module triggered by command-line argument must load");
                }
            }
        }

        [Test]
        public void TestUntriggerredCommandLineTriggerableModuleAndDependenciesDoNotLoad()
        {
            IContainer container = null;
            using (Application.Build().ConfigureDependencies(c => container = c).ConfigureModules(ConfigureModules).Run("a"))
            {
                if (container.TryResolve<B>(out _))
                {
                    Assert.Fail("Module not triggered by command-line argument must not load");
                }
                if (container.TryResolve<B1>(out _))
                {
                    Assert.Fail("Dependent modules of module not triggered by command-line argument must not load");
                }
                if (container.TryResolve<C>(out _))
                {
                    Assert.Fail("Module not triggered by command-line argument must not load");
                }
            }
        }

        [Test]
        public void TestTriggeredLooseCommandLineTriggerrableModuleAndDependenciesLoad()
        {
            IContainer container = null;
            using (Application.Build().ConfigureDependencies(c => container = c).ConfigureModules(ConfigureModules).Run("a", "whatever"))
            {
                if (!container.TryResolve<A>(out _))
                {
                    Assert.Fail("Module triggered by command-line argument must load");
                }
                if (!container.TryResolve<A1>(out _))
                {
                    Assert.Fail("Dependent modules of module triggered by command-line argument must load");
                }
                if (container.TryResolve<B>(out _))
                {
                    Assert.Fail("Module not triggered by command-line argument must not load");
                }
                if (container.TryResolve<B1>(out _))
                {
                    Assert.Fail("Dependent modules of module not triggered by command-line argument must not load");
                }
                if (!container.TryResolve<C>(out _))
                {
                    Assert.Fail("Module triggered by command-line argument must load");
                }
            }
        }
    }
}
